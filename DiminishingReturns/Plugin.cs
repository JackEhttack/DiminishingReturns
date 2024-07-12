using BepInEx;
using BepInEx.Logging;
using JackEhttack.patch;
using JackEhttack.service;
using On.GameNetcodeStuff;

namespace JackEhttack;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; set; }

    public static ManualLogSource Log => Instance.Logger;

    public TemplateService Service;

    public Plugin()
    {
        Instance = this;
    }

    private void Awake()
    {
        Service = new TemplateService();
        Log.LogInfo($"Applying patches...");
        PlayerControllerBPatch.ApplyPatches();
        ShipLightsPatch.ApplyPatches();
        Log.LogInfo($"Applied all patches!");
    }

}
