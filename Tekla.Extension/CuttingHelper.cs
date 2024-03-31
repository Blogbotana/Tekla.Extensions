using System;
using System.Collections.Generic;
using System.Globalization;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension;
/// <summary>
/// Helps to work with cuttings, fittings in Tekla
/// </summary>
public static class CuttingHelper
{
    /// <summary>
    /// Cut two beams by bisector
    /// </summary>
    /// <param name="beam1"></param>
    /// <param name="beam2"></param>
    /// <returns>Inserted objects</returns>
    public static IEnumerable<ModelObject> CutTwoBeamsBisector(Beam beam1, Beam beam2)
    {
        Plane plane = new();
        Line line1 = new(beam1.StartPoint, beam1.EndPoint);
        Line line2 = new(beam2.StartPoint, beam2.EndPoint);
        LineSegment segment = Intersection.LineToLine(line1, line2);
        if (segment is null)
            return new List<ModelObject>();

        plane.Origin = segment.StartPoint;
        Vector bicetor = (line1.Direction.GetLength() * line2.Direction.Negative()).Add(line2.Direction.GetLength() * line1.Direction).Subtract(line1.Direction);

        plane.AxisX = bicetor.GetNormal() * 300;
        plane.AxisY = line1.Direction.Cross(line2.Direction).GetNormal() * 300;

        Fitting fitting1 = new();
        Fitting fitting2 = new();
        fitting1.Plane = fitting2.Plane = plane;
        fitting1.Father = beam1;
        fitting2.Father = beam2;
        _ = fitting1.Insert();
        _ = fitting2.Insert();
        return new ModelObject[] { fitting1, fitting2 };
    }
    /// <summary>
    /// Cut the beam by another beam by fitting and center point of beam
    /// </summary>
    /// <param name="beamToCut"></param>
    /// <param name="beamCuttedBy"></param>
    /// <returns>Created object</returns>
    public static ModelObject CutOneBeamByAnotherFitting(Beam beamToCut, Beam beamCuttedBy)
    {
        Plane plane = new();
        Line line1 = new(beamToCut.StartPoint, beamToCut.EndPoint);
        Line line2 = new(beamCuttedBy.StartPoint, beamCuttedBy.EndPoint);
        return CutBeamsFitting(beamToCut, beamCuttedBy, plane, line1, line2);
    }
    private static ModelObject CutBeamsFitting(Beam beamToCut, Part beamCuttedBy, Plane plane, Line line1, Line line2)
    {
        IEnumerable<Point> points = beamCuttedBy.GetPartOBB().ComputeVertices();
        Point nearest = PointExtension.GetNearestPoint(beamToCut.GetCenterPoint(), points);
        plane.Origin = nearest;
        plane.AxisX = line2.Direction.GetNormal() * 300;
        plane.AxisY = line2.Direction.Cross(line1.Direction).GetNormal() * 300;
        Fitting fitting = new();
        fitting.Father = beamToCut;
        fitting.Plane = plane;
        _ = fitting.Insert();
        return fitting;
    }
    public static ModelObject CutOneBeamByAnotherPlane(Beam beamToCut, Beam beamCuttedBy, Vector vectorCross)
    {
        Plane plane = new();
        Line line2 = new(beamCuttedBy.StartPoint, beamCuttedBy.EndPoint);
        IEnumerable<Point> points = beamCuttedBy.GetPartOBB().ComputeVertices();
        Point nearest = PointExtension.GetNearestPoint(beamToCut.GetCenterPoint(), points);
        plane.Origin = nearest;
        plane.AxisX = line2.Direction.GetNormal() * 300;
        plane.AxisY = vectorCross.GetNormal() * 300;
        CutPlane fitting = new();
        fitting.Father = beamToCut;
        fitting.Plane = plane;
        _ = fitting.Insert();
        return fitting;
    }
    public static ModelObject CutBeamByTwoPoints(Part partToCut, Point startPoint, Point endPoint, Vector cross)
    {
        Plane plane = new();
        plane.Origin = startPoint;
        plane.AxisX = new Vector(endPoint - startPoint);
        plane.AxisY = cross;
        Fitting fitting = new();
        fitting.Father = partToCut;
        fitting.Plane = plane;
        _ = fitting.Insert();
        return fitting;
    }
    public static ModelObject CutPartByPolygon(Part part, IReadOnlyCollection<ContourPoint> points, double thickness = 200)
    {
        ContourPlate contourPlate = new();
        contourPlate.Profile.ProfileString =thickness.ToString("0.0", CultureInfo.InvariantCulture);
        contourPlate.Material.MaterialString = "ANTIMATERIAL";
        contourPlate.Class = BooleanPart.BooleanOperativeClassName;
        contourPlate.Position.Depth = Position.DepthEnum.MIDDLE;

        foreach (ContourPoint point in points)
        {
            _ = contourPlate.AddContourPoint(point);
        }

        _ = contourPlate.Insert();

        BooleanPart booleanPart = new();
        booleanPart.Father = part;
        _ = booleanPart.SetOperativePart(contourPlate);

        _ = booleanPart.Insert();
        _ = contourPlate.Delete();
        return booleanPart;
    }
    public static ModelObject CutPartByDetail(Part part, LineSegment segment, string profile, Position position)
    {
        BooleanPart booleanPart = new();
        booleanPart.Father = part;

        Beam beam = new(segment.Point1, segment.Point2);
        beam.Profile.ProfileString = profile;
        beam.Position = position;
        beam.Material.MaterialString = "ANTIMATERIAL";
        beam.Class = BooleanPart.BooleanOperativeClassName;

        beam.Insert();

        booleanPart.SetOperativePart(beam);
        booleanPart.Insert();

        beam.Delete();

        return booleanPart;
    }
}
