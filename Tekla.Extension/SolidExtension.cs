using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Solid;

namespace Tekla.Extension
{
    public static class SolidExtension
    {
        public static ICollection<Point> GetPoints(this Solid solid)
        {
            return solid.GetEdgeEnumerator()
                .ToIEnumerable<Edge>()
                .SelectMany(e => new Point[] { e.StartPoint, e.EndPoint })
                .ToHashSet();

        }
    }
}
