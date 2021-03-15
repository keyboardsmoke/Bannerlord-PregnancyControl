// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregnancyChildConceivedPatch
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace PregnancyControl
{
  [HarmonyPatch(typeof (PregnancyCampaignBehavior), "ChildConceived")]
  public class PregnancyChildConceivedPatch
  {
    private static Hero m_mother;
    private static Hero m_father;

    private PregnancyChildConceivedPatch()
    {
    }

    public static bool Prefix(PregnancyCampaignBehavior __instance, ref Hero mother)
    {
      PregnancyChildConceivedPatch._getRealParentsInCurPregnancy(mother);
      return true;
    }

    public static void Postfix()
    {
      PregnancyChildConceivedPatch._changeFatherInCurPregnancy();
      PregnancyChildConceivedPatch._checkNotificationOnChildConceived();
      PregnancyChildConceivedPatch._cleanSavedParents();
    }

    private static void _getRealParentsInCurPregnancy(Hero mother)
    {
      if (mother == null)
        return;
      PregnancyControlBehavior behaviorInstance = Utillty.GetPregnancyControlBehaviorInstance();
      if (behaviorInstance == null)
        return;
      Hero fatherInCurPregnancy = behaviorInstance.GetRealFatherInCurPregnancy(mother);
      PregnancyChildConceivedPatch.m_father = fatherInCurPregnancy != mother.Spouse ? fatherInCurPregnancy : (Hero) null;
      PregnancyChildConceivedPatch.m_mother = PregnancyChildConceivedPatch.m_father != null ? mother : (Hero) null;
      Utillty.DebugDisplayMessage("_getRealParentsInCurPregnancy End!");
    }

    private static void _changeFatherInCurPregnancy()
    {
      if (PregnancyChildConceivedPatch.m_mother == null || PregnancyChildConceivedPatch.m_father == null)
        return;
      IEnumerable pregnanciesObject = (IEnumerable) PregCampHelper.GetHeroPregnanciesObject();
      if (pregnanciesObject == null)
        return;
      foreach (object pregnancyObj in pregnanciesObject)
      {
        if (PregCampHelper.GetPregnancyMother(pregnancyObj) == PregnancyChildConceivedPatch.m_mother)
        {
          PregCampHelper.SetPregnancyFather(pregnancyObj, PregnancyChildConceivedPatch.m_father);
          Utillty.DebugDisplayMessage("_changeFatherInCurPregnancy Make Pregnant! Mother : " + ((object) PregnancyChildConceivedPatch.m_mother.Name).ToString() + " Father : " + ((object) PregnancyChildConceivedPatch.m_father.Name).ToString());
          break;
        }
      }
    }

    private static void _checkNotificationOnChildConceived()
    {
      if (PregnancyChildConceivedPatch.m_mother == null || PregnancyChildConceivedPatch.m_mother == Hero.MainHero || (PregnancyChildConceivedPatch.m_mother == Hero.MainHero.Spouse || PregnancyChildConceivedPatch.m_mother.Clan == Clan.PlayerClan) || PregnancyChildConceivedPatch.m_father != Hero.MainHero)
        return;
      TextObject textObject = new TextObject(Utillty.ET("{=pcm_child_conceived_ntf_father}{MOTHER} has just learned that she is with your child."), null);
      textObject.SetTextVariable("MOTHER", (TextObject) PregnancyChildConceivedPatch.m_mother.Name);
      InformationManager.AddQuickInformation(textObject, 0, (BasicCharacterObject) null, "");
    }

    private static void _cleanSavedParents()
    {
      PregnancyChildConceivedPatch.m_mother = (Hero) null;
      PregnancyChildConceivedPatch.m_father = (Hero) null;
    }
  }
}
