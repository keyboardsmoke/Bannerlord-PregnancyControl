// Decompiled with JetBrains decompiler
// Type: PregnancyControl.AbortionLogEntry
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using Helpers;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace PregnancyControl
{
  [SaveableClass(53290807)]
  public class AbortionLogEntry : LogEntry, IEncyclopediaLog, IChatNotification
  {
    [SaveableField(1)]
    public readonly Hero m_mother;
    [SaveableField(2)]
    public readonly Hero m_causedHero;

    public bool IsVisibleNotification
    {
      get
      {
        return ((object) this.m_mother.Clan).Equals((object) Hero.MainHero.Clan) || this.m_causedHero != null && this.m_causedHero.IsHumanPlayerCharacter;
      }
    }

    public override ChatNotificationType NotificationType
    {
      get
      {
        return this.CivilianNotification((IFaction) this.m_mother.Clan);
      }
    }

    public override CampaignTime KeepInHistoryTime
    {
      get
      {
        return CampaignTime.Days(Campaign.Current.Models.PregnancyModel.PregnancyDurationInDays + 7f);
      }
    }

    public AbortionLogEntry(Hero mother, Hero causedHero)
    {
      this.m_mother = mother;
      this.m_causedHero = causedHero;
    }

    public override string ToString()
    {
      return ((object) this.GetEncyclopediaText()).ToString();
    }

    public TextObject GetNotificationText()
    {
      return this.GetEncyclopediaText();
    }

    public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase
    {
      return (object) obj == this.m_mother;
    }

    public TextObject GetEncyclopediaText()
    {
      TextObject textObject = new TextObject(Utillty.ET("{=pcm_child_abortion_log}{MOTHER.LINK} had an abortion."), null);
      StringHelpers.SetCharacterProperties("MOTHER", this.m_mother.CharacterObject, (TextObject) null, textObject, false);
      return textObject;
    }
  }
}
