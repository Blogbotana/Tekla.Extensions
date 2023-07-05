using System;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for drawing temporary graphics in Tekla Structures
    /// </summary>
    public static class Drawer
    {
        #region Fields 
        private static GraphicsDrawer graphicsDrawer = new();
        #endregion
        #region Colors
        /// <summary>
        /// White color
        /// </summary>
        public static Color White => new(1.0, 1.0, 1.0);
        /// <summary>
        /// Black color
        /// </summary>
        public static Color Black => new(0.0, 0.0, 0.0);
        /// <summary>
        /// Blue color
        /// </summary>
        public static Color Blue => new(0.0, 0.0, 1.0);
        /// <summary>
        /// Red color
        /// </summary>
        public static Color Red => new(1.0, 0.0, 0.0);
        /// <summary>
        /// Green color
        /// </summary>
        public static Color Green => new(0.0, 1.0, 0.0);
        /// <summary>
        /// Purple color
        /// </summary>
        public static Color Purple => new(1.0, 0.0, 1.0);
        /// <summary>
        /// Cyan color
        /// </summary>
        public static Color Cyan => new(0.0, 1.0, 1.0);
        /// <summary>
        /// Orange color
        /// </summary>
        public static Color Orange => new(1.0, 0.6, 0.3);
        /// <summary>
        /// DarkBlue color
        /// </summary>
        public static Color DarkBlue => new(0.0, 0.0, 0.4);
        #endregion
        #region Line Methods
        /// <summary>
        /// Draws the line by two points
        /// </summary>
        /// <param name="point1">First point</param>
        /// <param name="point2">Second point</param>
        /// <param name="color"></param>
        public static void DrawLine(Point point1, Point point2, Color color)
        {
            _ = graphicsDrawer.DrawLineSegment(point1, point2, color);
        }
        /// <summary>
        /// Draws the line by two points
        /// </summary>
        /// <param name="point1">First point</param>
        /// <param name="point2">Second point</param>
        public static void DrawLine(Point point1, Point point2)
        {
            DrawLine(point1, point2, Black);
        }
        /// <summary>
        /// Draws the line by line segment
        /// </summary>
        /// <param name="segment">Line segment to be drawn</param>
        /// <param name="color"></param>
        public static void DrawLine(this LineSegment segment, Color color)
        {
            DrawLine(segment.Point1, segment.Point2, color);
        }
        /// <summary>
        /// Draws the line by line segment
        /// </summary>
        /// <param name="segment"></param>
        public static void DrawLine(this LineSegment segment)
        {
            DrawLine(segment.Point1, segment.Point2, Black);
        }
        /// <summary>
        /// Draw the line
        /// </summary>
        /// <param name="line">The line to be drawn</param>
        /// <param name="color">The color of the line</param>
        /// <param name="offset">Offset from origin point</param>
        public static void DrawLine(this Line line, Color color, double offset = 1000.0)
        {
            Vector vector = line.Direction.GetNormal();
            _ = vector.Normalize(offset);
            Point point1 = new(line.Origin + vector);
            Point point2 = new(line.Origin - vector);
            DrawLine(point1, point2, color);
        }
        /// <summary>
        /// Draw the line
        /// </summary>
        /// <param name="line">The line to be drawn</param>
        /// <param name="offset">Offset from origin point</param>
        public static void DrawLine(this Line line, double offset = 1000.0)
        {
            DrawLine(line, Black, offset);
        }
        #endregion
        #region Point Methods
        /// <summary>
        /// Draws the point in model
        /// </summary>
        /// <param name="point">The point to be drawn</param>
        /// <param name="text">The text for point</param>
        /// <param name="textColor">The color of text</param>
        public static void DrawPoint(this Point point, string text, Color textColor)
        {
            _ = graphicsDrawer.DrawText(point, text ?? "p", textColor);
        }
        /// <summary>
        /// Draws the point in model
        /// </summary>
        /// <param name="point">The point to be drawn</param>
        /// <param name="text">The text for point</param>
        public static void DrawPoint(this Point point, string text)
        {
            DrawPoint(point, text, Blue);
        }
        /// <summary>
        /// Draws the point in model
        /// </summary>
        /// <param name="point">The point to be drawn</param>
        public static void DrawPoint(this Point point)
        {
            DrawPoint(point, "p", Blue);
        }
        /// <summary>
        /// Draws the points of array
        /// </summary>
        /// <param name="points">The collection of points</param>
        /// <param name="color">The color of points</param>
        public static void DrawPoints(this IEnumerable<Point> points, Color color)
        {
            int counter = 0;
            foreach (Point point in points)
            {
                DrawPoint(point, counter.ToString(), color);
                counter++;
            }
        }
        /// <summary>
        /// Draws the points of array
        /// </summary>
        /// <param name="points">The collection of points</param>
        public static void DrawPoints(this IEnumerable<Point> points)
        {
            DrawPoints(points, Blue);
        }
        #endregion
    }
}
