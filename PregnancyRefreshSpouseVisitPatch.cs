// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregnancyRefreshSpouseVisitPatch
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace PregnancyControl
{
  [HarmonyPatch(typeof (PregnancyCampaignBehavior), "RefreshSpouseVisit", new Type[] {typeof (Hero)})]
  public class PregnancyRefreshSpouseVisitPatch
  {
    public static bool Prefix(PregnancyCampaignBehavior __instance, ref Hero hero)
    {
      return hero != Hero.MainHero && hero != Hero.MainHero.Spouse || !Config.Instance.DisablePlayerUncontrollablePregnancy;
    }
  }
}
