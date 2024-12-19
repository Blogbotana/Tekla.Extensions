﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Extension.Enums;
using Tekla.Extension.Services;
using Tekla.Structures;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Solid;
using SR = System.Reflection;

namespace Tekla.Extension;

/// <summary>
/// Class for working with <see cref="Part"/> in Tekla Structures 
/// </summary>
public static class PartExtension
{
    /// <summary>
    /// Get the <see cref="OBB"/> of the <see cref="Part"/>
    /// </summary>
    /// <param name="part">Part for calculating OBB</param>
    /// <param name="offset">Offset in all directions if need</param>
    /// <returns>Orientated Bounding Box of the part</returns>
    /// <exception cref="Exception">If there's no that profile</exception>
    public static OBB GetPartOBB(this Part part, double offset = 0.0)
    {
        CoordinateSystem coordinateSystem = part.GetCoordinateSystem();
        Vector direction = coordinateSystem.AxisX.Cross(coordinateSystem.AxisY);
        Solid solid = part.GetSolid(Solid.SolidCreationTypeEnum.NORMAL);
        IEnumerable<Point> points = solid.GetEdgeEnumerator()
            .ToIEnumerable<Edge>()
            .Select(e => e.StartPoint);

        if (points.Count() == 0)
        {
            throw new Exception($"Tekla don't see a solid of the object guid:{part.Identifier.GUID}");
        }

        Point maximumPoint = solid.MaximumPoint;
        Point minimumPoint = solid.MinimumPoint;

        Point originPoint = maximumPoint.GetCenterPoint(minimumPoint);

        Line XLine = new(originPoint, coordinateSystem.AxisX);
        Line YLine = new(originPoint, coordinateSystem.AxisY);
        Line ZLine = new(originPoint, direction);

        Point remotePointX = PointExtension.GetRemotePoint(originPoint, points.Select(p => Projection.PointToLine(p, XLine)));
        Point remotePointY = PointExtension.GetRemotePoint(originPoint, points.Select(p => Projection.PointToLine(p, YLine)));
        Point remotePointZ = PointExtension.GetRemotePoint(originPoint, points.Select(p => Projection.PointToLine(p, ZLine)));

        double extentX = Distance.PointToPoint(originPoint, remotePointX) + offset;
        double extentY = Distance.PointToPoint(originPoint, remotePointY) + offset;
        double extentZ = Distance.PointToPoint(originPoint, remotePointZ) + offset;

        return new OBB(originPoint, XLine.Direction, YLine.Direction, ZLine.Direction, extentX, extentY, extentZ);
    }
    /// <summary>
    /// Get the <see cref="AABB"/> of the <see cref="Part"/>
    /// </summary>
    /// <param name="part">Part for calculating AABB</param>
    /// <param name="offset">Offset in all directions if need</param>
    /// <returns>Absolute bounding box of the part</returns>
    public static AABB GetPartAABB(this Part part, double offset = 0.0)
    {
        Solid solid = part.GetSolid(Solid.SolidCreationTypeEnum.RAW);
        Vector vectorOffset = new Vector(1, 1, 1) * offset;
        Point min = PointExtension.MinPoint;
        Point max = PointExtension.MaxPoint;
        PointExtension.ComparePoints(solid.MinimumPoint, min, (x1, x2) => x1 > x2);
        PointExtension.ComparePoints(solid.MaximumPoint, min, (x1, x2) => x1 > x2);
        PointExtension.ComparePoints(solid.MinimumPoint, max, (x1, x2) => x1 < x2);
        PointExtension.ComparePoints(solid.MaximumPoint, max, (x1, x2) => x1 < x2);

        Point maximumPoint = max + vectorOffset;
        Point minimumPoint = min - vectorOffset;
        return new AABB(maximumPoint, minimumPoint);
    }

    public static double GetWeight(this Part part)
    {
        return part.GetReportProperty<double>("profile weight");
    }
    public static double GetHeight(this ModelObject part)
    {
        return part.GetReportProperty<double>("HEIGHT");
    }
    public static double GetLength(this ModelObject part)
    {
        return part.GetReportProperty<double>("LENGTH");
    }
    public static Tekla.Structures.Drawing.SinglePartDrawing GetPartDrawing(this Part part)
    {
        int id = part.GetReportProperty<int>("DRAWING.ID");
        if (id > 0)
        {
            Identifier thisIdentifier = new(id);
            Tekla.Structures.Drawing.SinglePartDrawing singlePartDrawing = new(part.Identifier);
            singlePartDrawing.GetType()
                .GetProperty("Identifier", SR.BindingFlags.Instance | SR.BindingFlags.Public | SR.BindingFlags.NonPublic)
                .SetValue(singlePartDrawing, thisIdentifier);
            _ = singlePartDrawing.Select();
        }
        return null;
    }
    public static bool ArePartsBolted(this Part part1, Part part2)
    {
        IEnumerable<Guid> bolts1 = part1.GetBolts().ToIEnumerable<BoltGroup>().Select(b => b.Identifier.GUID);
        IEnumerable<Guid> bolts2 = part2.GetBolts().ToIEnumerable<BoltGroup>().Select(b => b.Identifier.GUID);
        return bolts1.Intersect(bolts2).Count() > 0;
    }
    public static bool ArePartWelded(this Part part1, Part part2)
    {
        IEnumerable<Guid> welds1 = part1.GetWelds().ToIEnumerable<BaseWeld>().Select(b => b.Identifier.GUID);
        IEnumerable<Guid> welds2 = part2.GetWelds().ToIEnumerable<BaseWeld>().Select(b => b.Identifier.GUID);
        return welds1.Intersect(welds2).Count() > 0;
    }
    /// <summary>
    /// Gets profile type property and returns custom enum to work with profile type filtering.
    /// </summary>
    public static ProfileType GetProfileType(this Part part)
    {
        string profTypeStr = GetProfileTypeString(part);
        return ProfileTypeEnumConverter.GetProfileTypeFromString(profTypeStr);
    }
    public static string GetProfileTypeString(this ModelObject part)
    {
        return part.GetReportProperty<string>("PROFILE_TYPE");
    }
    public static Point GetStartPoint(this Part part)
    {
        double x = part.GetReportProperty<double>("START_X");
        double y = part.GetReportProperty<double>("START_Y");
        double z = part.GetReportProperty<double>("START_Z");
        return new Point(x, y, z);
    }
    public static Point GetEndPoint(this Part part)
    {
        double x = part.GetReportProperty<double>("END_X");
        double y = part.GetReportProperty<double>("END_Y");
        double z = part.GetReportProperty<double>("END_Z");
        return new Point(x, y, z);
    }
    public static LineSegment GetCenterLineSegment(this Part part, bool withCutsFittings = true)
    {
        ArrayList centerLine = part.GetCenterLine(withCutsFittings);
        Point point1 = centerLine[0] as Point;
        Point point2 = centerLine[1] as Point;
        return new LineSegment(point1, point2);
    }
}
