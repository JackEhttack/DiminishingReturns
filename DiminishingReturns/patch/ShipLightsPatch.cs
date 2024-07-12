using Dissonance;

namespace JackEhttack.patch;

public static class ShipLightsPatch
{
    public static void ApplyPatches()
    {
        On.ShipLights.ToggleShipLights += ShipLightsPrint;
    }

    private static void ShipLightsPrint(On.ShipLights.orig_ToggleShipLights orig, ShipLights self)
    {
        orig(self);
        HUDManager.Instance.AddTextToChatOnServer("Lights: " + self.areLightsOn);
    }
}