using System.Collections.Generic;
using System.Linq;
using JackEhttack.netcode;
using JackEhttack.service;
using Unity.Netcode;
using UnityEngine;

namespace JackEhttack.patch;

public static class TerminalPatch
{

    private static List<(TerminalNode, int)> oldMoonPrices = [];
    private static bool debounce = true;
    
    public static void ApplyPatches()
    {
        On.Terminal.Awake += Terminal_Awake;
        On.Terminal.TextPostProcess += Terminal_PostProcess;
        On.StartOfRound.OnClientConnect += DiscountPatch;
    }

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

    private static void Terminal_Awake(On.Terminal.orig_Awake orig, Terminal self)
    {
        orig(self);

        if (debounce)
        {
            debounce = false; 
            
            addTrackerCommand(self);
           
            // Get list of moon travel nodes
            var nodes = Resources.FindObjectsOfTypeAll<TerminalNode>();
            Plugin.Instance.Log.LogDebug($"Scanned {nodes.Length} terminal nodes to potentially discount.");
            foreach (var termNode in nodes)
            {
                if (termNode.buyRerouteToMoon != -1 && termNode.itemCost > 0) 
                    oldMoonPrices.Add((termNode, termNode.itemCost)); 
            } 
           
        }

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            UpdatePrices(Plugin.Config.moonDiscount.Value);
        }

    }

    private static string Terminal_PostProcess(On.Terminal.orig_TextPostProcess orig, Terminal self, string modifieddisplaytext, TerminalNode node)
    {
        modifieddisplaytext = orig(self, modifieddisplaytext, node);
        modifieddisplaytext = modifieddisplaytext.Replace("[moonTracker]", MoonTracker.Instance.GetText());
        modifieddisplaytext = modifieddisplaytext.Replace("Other commands:", 
            "Other commands:\n\n>TRACKER\nScans nearby moons for scrap density.");
        return modifieddisplaytext;
    }
    
    private static void DiscountPatch(On.StartOfRound.orig_OnClientConnect orig, StartOfRound self, ulong clientid)
    {
        orig(self, clientid);
        NetworkHandler.Instance.DiscountUpdateClientRpc(Plugin.Config.moonDiscount.Value);
    }
    
}