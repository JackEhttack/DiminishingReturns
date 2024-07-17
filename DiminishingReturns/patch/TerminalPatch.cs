﻿using System.Collections.Generic;
using System.Linq;
using JackEhttack.service;
using UnityEngine;

namespace JackEhttack.patch;

public static class TerminalPatch
{

    public static void ApplyPatches()
    {
        On.Terminal.Awake += Terminal_Awake;
        On.Terminal.TextPostProcess += Terminal_PostProcess;
    }

    private static void Terminal_Awake(On.Terminal.orig_Awake orig, Terminal self)
    {
        orig(self);

        TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
        node.name = "ShowMoonsDiminish";
        node.displayText = "ScrapTracker 9000 | V0.84 | FitLady CRACK (REPACK)\n____________________________\nAll rights reserved, do not redistribute.\n\nResults from local celestial bodies:\n[moonTracker]\n";
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

    private static string Terminal_PostProcess(On.Terminal.orig_TextPostProcess orig, Terminal self, string modifieddisplaytext, TerminalNode node)
    {
        modifieddisplaytext = orig(self, modifieddisplaytext, node);
        modifieddisplaytext = modifieddisplaytext.Replace("[moonTracker]", MoonTracker.Instance.GetText());
        return modifieddisplaytext;
    }

}