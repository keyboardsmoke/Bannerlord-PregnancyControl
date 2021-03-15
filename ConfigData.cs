// Decompiled with JetBrains decompiler
// Type: PregnancyControl.ConfigData
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using System;

namespace PregnancyControl
{
  [Serializable]
  public class ConfigData
  {
    public bool m_bDisablePlayerUncontrollablePregnancy = true;
    public int m_nDailySexLimitForEachHero = 999;
    public float m_fSpouseSexRelationLimit = -100f;
    public int m_nMinSexDelayDays = 1;
    public int m_nMaxSexDelayDays = 7;
    public float m_fPregnancyProbability = 0.7f;
    public int m_nMinPregnancyDelayDays = 1;
    public int m_nMaxPregnancyDelayDays = 5;
    public bool m_bEnableNotSpouseGaySex;
    public bool m_bEnableLesbianPregnancy;
    public bool m_bShowNpcPregnancyLog;
    public float m_fNormalSexRelationLimit;
    public float m_fAdultrySexRelationLimit;
  }
}
