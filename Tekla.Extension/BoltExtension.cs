using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Class for working with <see cref="BoltGroup"/>
    /// </summary>
    public static class BoltExtension
    {
        public static ICollection<Part> GetAllParts(this BoltGroup boltGroup)
        {
            List<Part> parts = new List<Part>();
            if (boltGroup.PartToBoltTo is not null)
                parts.Add(boltGroup.PartToBoltTo);
            if (boltGroup.PartToBeBolted is not null)
                parts.Add(boltGroup.PartToBeBolted);
            if (boltGroup.OtherPartsToBolt.Count > 0)
                parts.AddRange(boltGroup.OtherPartsToBolt.Cast<Part>().Where(p => p is not null));
            return parts;
        }

        public static ICollection<Point> GetBoltPoints(this BoltGroup boltGroup)
        {
            return boltGroup.BoltPositions.Cast<Point>().ToArray();
        }
        public static void FillFullInfoToBoltGroup(this BoltGroup boltGroup, BoltSettings boltSettings, Part partToBoltTo, Part PartToBeBolted = null, IEnumerable<Part> otherParts = null)
        {
            boltGroup.PartToBoltTo = partToBoltTo;
            if (PartToBeBolted is not null)
                boltGroup.PartToBeBolted = partToBoltTo;
            if(otherParts is not null)
            {
                foreach (Part part in otherParts)
                    if(part is not null)
                        boltGroup.AddOtherPartToBolt(part);
            }

            boltGroup.BoltStandard = boltSettings.BoltStandard;
            boltGroup.BoltSize = boltSettings.BoltSize;
            boltGroup.BoltType = boltSettings.BoltType;
            boltGroup.ConnectAssemblies = boltSettings.ConnectAssemblies;
            boltGroup.CutLength = boltSettings.CutLength;
            boltGroup.Tolerance = boltSettings.Tolerance;
            boltGroup.ExtraLength = boltSettings.ExtraLength;

            if(boltSettings.BoltConfiguration.Length == 6)
            {
                boltGroup.Bolt = boltSettings.BoltConfiguration[0];
                boltGroup.Nut1 = boltSettings.BoltConfiguration[1];
                boltGroup.Nut2 = boltSettings.BoltConfiguration[2];
                boltGroup.Washer1 = boltSettings.BoltConfiguration[3];
                boltGroup.Washer2 = boltSettings.BoltConfiguration[4];
                boltGroup.Washer3 = boltSettings.BoltConfiguration[5];
            }
        }

        public class BoltSettings
        {
            public BoltGroup.BoltTypeEnum BoltType { get; set; }
            public double BoltSize { get; set; }
            public string BoltStandard { get; set; }
            public bool ConnectAssemblies { get; set; }
            public double ExtraLength { get; set; }
            public double CutLength { get; set; }
            public double Tolerance { get; set; }
            public bool[] BoltConfiguration { get; set; }
        }

        public static void SetPointsToBolt(this BoltGroup boltGroup, IEnumerable<Point> points)
        {
            boltGroup.FirstPosition = points.FirstOrDefault();
            boltGroup.SecondPosition = points.LastOrDefault();
        }
        public static void SetStartPointDxOffset(this BoltGroup boltGroup, double distance)
        {
            boltGroup.StartPointOffset = new Offset() { Dx = distance, Dy = 0, Dz = 0 };
        }

        public static double GetSumDistX(this BoltArray boltArray)
        {
            int boltDistXCount = boltArray.GetBoltDistXCount();
            double num = 0.0;
            for (int i = 0; i < boltDistXCount; i++)
            {
                num += boltArray.GetBoltDistX(i);
            }
            return num;
        }
        public static double GetSumDistY(this BoltArray boltArray)
        {
            int boltDistYCount = boltArray.GetBoltDistYCount();
            double num = 0.0;
            for (int i = 0; i < boltDistYCount; i++)
            {
                num += boltArray.GetBoltDistY(i);
            }
            return num;
        }
    }
}
