using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace JackEhttack.service;

public class MoonTracker
{
   public static MoonTracker Instance;

   private Dictionary<int, int> moonVisits;

   public void DiminishMoon(SelectableLevel moon)
   {
      if (moon.name == "CompanyBuildingLevel") return;

      moonVisits[moon.levelID] = 3;
   }

   public MoonTracker()
   {
      Instance = this;

      moonVisits = new Dictionary<int, int>();
   }

   public void ReplenishMoons()
   {
      foreach (int moon in moonVisits.Keys.ToList())
      {
         moonVisits[moon] = Mathf.Max(moonVisits[moon] - 1, 0);
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
         
         var moons = ES3.Load<int[]>("MoonTrackerMoons", currentSaveFileName, []).ToList();
         var values = ES3.Load<int[]>("MoonTrackerValues", currentSaveFileName, []).ToList();

         for (int i = 0; i < moons.Count; i++)
         {
            moonVisits[moons[i]] = values[i];
         }
      }
      catch (Exception arg)
      {
         Plugin.Instance.Log.LogError($"Error while trying to load MoonTracker values: {arg}");
      }
   }

   public int GetMoon(SelectableLevel moon)
   {
      if (!moonVisits.ContainsKey(moon.levelID)) return 0;

      return moonVisits[moon.levelID];
   }

   public void ResetMoons()
   {
      moonVisits = new Dictionary<int, int>();
      SaveMoons();
   }

}