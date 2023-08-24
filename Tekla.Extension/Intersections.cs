﻿using System;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;

namespace Tekla.Extension;
public static class Intersections
{
    public static IReadOnlyCollection<Point> GetIntersectionPoints(IReadOnlyList<Point> polygon1, IReadOnlyList<Point> polygon2)
    {
        var lineSegment1 = polygon1.GetLineSegmentsOfPolygon();
        var lineSegment2 = polygon2.GetLineSegmentsOfPolygon();
        return GetIntersectionPoints(lineSegment1, lineSegment2);
    }

    public static IReadOnlyCollection<Point> GetIntersectionPoints(IReadOnlyCollection<LineSegment> segments1, IReadOnlyCollection<LineSegment> segments2)
    {
        List<Point> result = new();
        foreach (LineSegment segment1 in segments1)
        {
            foreach (LineSegment segment2 in segments2)
            {
                if (IsLineSegmentsIntersect(segment1, segment2, out Point point))
                    result.Add(point);
            }
        }
        return result;
    }

    public static bool IsLineSegmentsIntersect(LineSegment segment1, LineSegment segment2, out Point intersectionPoint)
    {
        intersectionPoint = null;

        double x1 = segment1.Point1.X, y1 = segment1.Point1.Y;
        double x2 = segment1.Point2.X, y2 = segment1.Point2.Y;
        double x3 = segment2.Point1.X, y3 = segment2.Point1.Y;
        double x4 = segment2.Point2.X, y4 = segment2.Point2.Y;

        double denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

        if (denominator == 0)
        {
            return false; // Parallel or coincident lines
        }

        double numerator1 = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denominator;
        double numerator2 = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / denominator;

        if (numerator1 >= 0 && numerator1 <= 1 && numerator2 >= 0 && numerator2 <= 1)
        {
            double x = x1 + numerator1 * (x2 - x1);
            double y = y1 + numerator1 * (y2 - y1);
            intersectionPoint = new Point(x, y, 0);
            return true;
        }

        return false;
    }
}
