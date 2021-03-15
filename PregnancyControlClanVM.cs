// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregnancyControlClanVM
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace PregnancyControl
{
  public class PregnancyControlClanVM : ViewModel
  {
    private HintViewModel m_abortionHint;

    public HintViewModel AbortionHint
    {
      get
      {
        return this.m_abortionHint;
      }
      set
      {
        this.m_abortionHint = value;
        this.OnPropertyChanged(nameof (AbortionHint));
      }
    }

    public PregnancyControlClanVM() : base()
    {
      var to = new TextObject(Utillty.ET("{=pcm_clan_abortion_button_hint}Make current selected member to have an abortion."), null);
      this.AbortionHint = new HintViewModel(to, (string) null);
    }

    public override void RefreshValues()
    {
      base.RefreshValues();
    }

    public void OnClickAbortionButton()
    {
      Hero curSelectedMember = UIHelper.GetCurSelectedMember();
      if (curSelectedMember == null)
        return;
      TextObject textObject;
      if (curSelectedMember == Hero.MainHero)
      {
        textObject = new TextObject(Utillty.ET("{=pcm_clan_abortion_ask_msg_self}Do you want to have an abortion?"), null);
      }
      else
      {
        textObject = new TextObject(Utillty.ET("{=pcm_clan_abortion_ask_msg_clan}Do you want {MOTHER} to have an abortion?"), null);
        textObject.SetTextVariable("MOTHER", (TextObject) curSelectedMember.Name);
      }
      UIHelper.InvokeHintViewModelExecuteEndHint((object) this.m_abortionHint);
      InformationManager.ShowInquiry(new InquiryData(((object) new TextObject("", null)).ToString(), ((object) textObject).ToString(), true, true, ((object) new TextObject(Utillty.ET("{=aeouhelq}Yes"), null)).ToString(), ((object) new TextObject(Utillty.ET("{=8OkPHu4f}No"), null)).ToString(), (Action) (() => this._recordPreAbortion()), (Action) null, ""), false);
    }

    private void _recordPreAbortion()
    {
      Hero curSelectedMember = UIHelper.GetCurSelectedMember();
      if (curSelectedMember == null)
        return;
      Utillty.GetPregnancyControlBehaviorInstance()?.RecordPreAbortion(curSelectedMember, (Hero) null);
    }
  }
}
