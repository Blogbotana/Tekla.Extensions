using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="Assembly"/> in Tekla Structures
    /// </summary>
    public static class AssemblyExtension
    {
        public static AABB GetBoundingBox(this Assembly assembly)
        {
            Point min = PointExtension.MinPoint;
            Point max = PointExtension.MaxPoint;

            IEnumerable<Part> parts = assembly.GetAllPartsOfAssembly().Cast<Part>();

            foreach (Part item in parts)
            {
                Solid solid = item.GetSolid();
                Point min_point = solid.MinimumPoint;
                Point max_point = solid.MaximumPoint;

                PointExtension.ComparePoints(min_point, min, (x1, x2) => x1 > x2);
                PointExtension.ComparePoints(max_point, max, (x1, x2) => x1 < x2);
            }

            return new AABB(min, max);
        }

        public static ICollection<Part> GetAllPartsOfAssembly(this Assembly assembly, bool isIncludeSubAssemblies = false)
        {
            ICollection<Assembly> subAssemblies = isIncludeSubAssemblies ? GetSubAssemblies(assembly) : (new Assembly[] { assembly });
            List<Part> parts = new();

            foreach (Assembly item in subAssemblies)
            {
                parts.Add(item.GetMainPart() as Part);
                parts.AddRange(item.GetSecondaries().Cast<Part>());
            }

            return parts;
        }
        private static ICollection<Assembly> GetSubAssemblies(Assembly assembly)
        {
            ArrayList subAssemblies = assembly.GetSubAssemblies();

            if (subAssemblies.Count == 0)
            {
                return new Assembly[] { assembly };
            }
            else
            {
                List<Assembly> assemblies = new();
                foreach (Assembly item in subAssemblies)
                {
                    assemblies.AddRange(GetSubAssemblies(item));
                }
                return assemblies;
            }
        }
        public static OBB GetOBB(this Assembly assembly)
        {
            IEnumerable<OBB> obbs = assembly.GetAllPartsOfAssembly().Select(p => p.GetPartOBB());
            return OBBExtension.CombineOBBs(obbs);
        }
    }
}
