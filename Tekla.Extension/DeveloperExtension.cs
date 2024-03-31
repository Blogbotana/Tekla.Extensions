using Tekla.Structures;

namespace Tekla.Extension;
/// <summary>
/// Class for developers in Tekla
/// </summary>
public static class DeveloperExtension
{
    /// <summary>
    /// Define is developer mode on. Static classes work different in this mode
    /// </summary>
    /// <returns></returns>
    public static bool IsDeveloperMode()
    {
        bool isModeOn = false;
        _ = TeklaStructuresSettings.GetAdvancedOption("XS_PLUGIN_DEVELOPER_MODE", ref isModeOn);
        return isModeOn;
    }
}
