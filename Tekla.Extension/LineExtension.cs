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
            double angleRadians;
            double stepDegree = arc.Angle / steps;
            Point[] points = new Point[steps + 1];
            for (int i = 0; i <= steps; i++)
            {
                angleRadians = i * stepDegree;
                points[i] = RotatePointAroundAxis(new Vector(arc.StartPoint - arc.CenterPoint), arc.CenterPoint, arc.Normal, angleRadians);
            }
            return points;
        }
        public static Point RotatePointAroundAxis(Vector startDirection, Point center, Vector axis, double angleRadians)
        {
            axis.Normalize();
            // Apply Rodrigues' rotation formula
            double cosTheta = Math.Cos(angleRadians);
            double sinTheta = Math.Sin(angleRadians);
            double newX = center.X + (cosTheta + (1 - cosTheta) * axis.X * axis.X) * startDirection.X + ((1 - cosTheta) * axis.X * axis.Y - axis.Z * sinTheta) * startDirection.Y + ((1 - cosTheta) * axis.X * axis.Z + axis.Y * sinTheta) * startDirection.Z;
            double newY = center.Y + ((1 - cosTheta) * axis.Y * axis.X + axis.Z * sinTheta) * startDirection.X + (cosTheta + (1 - cosTheta) * axis.Y * axis.Y) * startDirection.Y + ((1 - cosTheta) * axis.Y * axis.Z - axis.X * sinTheta) * startDirection.Z;
            double newZ = center.Z + ((1 - cosTheta) * axis.Z * axis.X - axis.Y * sinTheta) * startDirection.X + ((1 - cosTheta) * axis.Z * axis.Y + axis.X * sinTheta) * startDirection.Y + (cosTheta + (1 - cosTheta) * axis.Z * axis.Z) * startDirection.Z;

            return new Point(newX, newY, newZ);
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
