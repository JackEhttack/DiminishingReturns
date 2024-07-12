using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Collections;

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
   }

   public void ReplenishMoons()
   {
      foreach (SelectableLevel moon in moonVisits.Keys.ToList())
      {
         moonVisits[moon] = Mathf.Max(moonVisits[moon] - 1, 0);
      }
   }

   public int GetMoon(SelectableLevel moon)
   {
      if (!moonVisits.ContainsKey(moon)) return 0;

      return moonVisits[moon];
   }

}