using System.Collections;
using JackEhttack.service;
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
        On.StartOfRound.OnClientConnect += UpdateTrackerPatch;
    }
    
    private static void ScrapPatch(On.RoundManager.orig_SpawnScrapInLevel orig, RoundManager self)
    {
        
        float oldAmountMultiplier = self.scrapAmountMultiplier;
        float oldValueMultiplier = self.scrapValueMultiplier;
        
        float modifier = 1 - MoonTracker.Instance.GetMoon(self.currentLevel)/Plugin.Config.restock.Value*Plugin.Config.denominator.Value;

        self.scrapAmountMultiplier *= math.min(2f, modifier);
        self.scrapValueMultiplier += math.max(0f, modifier - 3);
        
        Plugin.Instance.Log.LogDebug(
            "Scrap Amount Modifier: " + self.scrapAmountMultiplier + ", Scrap Value Modifier: " + self.scrapValueMultiplier);
        
        Plugin.Instance.Log.LogDebug(
            "Old Scrap Amount Modifier: " + oldAmountMultiplier + ", Scrap Value Modifier: " + oldValueMultiplier);
        
        orig(self);

        self.scrapAmountMultiplier = oldAmountMultiplier;
        self.scrapValueMultiplier = oldValueMultiplier;
        
        
    }
    
    private static IEnumerator ReplenishPatch(On.StartOfRound.orig_EndOfGame orig, StartOfRound self, int bodiesinsured, int connectedplayersonserver, int scrapcollected)
    {
        IEnumerator enumerator = orig(self, bodiesinsured, connectedplayersonserver, scrapcollected);
        
        MoonTracker.Instance.ReplenishMoons();
        MoonTracker.Instance.DiminishMoon(self.currentLevel);
        
        return enumerator;
    }
    
    private static void StartPatch(On.StartOfRound.orig_Start orig, StartOfRound self)
    {
        orig(self);
        if (self.IsServer)
        {
            MoonTracker.Instance.LoadMoons();
        }
    }

    private static void ResetMoonsPatch(On.GameNetworkManager.orig_ResetSavedGameValues orig, GameNetworkManager self)
    {
        orig(self);
        MoonTracker.Instance.ResetMoons();
    }

    private static void UpdateTrackerPatch(On.StartOfRound.orig_OnClientConnect orig, StartOfRound self, ulong clientid)
    {
        orig(self, clientid);
        MoonTracker.Instance.UpdateClientTrackers();
    }
    
}