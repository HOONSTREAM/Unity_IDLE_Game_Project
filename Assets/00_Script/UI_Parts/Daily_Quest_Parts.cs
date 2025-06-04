using System;
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
    public GameObject Hand_Icon;

    public static Action Pressed_Daily_Quest_Parts_Reward;

    public void Init(DailyQuest quest, UI_Daily_Quest parentData)
    {
        Hand_Icon.gameObject.SetActive(false);

        if (TypeCount(quest.Type) >= quest.Goal)
        {
            Hand_Icon.gameObject.SetActive(true);
        }

        parent = parentData;
        CheckInit(quest.Type);
        m_Quest = quest;
        TItle.text = quest.Quest_Title;
        Description.text = quest.Quest_Description;
        RewardValue.text = quest.RewardCount.ToString();
        RewardImage.sprite = Utils.Get_Atlas(quest.Reward);
        Count.text = string.Format("({0}/{1})", TypeCount(quest.Type), quest.Goal);

       
    }

    public void GetReward()
    {
        if (TypeCount(m_Quest.Type) < m_Quest.Goal)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("퀘스트 달성조건이 충족되지 않았습니다.");
            return;
        }

        Base_Canvas.instance.Get_UI("UI_Reward");

        Utils.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(m_Quest.Reward, m_Quest.RewardCount); // 보상지급

        switch (m_Quest.Type)
        {
            case Daily_Quest_Type.Daily_Attendance: Data_Manager.Main_Players_Data.Daily_Attendance_Clear = true; break;
            case Daily_Quest_Type.Level_up: Data_Manager.Main_Players_Data.Level_up_Clear = true; break;
            case Daily_Quest_Type.Summon: Data_Manager.Main_Players_Data.Summon_Clear = true; break;
            case Daily_Quest_Type.Relic: Data_Manager.Main_Players_Data.Relic_Clear = true; break;
            case Daily_Quest_Type.Dungeon_Gold: Data_Manager.Main_Players_Data.Dungeon_Gold_Clear = true; break;
            case Daily_Quest_Type.Dungeon_Dia: Data_Manager.Main_Players_Data.Dungeon_Dia_Clear = true; break;

        }

        Pressed_Daily_Quest_Parts_Reward.Invoke();
        Hand_Icon.gameObject.SetActive(false);
        parent.Init();

        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
    }

    private void CheckInit(Daily_Quest_Type type)
    {
        bool GetCollected = false;

        switch (type)
        {
            case Daily_Quest_Type.Daily_Attendance: GetCollected = Data_Manager.Main_Players_Data.Daily_Attendance_Clear; break;
            case Daily_Quest_Type.Level_up: GetCollected = Data_Manager.Main_Players_Data.Level_up_Clear; break;
            case Daily_Quest_Type.Summon: GetCollected = Data_Manager.Main_Players_Data.Summon_Clear; break;
            case Daily_Quest_Type.Relic: GetCollected = Data_Manager.Main_Players_Data.Relic_Clear; break;
            case Daily_Quest_Type.Dungeon_Gold: GetCollected = Data_Manager.Main_Players_Data.Dungeon_Gold_Clear; break;
            case Daily_Quest_Type.Dungeon_Dia: GetCollected = Data_Manager.Main_Players_Data.Dungeon_Dia_Clear; break;
        }

        CollectedPanel.SetActive(GetCollected);

        if (CollectedPanel.activeSelf) // 보상 수령하였으면 핸드 아이콘 비활성화
        {
            Hand_Icon.gameObject.SetActive(false);
        }

    }

    public int TypeCount(Daily_Quest_Type type)
    {
        switch (type)
        {
            case Daily_Quest_Type.Daily_Attendance: return Data_Manager.Main_Players_Data.Daily_Attendance;
            case Daily_Quest_Type.Level_up: return Data_Manager.Main_Players_Data.Levelup;
            case Daily_Quest_Type.Summon: return Data_Manager.Main_Players_Data.Summon;
            case Daily_Quest_Type.Relic: return Data_Manager.Main_Players_Data.Relic;
            case Daily_Quest_Type.Dungeon_Gold: return Data_Manager.Main_Players_Data.Dungeon_Gold;
            case Daily_Quest_Type.Dungeon_Dia: return Data_Manager.Main_Players_Data.Dungeon_Dia;
        }
        return -1;
    }
}
