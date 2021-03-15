// Decompiled with JetBrains decompiler
// Type: PregnancyControl.PregnancyControlBehavior
// Assembly: PregnancyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3C229AC-D888-43DF-B6C0-C6CC1796F44D
// Assembly location: C:\Users\AARTZ\Downloads\Dead Mod Pack-2552-1-0-4-1609429197\Modules\PregnancyControl\bin\Win64_Shipping_Client\PregnancyControl.dll

using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace PregnancyControl
{
  public class PregnancyControlBehavior : CampaignBehaviorBase
  {
    private List<PregnancyControlBehavior.SexInfo> m_sexInformationList;
    private Dictionary<Hero, PregnancyControlBehavior.PrePregnancyInfo> m_prePregnancyInfoMap;
    private Dictionary<Hero, Hero> m_preAbortionMap;
    private Dictionary<Hero, int> m_dailySexTimeCountMap;

    public Dictionary<Hero, Hero> PreAbortionMap
    {
      get
      {
        return this.m_preAbortionMap;
      }
    }

    public override void RegisterEvents()
    {
      CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener((object) this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
      CampaignEvents.DailyTickEvent.AddNonSerializedListener((object) this, new Action(this.DailyTick));
      CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener((object) this, new Action<Hero>(this.DailyTickHero));
    }

    public override void SyncData(IDataStore dataStore)
    {
      // ISSUE: cast to a reference type
      dataStore.SyncData<List<PregnancyControlBehavior.SexInfo>>("m_sexInformationList", ref this.m_sexInformationList);
      if (this.m_sexInformationList == null)
        this.m_sexInformationList = new List<PregnancyControlBehavior.SexInfo>();
      // ISSUE: cast to a reference type
      dataStore.SyncData<Dictionary<Hero, PregnancyControlBehavior.PrePregnancyInfo>>("m_prePregnancyInfoMap", ref this.m_prePregnancyInfoMap);
      if (this.m_prePregnancyInfoMap == null)
        this.m_prePregnancyInfoMap = new Dictionary<Hero, PregnancyControlBehavior.PrePregnancyInfo>();
      // ISSUE: cast to a reference type
      dataStore.SyncData<Dictionary<Hero, Hero>>("m_preAbortionMap", ref this.m_preAbortionMap);
      if (this.m_preAbortionMap == null)
        this.m_preAbortionMap = new Dictionary<Hero, Hero>();
      // ISSUE: cast to a reference type
      dataStore.SyncData<Dictionary<Hero, int>>("m_dailySexTimeCountMap", ref this.m_dailySexTimeCountMap);
      if (this.m_dailySexTimeCountMap != null)
        return;
      this.m_dailySexTimeCountMap = new Dictionary<Hero, int>();
    }

    public void OnSessionLaunched(CampaignGameStarter starter)
    {
      this._addSexDialogs(starter);
      this._addAbortionDialogs(starter);
    }

    public void DailyTick()
    {
      this._cleanDailySexCount();
      this._maintainPrePregnanciesInfo();
    }

    public void DailyTickHero(Hero hero)
    {
      this.CheckAddNewPregnancies(hero);
    }

    private void _addSexDialogs(CampaignGameStarter starter)
    {
      // ISSUE: method pointer
      starter.AddPlayerLine("hero_sex_nor_ask", "hero_main_options", "hero_sex_nor_reply", "{SEX_NOR_ASK}", new ConversationSentence.OnConditionDelegate(conversation_player_sex_nor_ask_on_condition), (ConversationSentence.OnConsequenceDelegate) null, 100, (ConversationSentence.OnClickableConditionDelegate) null, (ConversationSentence.OnPersuasionOptionDelegate) null);
      // ISSUE: method pointer
      starter.AddDialogLine("hero_sex_nor_reply", "hero_sex_nor_reply", "lord_pretalk", "{SEX_NOR_RESP}", new ConversationSentence.OnConditionDelegate(conversation_player_sex_nor_reply_on_condition), (ConversationSentence.OnConsequenceDelegate) null, 100, (ConversationSentence.OnClickableConditionDelegate) null);
    }

    public bool conversation_player_sex_nor_ask_on_condition()
    {
      Hero mainHero = Hero.MainHero;
      Hero conversationHero = Hero.OneToOneConversationHero;
      bool flag1 = Config.Instance.EnableNotSpouseGaySex || (mainHero == conversationHero.Spouse || mainHero.IsFemale != conversationHero.IsFemale);
      bool sex = this.IsEnoughRealtionToSex(mainHero, conversationHero);
      bool flag2 = this.IsSexEnableToday(mainHero);
      TextObject empty = (TextObject) TextObject.Empty;
      MBTextManager.SetTextVariable("SEX_NOR_ASK", ((object) new TextObject(Utillty.ET("{=pcm_sex_nor_ask_1}Less talking! More fucking!"), null)).ToString(), false);
      return !conversationHero.IsWanderer & flag1 & sex & flag2;
    }

    public bool conversation_player_sex_nor_reply_on_condition()
    {
      Hero mainHero = Hero.MainHero;
      Hero conversationHero = Hero.OneToOneConversationHero;
      TextObject empty = (TextObject) TextObject.Empty;
      TextObject textObject;
      if (this.IsPreAbortion(mainHero))
        textObject = new TextObject(Utillty.ET("{=pcm_sex_nor_reply_me_abor_1}You are making preparations for an abortion. Not this time."), null);
      else if (this.IsPreAbortion(conversationHero))
        textObject = new TextObject(Utillty.ET("{=pcm_sex_nor_reply_sp_abor_1}I'm making preparations for an abortion. Not this time."), null);
      else if ((bool) mainHero.IsPregnant)
        textObject = new TextObject(Utillty.ET("{=pcm_sex_nor_reply_me_preg_1}You are pregnant. Not this time."), null);
      else if ((bool) conversationHero.IsPregnant)
        textObject = new TextObject(Utillty.ET("{=pcm_sex_nor_reply_sp_preg_1}I'm pregnant. Not this time."), null);
      else if (!this.IsSexEnableDate(mainHero, conversationHero) || !this.IsSexEnableToday(conversationHero))
      {
        textObject = !conversationHero.IsFemale ? new TextObject(Utillty.ET("{=pcm_sex_nor_reply_dis_ma_1}Uhhh... maybe some other time."), null) : new TextObject(Utillty.ET("{=pcm_sex_nor_reply_dis_fe_1}Not this time."), null);
      }
      else
      {
        textObject = new TextObject(Utillty.ET("{=pcm_sex_nor_reply_suc_1}A great idea my {TITLE}."), null);
        textObject.SetTextVariable("TITLE", Hero.MainHero.IsFemale ? "lady" : "lord");
        this.RecordSexInfo(mainHero, conversationHero);
        this.RecordPrePregnantInfo(mainHero, conversationHero);
      }
      MBTextManager.SetTextVariable("SEX_NOR_RESP", ((object) textObject).ToString(), false);
      return true;
    }

    private void _addAbortionDialogs(CampaignGameStarter starter)
    {
      // ISSUE: method pointer
      starter.AddPlayerLine("hero_abortion_ask", "hero_main_options", "hero_abortion_reply", "{ABORTION_ASK}", new ConversationSentence.OnConditionDelegate(conversation_player_abortion_ask_on_condition), (ConversationSentence.OnConsequenceDelegate) null, 100, (ConversationSentence.OnClickableConditionDelegate) null, (ConversationSentence.OnPersuasionOptionDelegate) null);
      // ISSUE: method pointer
      starter.AddDialogLine("hero_abortion_reply", "hero_abortion_reply", "lord_pretalk", "{ABORTION_REPLY}", new ConversationSentence.OnConditionDelegate(conversation_player_abortion_reply_on_condition), (ConversationSentence.OnConsequenceDelegate) null, 100, (ConversationSentence.OnClickableConditionDelegate) null);
    }

    public bool conversation_player_abortion_ask_on_condition()
    {
      Hero conversationHero = Hero.OneToOneConversationHero;
      TextObject empty = (TextObject) TextObject.Empty;
      MBTextManager.SetTextVariable("ABORTION_ASK", ((object) new TextObject(Utillty.ET("{=pcm_abor_ask_1}I don't want our unborn child anymore."), null)).ToString(), false);
      return PregCampHelper.IsInPregnancies(conversationHero, Hero.MainHero) && !this.IsPreAbortion(conversationHero);
    }

    public bool conversation_player_abortion_reply_on_condition()
    {
      Hero conversationHero = Hero.OneToOneConversationHero;
      TextObject empty = (TextObject) TextObject.Empty;
      MBTextManager.SetTextVariable("ABORTION_REPLY", ((object) new TextObject(Utillty.ET("{=pcm_abor_reply_1}Ok."), null)).ToString(), false);
      this.RecordPreAbortion(conversationHero, Hero.MainHero);
      return true;
    }

    public void RecordSexInfo(Hero sexPartner1, Hero sexPartner2)
    {
      if (sexPartner1 == null || sexPartner1.IsDead || (sexPartner2 == null || sexPartner2.IsDead))
        return;
      CampaignTime nextSexEnableDate = CampaignTime.DaysFromNow((float) MBRandom.RandomInt(Config.Instance.MinSexDelayDays, Config.Instance.MaxSexDelayDays));
      if (this.m_dailySexTimeCountMap.ContainsKey(sexPartner1))
        ++this.m_dailySexTimeCountMap[sexPartner1];
      else
        this.m_dailySexTimeCountMap.Add(sexPartner1, 1);
      if (this.m_dailySexTimeCountMap.ContainsKey(sexPartner2))
        ++this.m_dailySexTimeCountMap[sexPartner2];
      else
        this.m_dailySexTimeCountMap.Add(sexPartner2, 1);
      int index = this.m_sexInformationList.FindIndex((Predicate<PregnancyControlBehavior.SexInfo>) (info => info.m_sexPartner1 == sexPartner1 && info.m_sexPartner2 == sexPartner2 || info.m_sexPartner2 == sexPartner1 && info.m_sexPartner1 == sexPartner2));
      if (index >= 0)
        this.m_sexInformationList[index].m_nextSexEnableDate = (this.m_sexInformationList[index].m_nextSexEnableDate >= nextSexEnableDate) ? this.m_sexInformationList[index].m_nextSexEnableDate : nextSexEnableDate;
      else
        this.m_sexInformationList.Add(new PregnancyControlBehavior.SexInfo(sexPartner1, sexPartner2, nextSexEnableDate));
      Utillty.DebugDisplayMessage("_recordSexInfo Success! Partner1 : " + ((object) sexPartner1.Name).ToString() + " Partner2 : " + ((object) sexPartner2.Name).ToString() + " NexSexEnableDate : " + nextSexEnableDate.ToString());
    }

    private void _cleanDailySexCount()
    {
      this.m_dailySexTimeCountMap.Clear();
    }

    public bool IsSexEnableToday(Hero hero)
    {
      return hero != null && (!this.m_dailySexTimeCountMap.ContainsKey(hero) || this.m_dailySexTimeCountMap[hero] < Config.Instance.DailySexLimitForEachHero);
    }

    public bool IsSexEnableDate(Hero sexPartner1, Hero sexPartner2)
    {
      if (sexPartner1 == null || sexPartner1.IsDead || (sexPartner2 == null || sexPartner2.IsDead))
        return false;
      PregnancyControlBehavior.SexInfo sexInfo = this.m_sexInformationList.Find((Predicate<PregnancyControlBehavior.SexInfo>) (info => info.m_sexPartner1 == sexPartner1 && info.m_sexPartner2 == sexPartner2 || info.m_sexPartner2 == sexPartner1 && info.m_sexPartner1 == sexPartner2));
      return sexInfo == null || sexInfo.m_nextSexEnableDate.ElapsedDaysUntilNow > 0.0;
    }

    public bool IsEnoughRealtionToSex(Hero partner1, Hero partner2)
    {
      if (partner1 == null || partner2 == null)
        return false;
      float relation = (float) partner2.GetRelation(partner1);
      return partner2.Spouse != partner1 ? (partner2.Spouse != null ? (double) relation >= (double) Config.Instance.AdultrySexRealtionLimit : (double) relation >= (double) Config.Instance.NormalSexRealtionLimit) : (double) relation >= (double) Config.Instance.SpouseSexRealtionLimit;
    }

    public void RecordPrePregnantInfo(Hero sexPartner1, Hero sexPartner2)
    {
      if ((double) MBRandom.RandomFloatRanged(0.0f, 1f) > (double) Config.Instance.PregnancyProbability || sexPartner1 == null || sexPartner2 == null || !Config.Instance.EnableLesbianPregnancy && sexPartner1.IsFemale && sexPartner2.IsFemale)
        return;
      Hero key = (Hero) null;
      Hero father = (Hero) null;
      if (sexPartner1.IsAlive && sexPartner1.IsFemale && (sexPartner1.IsPregnant == false && (double) sexPartner1.Age > (double) Config.Instance.MinAge))
      {
        key = sexPartner1;
        father = sexPartner2;
      }
      if (sexPartner2.IsAlive && sexPartner2.IsFemale && (sexPartner2.IsPregnant == false && (double) sexPartner2.Age > (double) Config.Instance.MinAge) && (key == null || MBRandom.RandomInt(0, 1) == 0))
      {
        key = sexPartner2;
        father = sexPartner1;
      }
      if (key == null || father == null)
        return;
      CampaignTime pregnantDate = CampaignTime.DaysFromNow((float) MBRandom.RandomInt(Config.Instance.MinPregnancyDelayDays, Config.Instance.MaxPregnancyDelayDays));
      if (this.m_prePregnancyInfoMap.ContainsKey(key))
      {
        if (this.m_prePregnancyInfoMap[key].m_pregnantDate > pregnantDate)
        {
          this.m_prePregnancyInfoMap[key].m_father = father;
          this.m_prePregnancyInfoMap[key].m_pregnantDate = pregnantDate;
        }
      }
      else
        this.m_prePregnancyInfoMap.Add(key, new PregnancyControlBehavior.PrePregnancyInfo(father, pregnantDate));
      Utillty.DebugDisplayMessage("_recordPrePregnantInfo Success! Mother : " + ((object) key.Name).ToString() + " Father : " + ((object) father.Name).ToString() + " PregnantDate : " + pregnantDate.ToString());
    }

    public bool IsTimeToAddNewPregnant(Hero mother, Hero father = null)
    {
      if (mother == null || !this.m_prePregnancyInfoMap.ContainsKey(mother))
        return false;
      PregnancyControlBehavior.PrePregnancyInfo prePregnancyInfo = this.m_prePregnancyInfoMap[mother];
      if (!mother.IsFemale || mother.IsDead || (double) mother.Age <= (double) Config.Instance.MinAge || father != null && prePregnancyInfo.m_father != father)
        return false;
      CampaignTime pregnantDate = prePregnancyInfo.m_pregnantDate;
      return prePregnancyInfo.m_pregnantDate.ElapsedDaysUntilNow >= 0.0;
    }

    public bool CheckAddNewPregnancies(Hero hero)
    {
      if (!this.IsTimeToAddNewPregnant(hero, (Hero) null))
        return false;
      MakePregnantAction.Apply(hero);
      PregCampHelper.DoChildConceivedMethod(hero);
      this.m_prePregnancyInfoMap.Remove(hero);
      return true;
    }

    private void _maintainPrePregnanciesInfo()
    {
      List<Hero> heroList = new List<Hero>();
      using (Dictionary<Hero, PregnancyControlBehavior.PrePregnancyInfo>.Enumerator enumerator = this.m_prePregnancyInfoMap.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<Hero, PregnancyControlBehavior.PrePregnancyInfo> current = enumerator.Current;
          if (!current.Key.IsDead && current.Key.IsPregnant == false)
          {
            CampaignTime pregnantDate = current.Value.m_pregnantDate;
          }
          else
            heroList.Add(current.Key);
        }
      }
      using (List<Hero>.Enumerator enumerator = heroList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Hero current = enumerator.Current;
          this.m_prePregnancyInfoMap.Remove(current);
          Utillty.DebugDisplayMessage("_checkAddNewPregnancies Remove! Mother : " + ((object) current.Name).ToString());
        }
      }
    }

    public Hero GetRealFatherInCurPregnancy(Hero mother)
    {
      if (mother == null)
        return (Hero) null;
      if (this.m_prePregnancyInfoMap.ContainsKey(mother))
      {
        CampaignTime pregnantDate = this.m_prePregnancyInfoMap[mother].m_pregnantDate;
        if (this.m_prePregnancyInfoMap[mother].m_pregnantDate.ElapsedDaysUntilNow > 0.0)
          return this.m_prePregnancyInfoMap[mother].m_father;
      }
      return mother.Spouse;
    }

    public void RecordPreAbortion(Hero mother, Hero caused)
    {
      if (mother == null || !PregCampHelper.IsInPregnancies(mother, (Hero) null) || this.m_preAbortionMap.ContainsKey(mother))
        return;
      this.m_preAbortionMap.Add(mother, caused);
      Utillty.DebugDisplayMessage("_recordAbortion Success! Mother : " + ((object) mother.Name).ToString());
    }

    public bool IsPreAbortion(Hero mother)
    {
      return mother != null && !mother.IsDead && mother.IsPregnant != false && this.m_preAbortionMap.ContainsKey(mother);
    }

    public void CleanPreAbortionList()
    {
      this.m_preAbortionMap.Clear();
      Utillty.DebugDisplayMessage("CleanPreAbortionList Success!");
    }

    public void DoAbortionNotificationAndLog(Hero mother, Hero causedHero)
    {
      TextObject textObject;
      if (mother == Hero.MainHero)
        textObject = new TextObject(Utillty.ET("{=pcm_child_abortion_ntf_self}You just had an abortion."), null);
      else if (mother == Hero.MainHero.Spouse)
      {
        textObject = new TextObject(Utillty.ET("{=pcm_child_abortion_ntf_spouse}Your spouse {MOTHER} just had an abortion."), null);
        textObject.SetTextVariable("MOTHER", (TextObject) mother.Name);
      }
      else if (mother.Clan == Clan.PlayerClan)
      {
        textObject = new TextObject(Utillty.ET("{=pcm_child_abortion_ntf_clan}Your clan member {MOTHER} just had an abortion."), null);
        textObject.SetTextVariable("MOTHER", (TextObject) mother.Name);
      }
      else
      {
        if (causedHero != Hero.MainHero)
          return;
        textObject = new TextObject(Utillty.ET("{=pcm_child_abortion_ntf_caused}{MOTHER} just had an abortion."), null);
        textObject.SetTextVariable("MOTHER", (TextObject) mother.Name);
      }
      InformationManager.AddQuickInformation(textObject, 0, (BasicCharacterObject) null, "");
      LogEntry.AddLogEntry((LogEntry) new AbortionLogEntry(mother, causedHero), CampaignTime.Now);
    }

    public PregnancyControlBehavior() : base()
    {
    }

    public class SexInfo
    {
      [SaveableField(1)]
      public Hero m_sexPartner1;
      [SaveableField(2)]
      public Hero m_sexPartner2;
      [SaveableField(3)]
      public CampaignTime m_nextSexEnableDate;

      public SexInfo(Hero sexPartner1, Hero sexPartner2, CampaignTime nextSexEnableDate)
      {
        this.m_sexPartner1 = sexPartner1;
        this.m_sexPartner2 = sexPartner2;
        this.m_nextSexEnableDate = nextSexEnableDate;
      }
    }

    public class PrePregnancyInfo
    {
      [SaveableField(1)]
      public Hero m_father;
      [SaveableField(2)]
      public CampaignTime m_pregnantDate;

      public PrePregnancyInfo(Hero father, CampaignTime pregnantDate)
      {
        this.m_father = father;
        this.m_pregnantDate = pregnantDate;
      }
    }

    public class PregnancyControlBehaviorTypeDefiner : CampaignBehaviorBase.SaveableCampaignBehaviorTypeDefiner
    {
      public PregnancyControlBehaviorTypeDefiner() : base(13290807)
      {
      }

      protected override void DefineClassTypes()
      {
        AddClassDefinition(typeof (PregnancyControlBehavior.SexInfo), 1);
        AddClassDefinition(typeof (PregnancyControlBehavior.PrePregnancyInfo), 2);
        AddClassDefinition(typeof (AbortionLogEntry), 3);
      }

      protected override void DefineContainerDefinitions()
      {
        ConstructContainerDefinition(typeof (List<PregnancyControlBehavior.SexInfo>));
        ConstructContainerDefinition(typeof (Dictionary<Hero, PregnancyControlBehavior.PrePregnancyInfo>));
        ConstructContainerDefinition(typeof (Dictionary<Hero, Hero>));
      }
    }
  }
}
