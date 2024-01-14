using System;
using Tekla.Structures.Geometry3d;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="Line"/>
    /// </summary>
    public static class LineExtension
    {
        public static Point[] GetPoints(this Arc arc, int steps = 10)
        {
            Point startPoint = arc.StartPoint;
            double angleRadians;
            double stepDegree = arc.Angle / steps;
            Point[] points = new Point[steps + 1];
            for (int i = 0; i <= steps; i++)
            {
                angleRadians = i * stepDegree;
                double newX = arc.CenterPoint.X + (startPoint.X - arc.CenterPoint.X) * Math.Cos(angleRadians) - (startPoint.Y - arc.CenterPoint.Y) * Math.Sin(angleRadians);
                double newY = arc.CenterPoint.Y + (startPoint.X - arc.CenterPoint.X) * Math.Sin(angleRadians) + (startPoint.Y - arc.CenterPoint.Y) * Math.Cos(angleRadians);
                points[i] = new Point(newX, newY);
            }
            return points;
        }
        public static LineSegment[] DevideBy(this LineSegment segment, int quantity)
        {
            double length = Distance.PointToPoint(segment.Point1, segment.Point2) / quantity;
            Vector stepVector = segment.GetDirectionVector() * length;
            LineSegment[] segments = new LineSegment[quantity];
            for (int i = 0; i < quantity; i++)
            {
                segments[i] = new(segment.Point1 + (stepVector * i), segment.Point1 + (stepVector * (i + 1)));
            }
            return segments;
        }

        public static Point GetCenterPoint(this LineSegment lineSegment)
        {
            return lineSegment.Point1.GetCenterPoint(lineSegment.Point2);
        }

        public static Line ToLine(this LineSegment lineSegment)
        {
            return new Line(lineSegment);
        }
    }
}
