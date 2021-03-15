// Decompiled with JetBrains decompiler
// Type: PregnancyControl.ClanAbortionButtonWidget
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using TaleWorlds.CampaignSystem;
using TaleWorlds.GauntletUI;

namespace PregnancyControl
{
  public class ClanAbortionButtonWidget : ButtonWidget
  {
    public ClanAbortionButtonWidget(UIContext context) : base(context)
    {
    }

    protected override void OnUpdate(float dt)
    {
      base.OnUpdate(dt);
      IsEnabled = _isShowAbortionButton();
    }

    protected override void OnClick()
    {
      base.OnClick();
    }

    private bool _isShowAbortionButton()
    {
      if (!UIHelper.IsSelectedMember())
        return false;
      Hero curSelectedMember = UIHelper.GetCurSelectedMember();
      if (curSelectedMember == null)
        return false;
      PregnancyControlBehavior behaviorInstance = Utillty.GetPregnancyControlBehaviorInstance();
      return behaviorInstance != null && PregCampHelper.IsInPregnancies(curSelectedMember, (Hero) null) && !behaviorInstance.IsPreAbortion(curSelectedMember);
    }
  }
}
