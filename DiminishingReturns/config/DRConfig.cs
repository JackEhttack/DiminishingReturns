using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;

namespace JackEhttack;

class DRConfig
{
    public readonly ConfigEntry<int> restock;
    public readonly ConfigEntry<float> denominator;
    public readonly ConfigEntry<float> maxBonus;
    public readonly ConfigEntry<float> bonusChance;
    public readonly ConfigEntry<float> moonDiscount;

    public DRConfig(ConfigFile cfg)
    {
        // We want to disable saving our config file every time we bind a
        // setting as it's inefficient and slow
        cfg.SaveOnConfigSet = false; 
        
        restock = cfg.Bind(
            "General",
            "DaysTillRestock",
            4,
            "How many days until a moon with diminishing returns regenerate their scrap to full.");

        denominator = cfg.Bind(
            "General",
            "MaxDiminish",
            0.5f,
            "The maximum amount a moon's scrap can be diminished.");
        
        maxBonus = cfg.Bind(
            "General",
            "MaxBonus",
            3.2f,
            "The upper limit for a bonus moon's scrap.");

        bonusChance = cfg.Bind(
            "General",
            "BonusChance",
            0.5f,
            "The chance of a bonus moon appearing per day.");

        moonDiscount = cfg.Bind(
            "General",
            "MoonDiscount",
            0.5f,
            "A discount applied to the price of moons, to make up for diminishment.");

        ClearOrphanedEntries(cfg);
        cfg.Save();
        cfg.SaveOnConfigSet = true;
    }
    
    static void ClearOrphanedEntries(ConfigFile cfg) 
    { 
        // Find the private property `OrphanedEntries` from the type `ConfigFile`
        PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries"); 
        // And get the value of that property from our ConfigFile instance
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg); 
        // And finally, clear the `OrphanedEntries` dictionary
        orphanedEntries.Clear(); 
    } 

}