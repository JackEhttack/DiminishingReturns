using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JackEhttack.netcode;
using JackEhttack.service;
using Unity.Netcode;
using UnityEngine;

namespace JackEhttack.patch;

static class TerminalPatches
{

    private static List<(TerminalNode, int)> oldMoonPrices = [];
    private static bool debounce = true;

    private static void addTrackerCommand(Terminal self)
    {
        TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
        node.name = "ShowMoonsDiminish";
        node.displayText =
            "ScrapTracker 9000 | V0.84 | FitLady CRACK (REPACK)\n____________________________\nAll rights reserved, do not redistribute.\n\nResults from local celestial bodies:\n[moonTracker]\n";
        node.clearPreviousText = true;

        TerminalKeyword keyword = ScriptableObject.CreateInstance<TerminalKeyword>();
        keyword.name = "scrapAmountMoons";
        keyword.word = "tracker";
        keyword.isVerb = false;
        keyword.compatibleNouns = null;
        keyword.specialKeywordResult = node;
        keyword.defaultVerb = null;
        keyword.accessTerminalObjects = false;

        List<TerminalKeyword> keywords = self.terminalNodes.allKeywords.ToList();
        keywords.Add(keyword);
        self.terminalNodes.allKeywords = keywords.ToArray(); 
    }

    public static void UpdatePrices(float discount)
    {
        foreach (var pair in oldMoonPrices)
        {
            pair.Item1.itemCost = (int) (pair.Item2 * discount);
        }
    }

    [HarmonyPatch(typeof(Terminal), nameof(Terminal.Awake))]
    [HarmonyPostfix]
    private static void Terminal_Awake(Terminal __instance)
    {
        if (debounce)
        {
            debounce = false; 
            
            addTrackerCommand(__instance);
           
            // Get list of moon travel nodes
            var nodes = Resources.FindObjectsOfTypeAll<TerminalNode>();
            Plugin.Instance.Log.LogInfo($"Scanned {nodes.Length} terminal nodes to potentially discount.");
            foreach (var termNode in nodes)
            {
                if (termNode.buyRerouteToMoon != -1 && termNode.itemCost > 0) 
                    oldMoonPrices.Add((termNode, termNode.itemCost)); 
            } 
           
        }

        Plugin.Config.InitialSyncCompleted += (sender, args) => {
            UpdatePrices(Plugin.Config.moonDiscount.Value);
        };

    }

    [HarmonyPatch(typeof(Terminal), nameof(Terminal.TextPostProcess))]
    [HarmonyPostfix]
    private static void Terminal_PostProcess(ref string __result)
    {
        __result = __result.Replace("[moonTracker]", MoonTracker.Instance.GetText());
        __result = __result.Replace("Other commands:", 
            "Other commands:\n\n>TRACKER\nScans nearby moons for scrap density.");
    }
    
}