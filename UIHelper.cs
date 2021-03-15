// Decompiled with JetBrains decompiler
// Type: PregnancyControl.UIHelper
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using SandBox.GauntletUI;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;

namespace PregnancyControl
{
  public static class UIHelper
  {
    public static FieldInfo m_clanScreenGauntletLayerField = AccessTools.Field(typeof (GauntletClanScreen), "_gauntletLayer");
    public static FieldInfo m_clanScreenClanVMField = AccessTools.Field(typeof (GauntletClanScreen), "_dataSource");

    public static GauntletLayer GetClanScreenGauntletLayer()
    {
      GauntletClanScreen clanScreenInstance = Utillty.GetClanScreenInstance();
      return clanScreenInstance == null || UIHelper.m_clanScreenGauntletLayerField == (FieldInfo) null ? (GauntletLayer) null : UIHelper.m_clanScreenGauntletLayerField.GetValue((object) clanScreenInstance) as GauntletLayer;
    }

    public static ClanManagementVM GetClanScreenClanVM()
    {
      GauntletClanScreen clanScreenInstance = Utillty.GetClanScreenInstance();
      return clanScreenInstance == null || UIHelper.m_clanScreenGauntletLayerField == (FieldInfo) null ? (ClanManagementVM) null : UIHelper.m_clanScreenClanVMField.GetValue((object) clanScreenInstance) as ClanManagementVM;
    }

    public static bool IsSelectedMember()
    {
      ClanManagementVM clanScreenClanVm = UIHelper.GetClanScreenClanVM();
      return clanScreenClanVm != null && clanScreenClanVm.ClanMembers != null && clanScreenClanVm.ClanMembers.IsSelected;
    }

    public static Hero GetCurSelectedMember()
    {
      ClanManagementVM clanScreenClanVm = UIHelper.GetClanScreenClanVM();
      return clanScreenClanVm == null || clanScreenClanVm.ClanMembers == null || clanScreenClanVm.ClanMembers.CurrentSelectedMember == null ? (Hero) null : clanScreenClanVm.ClanMembers.CurrentSelectedMember.GetHero();
    }

    public static void InvokeHintViewModelExecuteEndHint(object instance)
    {
      MethodInfo method = typeof (HintViewModel).GetMethod("ExecuteEndHint", BindingFlags.Instance | BindingFlags.NonPublic);
      if (method == (MethodInfo) null)
        return;
      method.Invoke(instance, (object[]) null);
    }
  }
}
