using HarmonyLib;
using JackEhttack.service;
using Unity.Mathematics;

namespace JackEhttack.patch;

using JackEhttack;

static class ScrapModifierPatches
{
    private static float oldAmountMultiplier = 1;
    private static float oldValueMultiplier = 1;
    
    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnScrapInLevel))]
    [HarmonyPriority(-100)] // run as late as possible to avoid mod conflicts
    [HarmonyPrefix]
    private static void ScrapPatch(RoundManager __instance)
    {
        
        oldAmountMultiplier = __instance.scrapAmountMultiplier;
        oldValueMultiplier = __instance.scrapValueMultiplier;
        
        float modifier = 1 - MoonTracker.Instance.GetMoon(__instance.currentLevel)/Plugin.Config.restock.Value*Plugin.Config.denominator.Value;

        __instance.scrapAmountMultiplier *= math.min(2f, modifier);
        __instance.scrapValueMultiplier += math.max(0f, modifier - 3);
        
        Plugin.Instance.Log.LogInfo(
            "Scrap Amount Modifier: " + __instance.scrapAmountMultiplier + ", Scrap Value Modifier: " + __instance.scrapValueMultiplier);
        
        Plugin.Instance.Log.LogInfo(
            "Old Scrap Amount Modifier: " + oldAmountMultiplier + ", Scrap Value Modifier: " + oldValueMultiplier);
    }

    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnScrapInLevel))]
    [HarmonyPostfix]
    private static void ScrapFix(RoundManager __instance)
    {
        __instance.scrapAmountMultiplier = oldAmountMultiplier;
        __instance.scrapValueMultiplier = oldValueMultiplier;
    }

    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
    [HarmonyPostfix]
    private static void ReplenishPatch(StartOfRound __instance)
    {
        MoonTracker.Instance.ReplenishMoons();
        MoonTracker.Instance.DiminishMoon(__instance.currentLevel);
        MoonTracker.Instance.SaveMoons();
    }
   
    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Start))]
    [HarmonyPostfix]
    private static void StartPatch(StartOfRound __instance)
    {
        if (__instance.IsServer || __instance.IsHost)
        {
            MoonTracker.Instance.LoadMoons();
        }
    }

    [HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.ResetSavedGameValues))]
    [HarmonyPostfix]
    private static void ResetMoonsPatch(GameNetworkManager __instance)
    {
        MoonTracker.Instance.ResetMoons();
    }

    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.OnClientConnect))]
    [HarmonyPostfix]
    private static void UpdateTrackerPatch(StartOfRound __instance)
    {
        MoonTracker.Instance.UpdateClientTrackers();
    }
    
}