﻿using System.Collections;

namespace JackEhttack.patch;

using JackEhttack;
using JackEhttack.service;


public static class ScrapModifierPatch
{

    public static void ApplyPatches()
    {
        On.RoundManager.SpawnScrapInLevel += ScrapPatch;
        On.StartOfRound.EndOfGame += DiminishPatch;
    }

    private static void ScrapPatch(On.RoundManager.orig_SpawnScrapInLevel orig, RoundManager self)
    {
        // self.scrapAmountMultiplier
        // self.scrapValueMultiplier
        
        float oldAmountMultiplier = self.scrapAmountMultiplier;
        float oldValueMultiplier = self.scrapValueMultiplier;

        self.scrapAmountMultiplier *= (float) (3 - MoonTracker.Instance.GetMoon(self.currentLevel))/3;
        self.scrapValueMultiplier *= (float) (8 - MoonTracker.Instance.GetMoon(self.currentLevel))/8;
        
        Plugin.Instance.Log.LogInfo(
            "Scrap Amount Modifier: " + self.scrapAmountMultiplier + ", Scrap Value Modifier: " + self.scrapValueMultiplier);
        
        orig(self);

        self.scrapAmountMultiplier = oldAmountMultiplier;
        self.scrapValueMultiplier = oldValueMultiplier;
        
        MoonTracker.Instance.DiminishMoon(self.currentLevel);
        
    }
    
    private static IEnumerator DiminishPatch(On.StartOfRound.orig_EndOfGame orig, StartOfRound self, int bodiesinsured, int connectedplayersonserver, int scrapcollected)
    {
        IEnumerator enumerator = orig(self, bodiesinsured, connectedplayersonserver, scrapcollected);
        
        MoonTracker.Instance.ReplenishMoons();
        
        return enumerator;
    }
    
}