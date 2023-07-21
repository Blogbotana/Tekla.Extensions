using System;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Solid;

namespace Tekla.Extension
{
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
            System.Collections.Generic.IEnumerable<Point> points = solid.GetEdgeEnumerator()
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

        public static double GetWeight(this Part part)
        {
            return part.GetReportProperty<double>("profile weight");
        }
    }
}
