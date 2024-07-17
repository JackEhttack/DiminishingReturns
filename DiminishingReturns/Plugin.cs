using BepInEx;
using BepInEx.Logging;
using JackEhttack.patch;
using JackEhttack.service;

namespace JackEhttack;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; set; }

    public ManualLogSource Log => Instance.Logger;

    public static MoonTracker Service;
    internal static DRConfig Config { get; private set; } = null!;

    public Plugin()
    {
        Instance = this;
    }

    private void Awake()
    {
        Service = new MoonTracker();
        Config = new DRConfig(base.Config);

        Log.LogInfo($"Applying patches...");
        ScrapModifierPatch.ApplyPatches();
        TerminalPatch.ApplyPatches();
        Log.LogInfo($"Applied all patches!");
    }

}
