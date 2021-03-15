// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregCampHelper
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using System;
using System.Collections;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace PregnancyControl
{
  public static class PregCampHelper
  {
    public static Type m_pregnancyType = AccessTools.Inner(typeof (PregnancyCampaignBehavior), "Pregnancy");
    public static FieldInfo m_pregnancyMotherField = (FieldInfo) null;
    public static FieldInfo m_pregnancyFatherField = (FieldInfo) null;
    public static FieldInfo m_pregnancyDueDateField = (FieldInfo) null;
    public static FieldInfo m_heroPregnanciesField = AccessTools.Field(typeof (PregnancyCampaignBehavior), "_heroPregnancies");

    static PregCampHelper()
    {
      PregCampHelper.m_pregnancyMotherField = AccessTools.Field(PregCampHelper.m_pregnancyType, "Mother");
      PregCampHelper.m_pregnancyFatherField = AccessTools.Field(PregCampHelper.m_pregnancyType, "Father");
      PregCampHelper.m_pregnancyDueDateField = AccessTools.Field(PregCampHelper.m_pregnancyType, "DueDate");
    }

    public static Hero GetPregnancyMother(object pregnancyObj)
    {
      return PregCampHelper.m_pregnancyMotherField == (FieldInfo) null ? (Hero) null : (Hero) PregCampHelper.m_pregnancyMotherField.GetValue(pregnancyObj);
    }

    public static void SetPregnancyFather(object pregnancyObj, Hero newFather)
    {
      if (PregCampHelper.m_pregnancyFatherField == (FieldInfo) null)
        return;
      PregCampHelper.m_pregnancyFatherField.SetValue(pregnancyObj, (object) newFather);
    }

    public static Hero GetPregnancyFather(object pregnancyObj)
    {
      return PregCampHelper.m_pregnancyFatherField == (FieldInfo) null ? (Hero) null : (Hero) PregCampHelper.m_pregnancyFatherField.GetValue(pregnancyObj);
    }

    public static void SetPregnancyDueDate(object pregnancyObj, CampaignTime newDueDate)
    {
      if (PregCampHelper.m_pregnancyDueDateField == (FieldInfo) null)
        return;
      PregCampHelper.m_pregnancyDueDateField.SetValue(pregnancyObj, (object) newDueDate);
    }

    public static CampaignTime GetPregnancyDueDate(object pregnancyObj)
    {
      return PregCampHelper.m_pregnancyDueDateField == (FieldInfo) null ? CampaignTime.Never : (CampaignTime) PregCampHelper.m_pregnancyDueDateField.GetValue(pregnancyObj);
    }

    public static object GetHeroPregnanciesObject()
    {
      if (PregCampHelper.m_heroPregnanciesField == (FieldInfo) null)
        return (object) null;
      PregnancyCampaignBehavior behaviorInstance = Utillty.GetPregnancyCampaignBehaviorInstance();
      return behaviorInstance == null ? (object) null : PregCampHelper.m_heroPregnanciesField.GetValue((object) behaviorInstance);
    }

    public static MethodInfo GetHeroPregnanciesMethodInfo(string method)
    {
      return PregCampHelper.m_heroPregnanciesField == (FieldInfo) null || string.IsNullOrEmpty(method) ? (MethodInfo) null : PregCampHelper.m_heroPregnanciesField.FieldType.GetMethod(method);
    }

    public static void DoChildConceivedMethod(Hero hero)
    {
      if (hero == null)
        return;
      PregnancyCampaignBehavior behaviorInstance = Utillty.GetPregnancyCampaignBehaviorInstance();
      if (behaviorInstance == null)
        return;
      MethodInfo method = typeof (PregnancyCampaignBehavior).GetMethod("ChildConceived", BindingFlags.Instance | BindingFlags.NonPublic);
      if (method == (MethodInfo) null)
        return;
      method.Invoke((object) behaviorInstance, new object[1]
      {
        (object) hero
      });
    }

    public static bool IsInPregnancies(Hero mother, Hero father)
    {
      if (mother == null && father == null)
        return false;
      IEnumerable pregnanciesObject = (IEnumerable) PregCampHelper.GetHeroPregnanciesObject();
      if (pregnanciesObject == null)
        return false;
      bool flag = false;
      foreach (object pregnancyObj in pregnanciesObject)
      {
        if ((uint) ((mother == null ? 1 : (PregCampHelper.GetPregnancyMother(pregnancyObj) == mother ? 1 : 0)) & (father == null ? 1 : (PregCampHelper.GetPregnancyFather(pregnancyObj) == father ? 1 : 0))) > 0U)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }
  }
}
