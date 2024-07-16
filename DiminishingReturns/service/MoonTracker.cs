using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace JackEhttack.service;

public class MoonTracker
{
   public static MoonTracker Instance;

   private Dictionary<SelectableLevel, int> moonVisits;

   public void DiminishMoon(SelectableLevel moon)
   {
      if (moon.name == "CompanyBuildingLevel") return;

      moonVisits[moon] = 3;

   }

   public MoonTracker()
   {
      Instance = this;

      moonVisits = new Dictionary<SelectableLevel, int>();
   }

   public void ReplenishMoons()
   {
      foreach (SelectableLevel moon in moonVisits.Keys.ToList())
      {
         moonVisits[moon] = Mathf.Max(moonVisits[moon] - 1, 0);
      }

      try
      {
         throw new NotImplementedException("Saving has not been implemented.");
      }
      catch (Exception arg)
      {
         Plugin.Instance.Log.LogError($"Error while trying to save MoonTracker values: {arg}");
      }
   }

   public int GetMoon(SelectableLevel moon)
   {
      if (!moonVisits.ContainsKey(moon)) return 0;

      return moonVisits[moon];
   }

}