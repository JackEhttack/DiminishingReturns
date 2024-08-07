﻿using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace JackEhttack.service;

public class MoonTracker
{
   public static MoonTracker Instance;

   private string bonusMoon;
   private float bonusAmount;
   private Dictionary<string, int> moonVisits;

   public void DiminishMoon(SelectableLevel moon)
   {
      if (moon.name == "CompanyBuildingLevel") return;

      moonVisits[moon.PlanetName] = Plugin.Config.restock.Value;
   }

   public MoonTracker()
   {
      Instance = this;

      moonVisits = new Dictionary<string, int>();
      bonusMoon = "";
      bonusAmount = 1.0f;
   }

   public void ReplenishMoons()
   {
      foreach (string moon in moonVisits.Keys.ToList())
      {
         moonVisits[moon] = Mathf.Max(moonVisits[moon] - 1, 0);
         if (moonVisits[moon] < 1) moonVisits.Remove(moon);
      }

      Plugin.Instance.Log.LogDebug("Successfully Replenished Moons!");
      
      System.Random random = new System.Random(StartOfRound.Instance.randomMapSeed + 216);
      
      List<SelectableLevel> levels = StartOfRound.Instance.levels.ToList();
      SelectableLevel level = levels[random.Next(0, levels.Count)];
         
      if (random.NextDouble() < Plugin.Config.bonusChance.Value && level.levelID != 3 && level.levelID != -1)
      {
         bonusMoon = level.PlanetName;
         bonusAmount = 1 + (float) random.NextDouble() * (Plugin.Config.maxBonus.Value - 1);
         moonVisits[level.PlanetName] = 0;
      }
      else
      {
         bonusMoon = "";
         bonusAmount = 1.0f;
      }

      SaveMoons();
      
   }

   private void SaveMoons()
   {
      try
      {
         var currentSaveFileName = GameNetworkManager.Instance.currentSaveFileName;

         ES3.Save("MoonTrackerMoons", moonVisits.Keys.ToArray(), currentSaveFileName);
         ES3.Save("MoonTrackerValues", moonVisits.Values.ToArray(), currentSaveFileName);
         ES3.Save("BonusMoon", bonusMoon, currentSaveFileName);
         ES3.Save("BonusAmount", bonusAmount, currentSaveFileName);
         
         Plugin.Instance.Log.LogDebug("Successfully saved MoonTracker data.");
      }
      catch (Exception arg)
      {
         Plugin.Instance.Log.LogError($"Error while trying to save MoonTracker values: {arg}");
      }
   }

   public void LoadMoons()
   {
      try
      {
         var currentSaveFileName = GameNetworkManager.Instance.currentSaveFileName;
         
         var moons = ES3.Load<string[]>("MoonTrackerMoons", currentSaveFileName, []).ToList();
         var values = ES3.Load<int[]>("MoonTrackerValues", currentSaveFileName, []).ToList();
         bonusMoon = ES3.Load("BonusMoon", currentSaveFileName, "");
         bonusAmount = ES3.Load("BonusAmount", currentSaveFileName, 1.0f);

         for (int i = 0; i < moons.Count; i++)
         {
            moonVisits[moons[i]] = values[i];
         }
         Plugin.Instance.Log.LogDebug("Successfully loaded MoonTracker data.");
      }
      catch (Exception arg)
      {
         Plugin.Instance.Log.LogError($"Error while trying to load MoonTracker values: {arg}");
      }
   }

   public float GetMoon(SelectableLevel moon)
   {
      if (moon.PlanetName == bonusMoon) return -(Plugin.Config.restock.Value / Plugin.Config.denominator.Value) * (bonusAmount - 1);
      if (!moonVisits.ContainsKey(moon.PlanetName)) return 0;

      return moonVisits[moon.PlanetName];
   }

   public void ResetMoons()
   {
      moonVisits = new Dictionary<string, int>();
      SaveMoons();
   }

   public string GetText()
   {
      string text = "";

      if (bonusMoon.Length > 0)
      {
         text += $"\nBonus Scrap Moons:\n      {bonusMoon}: {bonusAmount:P1}\n";
      }

      if (moonVisits.Count > 0) text += "\nReduced Scrap Moons:\n";
      
      foreach (var pair in moonVisits)
      {
         text += $"      {pair.Key}: {1-(float)pair.Value/Plugin.Config.restock.Value*Plugin.Config.denominator.Value:P1}\n";
      }

      return text.Length == 0 ? "\nNo scrap anomalies detected!\n" : text;
   }

}