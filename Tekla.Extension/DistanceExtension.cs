using System.Collections.Generic;
using System.Globalization;
using TSD = Tekla.Structures.Datatype;


namespace Tekla.Extension;
public static class DistanceExtension
{
    public static IReadOnlyList<double> GetDistances(this string distances)
    {
        List<double> result = new();

        foreach (TSD.Distance distance in TSD.DistanceList.Parse(distances, CultureInfo.InvariantCulture, TSD.Distance.CurrentUnitType))
            result.Add(distance.ConvertTo(TSD.Distance.CurrentUnitType));

        return result;
    }
}
