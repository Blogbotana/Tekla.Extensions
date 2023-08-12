using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace Tekla.Extension;
/// <summary>
/// Class for working with 3d geometry
/// </summary>
public static class GeometryExtension
{
    public static bool IsPointInLineSegment3d(LineSegment lineSegment, Point testPoint, double tolerance = 0.01)
    {
        return IsPointInLineSegment3d(lineSegment.Point1, lineSegment.Point2, testPoint, tolerance);
    }
    public static bool IsPointInLineSegment3d(Point startPoint, Point endPoint, Point testPoint, double tolerance = 0.01)
    {
        Vector lineVector = new Vector(endPoint - startPoint);
        Vector testPointVector = new Vector(testPoint - startPoint);

        double dotProduct = lineVector.Dot(testPointVector);
        double magnitude = lineVector.GetLengthSquared();

        if (dotProduct < 0 || dotProduct > magnitude)
            return false;
        else
        {
            double distance = testPointVector.Cross(lineVector).GetLength() / lineVector.GetLength();
            return distance <= tolerance;
        }
    }
    public static bool IsPointInsidePolygon(this Point testPoint, IReadOnlyCollection<Point> polygon, bool includeLine = true)
    {
        int windingNumber = 0;
        for (int i = 0; i < polygon.Count; i++)
        {
            int j = i == 0 ? polygon.Count - 1 : i - 1;
            Point point1 = polygon.ElementAt(i);
            Point point2 = polygon.ElementAt(j);    

            if (point1.Y <= testPoint.Y)
            {
                if (point2.Y > testPoint.Y && IsPointLeftOnEdge(point1, point2, testPoint))
                {
                    windingNumber++;
                }
            }
            else
            {
                if (point2.Y <= testPoint.Y && IsPointLeftOnEdge(point2, point1, testPoint))
                {
                    windingNumber--;
                }
            }


            if(includeLine)
            {
                if(IsPointInLineSegment3d(point1, point2, testPoint))
                    return true;
            }
        }
        return windingNumber != 0;
    }

    private static bool IsPointLeftOnEdge(Point point1, Point point2, Point testPoint)
    {
        return ((point2.X - point1.X) * (testPoint.Y - point1.Y)) - ((testPoint.X - point1.X) * (point2.Y - point1.Y)) > 0;
    }

}
