using System;
using Tekla.Structures.Geometry3d;

namespace Tekla.Extension
{
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
    }
}
