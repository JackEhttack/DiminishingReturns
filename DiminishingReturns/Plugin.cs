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

    public MoonTracker Service;

    public Plugin()
    {
        Instance = this;
    }

    private void Awake()
    {
        Service = new MoonTracker();

        Log.LogInfo($"Applying patches...");
        ScrapModifierPatch.ApplyPatches();
        Log.LogInfo($"Applied all patches!");
    }

}
