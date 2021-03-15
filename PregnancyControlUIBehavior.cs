// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregnancyControlUIBehavior
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;

namespace PregnancyControl
{
  internal class PregnancyControlUIBehavior : CampaignBehaviorBase
  {
    private string m_sPregCtrlClanXmlName;
    private PregnancyControlClanVM m_pregContClanVM;
    // private GauntletLayer m_pregContClanLayer;

    public override void RegisterEvents()
    {
      //Game.Current.EventManager.RegisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this._addGauntletLayer));
    }

    public override void SyncData(IDataStore dataStore)
    {
    }

    private void _addGauntletLayer(TutorialContextChangedEvent tccEvent)
    {
      this._addPregCtrlClanLayer(tccEvent);
    }

    private void _addPregCtrlClanLayer(TutorialContextChangedEvent tccEvent)
    {
      if (tccEvent.NewContext != TutorialContexts.ClanScreen)
        return;
      GauntletLayer screenGauntletLayer = UIHelper.GetClanScreenGauntletLayer();
      if (screenGauntletLayer == null)
        return;
      this.m_pregContClanVM = new PregnancyControlClanVM();
      if (((List<Tuple<GauntletMovie, ViewModel>>) screenGauntletLayer._moviesAndDatasources).FindIndex((Predicate<Tuple<GauntletMovie, ViewModel>>) (obj => string.Equals(obj.Item1.MovieName, this.m_sPregCtrlClanXmlName))) >= 0)
        return;
      screenGauntletLayer.LoadMovie("PregnancyControlClan", (ViewModel) this.m_pregContClanVM);
      Utillty.DebugDisplayMessage("_addPregCtrlClanLayer Add Layer Success!");
    }

    public PregnancyControlUIBehavior() : base()
    {
    }
  }
}
