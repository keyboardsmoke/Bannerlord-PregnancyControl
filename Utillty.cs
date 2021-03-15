// Decompiled with JetBrains decompiler
// Type: PregnancyControl.Utillty
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using HarmonyLib;
using SandBox.GauntletUI;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace PregnancyControl
{
  public static class Utillty
  {
    public static FieldInfo m_currentLanguageIdField = AccessTools.Field(typeof (MBTextManager), "_currentLanguageId");

    public static void DebugDisplayMessage(string information)
    {
    }

    public static PregnancyCampaignBehavior GetPregnancyCampaignBehaviorInstance()
    {
      return (PregnancyCampaignBehavior) Campaign.Current.CampaignBehaviorManager.GetBehavior<PregnancyCampaignBehavior>();
    }

    public static PregnancyControlBehavior GetPregnancyControlBehaviorInstance()
    {
      return (PregnancyControlBehavior) Campaign.Current.CampaignBehaviorManager.GetBehavior<PregnancyControlBehavior>();
    }

    public static GauntletClanScreen GetClanScreenInstance()
    {
      return ScreenManager.TopScreen as GauntletClanScreen;
    }

    public static bool IsPrisonBelongHeroParty(Hero hero, Hero prison)
    {
      if (hero == null || prison == null || (!prison.IsPrisoner || prison.PartyBelongedToAsPrisoner == null))
        return false;
      MobileParty partyBelongedTo = hero.PartyBelongedTo;
      MobileParty mobileParty = prison.PartyBelongedToAsPrisoner.MobileParty;
      return partyBelongedTo != null && mobileParty != null && (partyBelongedTo == mobileParty || partyBelongedTo == mobileParty.AttachedTo || partyBelongedTo.AttachedTo == mobileParty || partyBelongedTo.AttachedTo == mobileParty.AttachedTo);
    }

    public static string ET(string text)
    {
      if (BannerlordConfig.Language != "English")
        return text;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      if (text == null || text.Length <= 2 || (text[0] != '{' || text[1] != '='))
        return text;
      for (int index1 = 2; index1 < text.Length; ++index1)
      {
        if (text[index1] != '}')
        {
          stringBuilder1.Append(text[index1]);
        }
        else
        {
          for (int index2 = index1 + 1; index2 < text.Length; ++index2)
            stringBuilder2.Append(text[index2]);
          if (!(stringBuilder1.ToString() == "*") && !(stringBuilder1.ToString() == "!"))
          {
            string input = LocalizedTextManager.GetTranslatedText(BannerlordConfig.Language, stringBuilder1.ToString());
            if (input != null)
            {
              string pattern = "{%.+?}";
              foreach (Match match in Regex.Matches(input, pattern))
                input = input.Replace(match.Value, "");
              return input;
            }
          }
          return stringBuilder2.ToString();
        }
      }
      return stringBuilder2.ToString();
    }
  }
}
