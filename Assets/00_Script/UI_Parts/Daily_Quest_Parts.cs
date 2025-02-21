using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Daily_Quest_Parts : MonoBehaviour
{
    public TextMeshProUGUI TItle, Description, Count, RewardValue;
    public Image RewardImage;
    DailyQuest m_Quest;
    public GameObject CollectedPanel;
    UI_Daily_Quest parent;

    //public void Init(DailyQuest quest, UI_Daily_Quest parentData)
    //{
    //    parent = parentData;
    //    CheckInit(quest.Type);
    //    m_Quest = quest;
    //    TItle.text = quest.Quest_Title;
    //    Description.text = quest.Quest_Description;
    //    RewardValue.text = quest.RewardCount.ToString();
    //    RewardImage.sprite = Utils.Get_Atlas(quest.Reward);
    //    Count.text = string.Format("({0}/{1})", TypeCount(quest.Type), quest.Goal);
    //    SliderFill.fillAmount = (float)TypeCount(quest.Type) / (float)quest.Goal;
    //}

    //public void GetReward()
    //{
    //    if (TypeCount(m_Quest.Type) < m_Quest.Goal)
    //    {
    //        Base_Canvas.instance.Get_Toast().Initalize("퀘스트를 완료하지 못 하였습니다.", Color.white);
    //        return;
    //    }

    //    Base_Canvas.instance.Get_UI("#Reward");
    //    Utils.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(m_Quest.Reward, m_Quest.RewardCount);
    //    switch (m_Quest.Type)
    //    {
    //        case DailyQuest_Type.DailyAttendance: Data_Mng.m_Data.DailyQuests[0] = true; break;
    //        case DailyQuest_Type.LevelUpButton: Data_Mng.m_Data.DailyQuests[1] = true; break;
    //        case DailyQuest_Type.Summon: Data_Mng.m_Data.DailyQuests[2] = true; break;
    //        case DailyQuest_Type.ADS: Data_Mng.m_Data.DailyQuests[3] = true; break;
    //        case DailyQuest_Type.Dungeon: Data_Mng.m_Data.DailyQuests[4] = true; break;
    //    }
    //    parent.Init();
    //}

    //private void CheckInit(DailyQuest_Type type)
    //{
    //    bool GetCollected = false;
    //    switch (type)
    //    {
    //        case DailyQuest_Type.DailyAttendance: GetCollected = Data_Mng.m_Data.DailyQuests[0]; break;
    //        case DailyQuest_Type.LevelUpButton: GetCollected = Data_Mng.m_Data.DailyQuests[1]; break;
    //        case DailyQuest_Type.Summon: GetCollected = Data_Mng.m_Data.DailyQuests[2]; break;
    //        case DailyQuest_Type.ADS: GetCollected = Data_Mng.m_Data.DailyQuests[3]; break;
    //        case DailyQuest_Type.Dungeon: GetCollected = Data_Mng.m_Data.DailyQuests[4]; break;
    //    }
    //    CollectedPanel.SetActive(GetCollected);
    //}

    //public int TypeCount(DailyQuest_Type type)
    //{
    //    switch (type)
    //    {
    //        case DailyQuest_Type.DailyAttendance: return Data_Mng.m_Data.DailyAttendance;
    //        case DailyQuest_Type.LevelUpButton: return Data_Mng.m_Data.LevelUp;
    //        case DailyQuest_Type.Summon: return Data_Mng.m_Data.Summon;
    //        case DailyQuest_Type.ADS: return Data_Mng.m_Data.ADS;
    //        case DailyQuest_Type.Dungeon: return Data_Mng.m_Data.Dungeon;
    //    }
    //    return -1;
    //}
}
