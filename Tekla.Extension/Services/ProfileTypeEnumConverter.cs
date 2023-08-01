using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekla.Extension.Services;

public static class ProfileTypeEnumConverter
{

    /// <summary>
    /// Converts string to ProfileType.
    /// </summary>
    /// <param name="str">String value API is working with.</param>
    public static ProfileType GetProfileTypeFromString(string str)
    {
        switch (str)
        {
            case "B":
                return ProfileType.Plate;
            case "I":
                return ProfileType.Ibeam;
            case "L":
                return ProfileType.Angle;
            case "U":
                return ProfileType.Channel;
            case "RU":
                return ProfileType.RoundBar;
            case "RO":
                return ProfileType.RoundTube;
            case "M":
                return ProfileType.RectangularTube;
            case "C":
                return ProfileType.CFChannel;
            case "T":
                return ProfileType.Tbeam;
            case "Z":
                return ProfileType.Zbeam;
            default:
                return ProfileType.Unknown;
        }
    }

    /// <summary>
    /// Converts ProfileType back to string.
    /// </summary>
    /// <param name="profileType"></param>
    /// <returns>String corresponding to Enum</returns>
    public static string GetStringValueFromProfileType(ProfileType profileType)
    {
        switch (profileType)
        {
            case ProfileType.Plate:
                return "B";
            case ProfileType.Ibeam:
                return "I";
            case ProfileType.Angle:
                return "L";
            case ProfileType.Channel:
                return "U";
            case ProfileType.RoundBar:
                return "RU";
            case ProfileType.RoundTube:
                return "RO";
            case ProfileType.RectangularTube:
                return "M";
            case ProfileType.CFChannel:
                return "C";
            case ProfileType.Tbeam:
                return "T";
            case ProfileType.Zbeam:
                return "Z";
            default:
                return "Unknown";
        }
    }
}
