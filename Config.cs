// Decompiled with JetBrains decompiler
// Type: PregnancyControl.Config
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using System;
using System.IO;
using System.Xml.Serialization;
using TaleWorlds.Library;

namespace PregnancyControl
{
  internal class Config
  {
    private static Config m_instance;
    private ConfigData m_data;

    public static Config Instance
    {
      get
      {
        if (Config.m_instance == null)
          Config.m_instance = new Config();
        return Config.m_instance;
      }
    }

    public bool DisablePlayerUncontrollablePregnancy
    {
      get
      {
        return this.m_data.m_bDisablePlayerUncontrollablePregnancy;
      }
    }

    public bool EnableNotSpouseGaySex
    {
      get
      {
        return this.m_data.m_bEnableNotSpouseGaySex;
      }
    }

    public bool EnableLesbianPregnancy
    {
      get
      {
        return this.m_data.m_bEnableLesbianPregnancy;
      }
    }

    public bool ShowNpcPregnancyLog
    {
      get
      {
        return this.m_data.m_bShowNpcPregnancyLog;
      }
    }

    public int DailySexLimitForEachHero
    {
      get
      {
        return this.m_data.m_nDailySexLimitForEachHero;
      }
    }

    public float MinAge
    {
      get
      {
        return 18f;
      }
    }

    public float NormalSexRealtionLimit
    {
      get
      {
        return this.m_data.m_fNormalSexRelationLimit;
      }
    }

    public float AdultrySexRealtionLimit
    {
      get
      {
        return this.m_data.m_fAdultrySexRelationLimit;
      }
    }

    public float SpouseSexRealtionLimit
    {
      get
      {
        return this.m_data.m_fSpouseSexRelationLimit;
      }
    }

    public int MinSexDelayDays
    {
      get
      {
        return this.m_data.m_nMinSexDelayDays;
      }
    }

    public int MaxSexDelayDays
    {
      get
      {
        return this.m_data.m_nMaxSexDelayDays;
      }
    }

    public float PregnancyProbability
    {
      get
      {
        return this.m_data.m_fPregnancyProbability;
      }
    }

    public int MinPregnancyDelayDays
    {
      get
      {
        return this.m_data.m_nMinPregnancyDelayDays;
      }
    }

    public int MaxPregnancyDelayDays
    {
      get
      {
        return this.m_data.m_nMaxPregnancyDelayDays;
      }
    }

    public static void Init()
    {
      if (Config.m_instance != null)
        return;
      Config.m_instance = new Config();
    }

    private Config()
    {
      this._loadConfig();
    }

    private void _loadConfig()
    {
      string path = Path.Combine(BasePath.Name, "Modules", "PregnancyControl", "ModuleData", "config.xml");
      try
      {
        using (StreamReader streamReader = new StreamReader(path))
          this.m_data = (ConfigData) new XmlSerializer(typeof (ConfigData)).Deserialize((TextReader) streamReader);
      }
      catch (Exception ex)
      {
        this.m_data = new ConfigData();
        Debug.PrintError("Config:_loadConfig Load Config Error : " + ex.Message, (string) null, 17592186044416UL);
      }
    }
  }
}
