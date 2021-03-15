// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregnancyControlSubModule
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace PregnancyControl
{
  public class PregnancyControlSubModule : MBSubModuleBase
  {
    public static CampaignGameStarter m_campaignGamestarterInst;

    protected override void OnSubModuleLoad()
    {
      base.OnSubModuleLoad();
      Config.Init();
      this._harmonyPatchAll();
    }

    private void _harmonyPatchAll()
    {
      try
      {
        new Harmony(typeof (PregnancyControlSubModule).FullName).PatchAll();
      }
      catch (Exception ex)
      {
        Debug.PrintError("PregnancyControlSubModule:_harmonyPatchAll Patch Error : " + ex.Message, (string) null, 17592186044416UL);
      }
    }

    protected override void OnGameStart(Game game, IGameStarter gameStarter)
    {
      base.OnGameStart(game, gameStarter);
      if (!(game.GameType is Campaign))
        return;
      PregnancyControlSubModule.m_campaignGamestarterInst = (CampaignGameStarter) gameStarter;
      this._addBehaviors(PregnancyControlSubModule.m_campaignGamestarterInst);
    }

    private void _addBehaviors(CampaignGameStarter starter)
    {
      starter.AddBehavior((CampaignBehaviorBase) new PregnancyControlBehavior());
      starter.AddBehavior((CampaignBehaviorBase) new PregnancyControlUIBehavior());
    }

    public PregnancyControlSubModule() : base()
    {
    }
  }
}
