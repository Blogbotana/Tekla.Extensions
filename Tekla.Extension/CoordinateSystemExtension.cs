using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="CoordinateSystem"/>
    /// </summary>
    public static class CoordinateSystemExtension
    {

        public static Vector GetAxisZ(this CoordinateSystem cs)
        {
            return cs.AxisX.Cross(cs.AxisY);
        }

        public static TransformationPlane ToTransformationPlane(this CoordinateSystem cs)
        {
            return new TransformationPlane(cs);
        }

        public static Matrix MatrixToGlobal(this CoordinateSystem cs)
        {
            return MatrixFactory.FromCoordinateSystem(cs);
        }

        public static Matrix MatrixToLocal(this CoordinateSystem cs)
        {
            return MatrixFactory.ToCoordinateSystem(cs);
        }

        public static GeometricPlane ToGeometricPlane(this CoordinateSystem cs)
        {
            return new GeometricPlane(cs);
        }
    }
}
