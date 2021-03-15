// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregnancyCheckOffspringsToDeliverPatch
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace PregnancyControl
{
  [HarmonyPatch(typeof (PregnancyCampaignBehavior), "CheckOffspringsToDeliver", new Type[] {typeof (Hero)})]
  public class PregnancyCheckOffspringsToDeliverPatch
  {
    private PregnancyCheckOffspringsToDeliverPatch()
    {
    }

    public static bool Prefix(PregnancyCampaignBehavior __instance, ref Hero hero)
    {
      PregnancyCheckOffspringsToDeliverPatch._removeAbortionMotherInHeroPregnancies(hero);
      return true;
    }

    private static void _removeAbortionMotherInHeroPregnancies(Hero hero)
    {
      if (hero == null)
        return;
      PregnancyControlBehavior pregnancyControlInst = Utillty.GetPregnancyControlBehaviorInstance();
      if (pregnancyControlInst == null || !pregnancyControlInst.PreAbortionMap.ContainsKey(hero))
        return;
      object pregnanciesObject = PregCampHelper.GetHeroPregnanciesObject();
      if (pregnanciesObject == null)
        return;
      MethodInfo pregnanciesMethodInfo = PregCampHelper.GetHeroPregnanciesMethodInfo("RemoveAll");
      if (pregnanciesMethodInfo == (MethodInfo) null)
        return;
      Predicate<object> predicate = (Predicate<object>) (pregnancyObj => PregnancyCheckOffspringsToDeliverPatch._removeAbortionMotherPredicate(ref pregnancyControlInst, hero, pregnancyObj));
      Utillty.DebugDisplayMessage("_removeAbortionMotherInHeroPregnancies Success! RemoveNums : " + ((int) pregnanciesMethodInfo.Invoke(pregnanciesObject, new object[1]
      {
        (object) predicate
      })).ToString());
      pregnancyControlInst.CleanPreAbortionList();
    }

    private static bool _removeAbortionMotherPredicate(
      ref PregnancyControlBehavior pregnancyControlInst,
      Hero hero,
      object heroPregnancyObj)
    {
      if (heroPregnancyObj == null)
        return false;
      Hero pregnancyMother = PregCampHelper.GetPregnancyMother(heroPregnancyObj);
      if (pregnancyMother != hero)
        return false;
      pregnancyControlInst.DoAbortionNotificationAndLog(pregnancyMother, pregnancyControlInst.PreAbortionMap[pregnancyMother]);
      return true;
    }
  }
}
