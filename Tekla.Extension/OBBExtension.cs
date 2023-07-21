﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="OBB"/>
    /// </summary>
    public static class OBBExtension
    {
        public static OBB CombineOBBs(this IEnumerable<OBB> obbs, double offset = 0)
        {
            if (obbs.Count() < 1)
                return new OBB();

            OBB firstOBB = obbs.First();
            CoordinateSystem coordinateSystem = new(firstOBB.Center, firstOBB.Axis0, firstOBB.Axis1);
            Matrix matrix = MatrixFactory.ToCoordinateSystem(coordinateSystem);

            var points = obbs.Select(obb => obb is null ? Array.Empty<Point>() : obb.ComputeVertices())
                .SelectMany(v => v)
                .OrderBy(p => p.X)
                .ThenBy(p => p.Y)
                .ThenBy(p => p.Z)
                .Select(p => matrix.Transform(p)).ToList();

            AABB polygonAABB = PointExtension.GetPolygonAABB(points);
            Point centerPoint = polygonAABB.GetCenterPoint();
            double extentX = polygonAABB.MaxPoint.X - centerPoint.X;
            double extentY = polygonAABB.MaxPoint.Y - centerPoint.Y;
            double extentZ = polygonAABB.MaxPoint.Z - centerPoint.Z;
            Matrix matrixBack = MatrixFactory.FromCoordinateSystem(coordinateSystem);
            Point transformedMinPoint = matrixBack.Transform(polygonAABB.MinPoint);
            Point transformedMaxPoint = matrixBack.Transform(polygonAABB.MaxPoint);
            Point transformedCenterPoint = transformedMinPoint.GetCenterPoint(transformedMaxPoint);

            return new(transformedCenterPoint, coordinateSystem.AxisX, coordinateSystem.AxisY, coordinateSystem.AxisX.Cross(coordinateSystem.AxisY),
                extentX + offset, extentY + offset, extentZ + offset);
        }
        public static Point GetMaximumPoint(this OBB obb)
        {
            Point max = PointExtension.MaxPoint;
            foreach (Point point in obb.ComputeVertices())
            {
                PointExtension.ComparePoints(point, max, (x, y) => x > y);
            }
            return max;
        }
        public static Point GetMinimumPoint(this OBB obb)
        {
            Point min = PointExtension.MaxPoint;
            foreach (Point point in obb.ComputeVertices())
            {
                PointExtension.ComparePoints(point, min, (x, y) => x < y);
            }
            return min;
        }
    }
}
