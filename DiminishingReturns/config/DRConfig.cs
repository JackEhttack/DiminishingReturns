using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using HarmonyLib;

namespace JackEhttack;

public class DRConfig : SyncedConfig2<DRConfig>
{
    [field: SyncedEntryField] public SyncedEntry<int> restock { get; private set; }
    [field: SyncedEntryField] public SyncedEntry<float> denominator { get; private set; }
    [field: SyncedEntryField] public SyncedEntry<float> amount { get; private set; }
    [field: SyncedEntryField] public SyncedEntry<float> maxBonus { get; private set; }
    [field: SyncedEntryField] public SyncedEntry<float> bonusChance { get; private set; }
    [field: SyncedEntryField] public SyncedEntry<float> moonDiscount { get; private set; }

    public DRConfig(ConfigFile cfg) : base("DiminishingReturns")
    {
        // We want to disable saving our config file every time we bind a
        // setting as it's inefficient and slow
        cfg.SaveOnConfigSet = false; 
        
        restock = cfg.BindSyncedEntry(
            "General",
            "DaysTillRestock",
            4,
            "How many days until a moon with diminishing returns regenerate their scrap to full.");

        denominator = cfg.BindSyncedEntry(
            "General",
            "MaxDiminish",
            0.5f,
            "The maximum amount a moon's scrap can be diminished.");

        amount = cfg.BindSyncedEntry(
            "General",
            "DiminishAmount",
            1f,
            "How much a moon is diminished per visit. Note: diminishment will not exceed MaxDiminish. [0 - 1.0]");
        
        maxBonus = cfg.BindSyncedEntry(
            "General",
            "MaxBonus",
            3.2f,
            "The upper limit for a bonus moon's scrap.");

        bonusChance = cfg.BindSyncedEntry(
            "General",
            "BonusChance",
            0.5f,
            "The chance of a bonus moon appearing per day.");

        moonDiscount = cfg.BindSyncedEntry(
            "General",
            "MoonDiscount",
            0.5f,
            "A discount applied to the price of moons, to make up for diminishment. [0 - 1.0]");

        ClearOrphanedEntries(cfg);
        cfg.Save();
        cfg.SaveOnConfigSet = true;

        ConfigManager.Register(this);
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