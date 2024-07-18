using System.Collections;
using Unity.Mathematics;

namespace JackEhttack.patch;

using JackEhttack;

public static class ScrapModifierPatch
{
    
    public static void ApplyPatches()
    {
        On.RoundManager.SpawnScrapInLevel += ScrapPatch;
        On.StartOfRound.EndOfGame += ReplenishPatch;
        On.StartOfRound.Start += StartPatch;
        On.GameNetworkManager.ResetSavedGameValues += ResetMoonsPatch;
    }

    private static void ScrapPatch(On.RoundManager.orig_SpawnScrapInLevel orig, RoundManager self)
    {
        
        float oldAmountMultiplier = self.scrapAmountMultiplier;
        float oldValueMultiplier = self.scrapValueMultiplier;
        
        float modifier = 1 - service.MoonTracker.Instance.GetMoon(self.currentLevel)/Plugin.Config.restock.Value*Plugin.Config.denominator.Value;

        self.scrapAmountMultiplier *= math.min(2f, modifier);
        self.scrapValueMultiplier += math.max(1f, modifier - 2);
        
        Plugin.Instance.Log.LogInfo(
            "Scrap Amount Modifier: " + self.scrapAmountMultiplier + ", Scrap Value Modifier: " + self.scrapValueMultiplier);
        
        orig(self);

        self.scrapAmountMultiplier = oldAmountMultiplier;
        self.scrapValueMultiplier = oldValueMultiplier;
        
        service.MoonTracker.Instance.DiminishMoon(self.currentLevel);
        
    }
    
    private static IEnumerator ReplenishPatch(On.StartOfRound.orig_EndOfGame orig, StartOfRound self, int bodiesinsured, int connectedplayersonserver, int scrapcollected)
    {
        IEnumerator enumerator = orig(self, bodiesinsured, connectedplayersonserver, scrapcollected);
        
        service.MoonTracker.Instance.ReplenishMoons();
        
        return enumerator;
    }
    
    private static void StartPatch(On.StartOfRound.orig_Start orig, StartOfRound self)
    {
        orig(self);
        if (self.IsServer)
        {
            service.MoonTracker.Instance.LoadMoons();
        }
    }

    private static void ResetMoonsPatch(On.GameNetworkManager.orig_ResetSavedGameValues orig, GameNetworkManager self)
    {
        orig(self);
        service.MoonTracker.Instance.ResetMoons();
    }

}