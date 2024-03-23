using System;
using System.Linq;
using Tekla.Structures.Catalogs;

namespace Tekla.Extension;
/// <summary>
/// Helps to work with catalog and get data from profiles.
/// </summary>
public static class ProfileExtension
{
    public static double GetProfileAttribute(string profile, string attribute)
    {
        LibraryProfileItem libraryProfileItem = new();
        libraryProfileItem.ProfileName = profile;
        if (libraryProfileItem.Select())
        {
            ProfileItemParameter parametr = libraryProfileItem.aProfileItemParameters
                .Cast<ProfileItemParameter>()
                .Where(p => string.Equals(p.Property, attribute, StringComparison.InvariantCulture))
                .FirstOrDefault();

            return parametr is null ? 0 : parametr.Value;
        }
        else
        {
            ParametricProfileItem parametricProfileItem = new();
            parametricProfileItem.ProfilePrefix = profile;
            bool isok = parametricProfileItem.Select();

            if (!isok)
                return 0;

            ProfileItemParameter parametr = parametricProfileItem.aProfileItemParameters
                .Cast<ProfileItemParameter>()
                .Where(p => string.Equals(p.Property, attribute))
                .FirstOrDefault();

            return parametr is null ? 0 : parametr.Value;
        }
    }
    public static double GetProfileHeight(string profile)
    {
        return GetProfileAttribute(profile, "HEIGHT");
    }
    public static double GetProfileWidth(string profile)
    {
        return GetProfileAttribute(profile, "WIDTH");
    }
    public static bool IsProfileExist(string profile)
    {
        LibraryProfileItem libraryProfileItem = new();
        libraryProfileItem.ProfileName = profile;
        bool isStatic = libraryProfileItem.Select();
        if (isStatic)
            return isStatic;
        else
        {
            ParametricProfileItem parametricProfileItem = new();
            return parametricProfileItem.Select();
        }
    }
    /// <summary>
    /// Get width of plate without inserting in tekla. PL10 will return 10.
    /// </summary>
    /// <param name="profile"></param>
    /// <returns>Width of profile</returns>
    public static double GetPlateWidthByProfile(string profile)
    {
        ParametricProfileItem parametric = new();
        parametric.ProfilePrefix = profile;
        parametric.Select();

        foreach (ProfileItemParameter item in parametric.aProfileItemParameters)
        {
            return item.Value;
        }
        return 0;
    }
}
