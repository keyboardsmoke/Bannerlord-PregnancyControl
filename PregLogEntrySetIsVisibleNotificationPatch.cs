// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregLogEntrySetIsVisibleNotificationPatch
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.LogEntries;

namespace PregnancyControl
{
  [HarmonyPatch(typeof (PregnancyLogEntry), "get_IsVisibleNotification")]
  internal class PregLogEntrySetIsVisibleNotificationPatch
  {
    private PregLogEntrySetIsVisibleNotificationPatch()
    {
    }

    public static bool Prefix(PregnancyLogEntry __instance, ref bool __result)
    {
      PregnancyControlBehavior behaviorInstance = Utillty.GetPregnancyControlBehaviorInstance();
      bool flag = behaviorInstance != null && behaviorInstance.IsTimeToAddNewPregnant((Hero) __instance.Mother, Hero.MainHero);
      __result = (uint) ((Config.Instance.ShowNpcPregnancyLog ? 1 : (((object) ((Hero) __instance.Mother).Clan).Equals((object) Hero.MainHero.Clan) ? 1 : 0)) | (flag ? 1 : 0)) > 0U;
      return false;
    }
  }
}
