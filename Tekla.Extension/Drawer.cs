﻿using System;
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
        private static readonly GraphicsDrawer graphicsDrawer = new();
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
        public static Color DarkGreen => new(0.0, 0.5, 0.0);
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
        public static void DrawLine(Point point1, Point point2, Color color = null)
        {
            color ??= Black;
            _ = graphicsDrawer.DrawLineSegment(point1, point2, color);
        }
        /// <summary>
        /// Draws the line by line segment
        /// </summary>
        /// <param name="segment">Line segment to be drawn</param>
        /// <param name="color"></param>
        public static void DrawLine(this LineSegment segment, Color color = null)
        {
            color ??= Black;
            DrawLine(segment.Point1, segment.Point2, color);
        }
        /// <summary>
        /// Draw the line
        /// </summary>
        /// <param name="line">The line to be drawn</param>
        /// <param name="color">The color of the line</param>
        /// <param name="offset">Offset from origin point</param>
        public static void DrawLine(this Line line, Color color = null, double offset = 1000.0)
        {
            color ??= Black;

            Vector vector = line.Direction.GetNormal();
            _ = vector.Normalize(offset);
            Point point1 = new(line.Origin + vector);
            Point point2 = new(line.Origin - vector);
            DrawLine(point1, point2, color);
        }
        #endregion
        #region Point Methods
        /// <summary>
        /// Draws the point in model
        /// </summary>
        /// <param name="point">The point to be drawn</param>
        /// <param name="text">The text for point</param>
        /// <param name="textColor">The color of text</param>
        public static void DrawPoint(this Point point, string text, Color textColor = null)
        {
            textColor ??= Blue;
            _ = graphicsDrawer.DrawText(point, text ?? "p", textColor);
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
        public static void DrawPoints(this IEnumerable<Point> points, Color color = null)
        {
            color ??= DarkBlue;

            int counter = 0;
            foreach (Point point in points)
            {
                DrawPoint(point, counter.ToString(), color);
                counter++;
            }
        }

        #endregion
        #region Arrow Methods
        public static void DrawCross(this Point point, Color colorOfCross = null, double length = 100)
        {
            colorOfCross ??= Black;
            double koef = Math.Sqrt(2);
            Point point1 = new(point);
            point1.Translate(-length * 0.5 * koef, -length * 0.5 * koef, 0);
            Point point2 = new(point);
            point2.Translate(length * 0.5 * koef, length * 0.5 * koef, 0);
            Point point3 = new(point);
            point3.Translate(-length * 0.5 * koef, length * 0.5 * koef, 0);
            Point point4 = new(point);
            point4.Translate(length * 0.5 * koef, -length * 0.5 * koef, 0);
            DrawLine(point1, point2, colorOfCross);
            DrawLine(point3, point4, colorOfCross);
        }

        public static void DrawArrow(Point point1, Point point2, Color color = null, double xSize = 100, double ySize = 100)
        {
            color ??= Red;

            Vector vectorArrow = new(point2 - point1);
            Vector vectorNormal = VectorExtension.Z;
            Vector vectorY = vectorNormal.Cross(vectorArrow);
            if (vectorY.IsNull())
            {
                vectorNormal = VectorExtension.Y;
                vectorY = vectorNormal.Cross(vectorArrow);
            }
            _ = vectorY.Normalize(ySize);
            Vector vectorX = new(vectorArrow);
            _ = vectorX.Normalize(xSize);
            Point pointOfTriangle1 = point2 - vectorX;
            Point pointOfTriangle2 = pointOfTriangle1 + vectorY;
            Point pointOfTriangle3 = pointOfTriangle1 - vectorY;

            DrawLine(point2, pointOfTriangle2, color);
            DrawLine(pointOfTriangle2, pointOfTriangle3, color);
            DrawLine(point2, pointOfTriangle3, color);
            DrawLine(point1, point2, color);
        }
        public static void DrawArrow(LineSegment segment, Color color = null)
        {
            color ??= Blue;
            DrawArrow(segment.Point1, segment.Point2, color);
        }
        public static void DrawArrow(Point point1, Vector vector, Color color = null)
        {
            color ??= Blue;
            DrawArrow(point1, point1 + vector, color);
        }
        public static void DrawArrow(Point point1, Vector vector, double length = 500, Color color = null)
        {
            color ??= Blue;
            DrawArrow(point1, point1 + (vector.GetNormal() * length), color);
        }
        public static void DrawVector(this Vector vector, Color color = null)
        {
            color ??= Red;
            DrawArrow(new Point(0, 0, 0), new Point(vector.X, vector.Y, vector.Z), color);
        }
        #endregion
        #region CoordinateSystem Methods
        public static void DrawCS(this CoordinateSystem coordinateSystem, Color colorOfText = null, string comment = "")
        {
            colorOfText ??= Black;
            double lineLength = 500;
            Vector vectorZ = coordinateSystem.AxisX.Cross(coordinateSystem.AxisY);
            _ = coordinateSystem.AxisX.Normalize(lineLength);
            _ = coordinateSystem.AxisY.Normalize(lineLength);
            _ = vectorZ.Normalize(lineLength);
            Point point1 = new(coordinateSystem.Origin + coordinateSystem.AxisX);
            Point point2 = new(coordinateSystem.Origin + coordinateSystem.AxisY);
            Point point3 = new(coordinateSystem.Origin + vectorZ);
            _ = graphicsDrawer.DrawLineSegment(coordinateSystem.Origin, point1, Red);
            _ = graphicsDrawer.DrawLineSegment(coordinateSystem.Origin, point2, Green);
            _ = graphicsDrawer.DrawLineSegment(coordinateSystem.Origin, point3, DarkBlue);
            point1.DrawPoint("X", colorOfText);
            point2.DrawPoint("Y", colorOfText);
            point3.DrawPoint("Z", colorOfText);
            coordinateSystem.Origin.DrawPoint(comment, colorOfText);
        }
        #endregion
        #region Polygons
        public static void DrawArc(this Arc arc, int steps = 10, Color color = null)
        {
            color ??= Blue;
            Point[] points = arc.GetPoints(steps);
            GraphicPolyLine polyline = new(color, 1, GraphicPolyLine.LineType.Solid)
            {
                PolyLine = new PolyLine(points)
            };
            graphicsDrawer.DrawPolyLine(polyline);
        }
        public static void DrawPolygon(this IEnumerable<LineSegment> polygon, Color color = null)
        {
            color ??= Black;
            foreach (LineSegment line in polygon)
            {
                DrawLine(line.Point1, line.Point2, color);
            }
        }
        public static void DrawPolygon(this IEnumerable<Point> polygon, Color color = null)
        {
            color ??= Blue;
            int count = polygon.Count();
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    DrawLine(polygon.Last(), polygon.First(), color);
                }
                else
                {
                    DrawLine(polygon.ElementAt(i), polygon.ElementAt(i + 1), color);
                }
            }
        }
        public static void DrawMesh(ICollection<Point> vertexes, ICollection<int> triangles)
        {
            Mesh mesh = new();
            foreach (Point vertex in vertexes)
            {
                _ = mesh.AddPoint(vertex);
            }

            for (int i = 0; i < triangles.Count; i += 3)
            {
                mesh.AddTriangle(triangles.ElementAt(i), triangles.ElementAt(i + 1), triangles.ElementAt(i + 2));
            }

            _ = graphicsDrawer.DrawMeshSurface(mesh, Green);
            _ = graphicsDrawer.DrawMeshLines(mesh, Black);
        }
        #endregion
        #region OBB and AABB
        public static void DrawObb(this OBB obb, Color color = null, bool drawPoints = false)
        {
            color ??= Black;
            Point[] array = obb.ComputeVertices();
            DrawBox(array, color, drawPoints);
        }
        public static void DrawAABB(this AABB aabb, Color color = null, bool drawPoints = false)
        {
            color ??= Black;
            Point[] array = aabb.ComputeVertices();
            DrawBox(array, color, drawPoints);
        }
        private static void DrawBox(Point[] array, Color color, bool drawPoints)
        {
            Point[] bottomPoints = new Point[]
               {
                    array[0],
                    array[1],
                    array[2],
                    array[3],
               };

            Point[] upperPoints = new Point[]
              {
                    array[4],
                    array[5],
                    array[6],
                    array[7],
              };

            var bottomLines = bottomPoints.GetLineSegmentsOfPolygon();
            var upperLines = upperPoints.GetLineSegmentsOfPolygon();

            bottomLines.DrawPolygon(color);
            upperLines.DrawPolygon(color);
            LineSegment[] sideLines = new LineSegment[]
            {
                new LineSegment(array[0], array[4]),
                new LineSegment(array[1], array[5]),
                new LineSegment(array[2], array[6]),
                new LineSegment(array[3], array[7]),
            };

            sideLines.DrawPolygon(color);

            if (drawPoints)
                DrawPoints(array);
        }
        #endregion
        #region Plane
        public static void DrawPlane(this Plane plane, double length = 100, Color color = null)
        {
            color ??= Blue;

            Vector vectorX = new(plane.AxisX);
            Vector vectorY = new(plane.AxisY);
            _ = vectorX.Normalize(length * 0.5);
            _ = vectorY.Normalize(length * 0.5);
            Point[] points = new Point[]
            {
                plane.Origin + vectorX + vectorY,
                plane.Origin + vectorX - vectorY,
                plane.Origin - vectorX - vectorY,
                plane.Origin - vectorX + vectorY
            };
            DrawSquareMesh(points, color);
        }
        public static void DrawGeometricPlane(this GeometricPlane geometricPlane, double length = 100, Color color = null)
        {
            color ??= Blue;
            var crossedVector1 = geometricPlane.Normal.Cross(VectorExtension.Z);
            if(crossedVector1.IsNull() || crossedVector1.IsEmpty())
                crossedVector1 = geometricPlane.Normal.Cross(VectorExtension.Y);

            var crossedVector2 = geometricPlane.Normal.Cross(crossedVector1);

            crossedVector1.Normalize(length * 0.5);
            crossedVector2.Normalize(length * 0.5);

            Point[] points = new Point[]
            {
                geometricPlane.Origin + crossedVector1 + crossedVector2,
                geometricPlane.Origin + crossedVector1 - crossedVector2,
                geometricPlane.Origin - crossedVector1 - crossedVector2,
                geometricPlane.Origin - crossedVector1 + crossedVector2
            };
            DrawSquareMesh(points, color);
        }
        private static void DrawSquareMesh(Point[] points, Color color)
        {
            if (points.Length != 4)
                return;
            Mesh mesh = new();
            mesh.AddPoint(points[0]);
            mesh.AddPoint(points[1]);
            mesh.AddPoint(points[2]);
            mesh.AddPoint(points[3]);
            mesh.AddTriangle(0, 1, 2);
            mesh.AddTriangle(2, 1, 0);
            mesh.AddTriangle(0, 2, 3);
            mesh.AddTriangle(3, 2, 0);

            graphicsDrawer.DrawMeshSurface(mesh, color);
        }
        #endregion
    }
}
