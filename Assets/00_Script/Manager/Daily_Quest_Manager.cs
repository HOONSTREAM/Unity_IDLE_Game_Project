using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class DailyQuest
{
    public Daily_Quest_Type Type;
    public string Quest_Title;
    public string Quest_Description;
    public int Goal;
    public int CurrentProgress;
    public string Reward;
    public int RewardCount;

    public bool IsCompleted => CurrentProgress >= Goal;

    public DailyQuest(Daily_Quest_Type type, string quest_Title, string quest_Description, int goal, string reward, int rewardCount)
    {
        Type = type;
        Quest_Title = quest_Title;
        Quest_Description = quest_Description;
        Goal = goal;
        CurrentProgress = 0;
        Reward = reward;
        RewardCount = rewardCount;
    }

    public void UpdateProgress(int value)
    {
        if (!IsCompleted)
        {
            CurrentProgress += value;
            if (CurrentProgress >= Goal)
                CurrentProgress = Goal;
        }
    }
}
public class Achievement_Status
{
    public float ATK;
    public float HP;
    public float MONEY;
    public float ITEM;
    public float SKILL;
    public float ATK_SPEED;
    public float CRITICAL_P;
    public float CRITICAL_D;

    //public void GetStatusData(Status_Holder holder, float value)
    //{
    //    switch (holder)
    //    {
    //        case Status_Holder.ATK: ATK += value; break;
    //        case Status_Holder.HP: HP += value; break;
    //        case Status_Holder.MONEY: MONEY += value; break;
    //        case Status_Holder.ITEM: ITEM += value; break;
    //        case Status_Holder.SKILL: SKILL += value; break;
    //        case Status_Holder.ATK_SPEED: ATK_SPEED += value; break;
    //        case Status_Holder.CRITICAL_P: CRITICAL_P += value; break;
    //        case Status_Holder: CRITICAL_D += value; break;
    //    }
    //}
}
public class Daily_Quest_Manager
{
    List<Dictionary<string, object>> questData;
    public List<DailyQuest> activeQuests = new List<DailyQuest>();

    //public Achievement_Scriptable scriptable_Data;
    public List<Achievement> Achievement_Lists = new List<Achievement>();
    public Achievement_Status Achivewment_status_Data = new Achievement_Status();

    public void Init()
    {
        //LoadQuests();
        //scriptable_Data = Resources.Load<Achievement_Scriptable>("Scriptable/Achievement_Data");
        //Achievement_Lists = scriptable_Data.Achievement_Data;
        //LoadAchivement_Data();
    }

    //public void LoadAchivement_Data()
    //{
    //    Achievement_Status achievementStatus = new Achievement_Status();
    //    for (int i = 0; i < Data_Mng.m_Data.Achievement_B.Length; i++)
    //    {
    //        if (Data_Mng.m_Data.Achievement_B[i] == true)
    //        {
    //            achievementStatus.GetStatusData(Achievement_Lists[i].GetRewardStatus, Achievement_Lists[i].RewardValue);
    //        }
    //    }
    //    Achivewment_status_Data = achievementStatus;
    //}

    public void LoadQuests()
    {
        questData = new List<Dictionary<string, object>>(CSV_Importer.Daily_Quest_Design);
        for (int i = 0; i < questData.Count; i++)
        {
            var data = questData[i];
            Daily_Quest_Type type = (Daily_Quest_Type)Enum.Parse(typeof(Daily_Quest_Type), data["Type"].ToString());
            string questId = data["Title"].ToString();
            string description = data["Description"].ToString();
            int goal = int.Parse(data["Quest_Count"].ToString());
            string reward = data["Reward"].ToString();
            int rewardCount = int.Parse(data["Reward_Value"].ToString());

            DailyQuest quest = new DailyQuest(type, questId, description, goal, reward, rewardCount);
            activeQuests.Add(quest);
        }
    }
}




