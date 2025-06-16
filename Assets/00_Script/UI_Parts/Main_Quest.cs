using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_Quest : MonoBehaviour
{
    List<Dictionary<string, object>> Data = new List<Dictionary<string, object>>();
    Dictionary<string, object> now_quest;

    public static int monster_index;

    [SerializeField] 
    private TextMeshProUGUI Title_Text, Explain_Text, Count_Text, Reward_Text;
    [SerializeField]
    private GameObject HandObj;
    [SerializeField]
    private GameObject All_Quest_Complete_OBJ;
    Quest_Type m_State;
    public static bool GetEnemy = false;
    bool reward = false;

    private readonly int MAX_QUEST_COUNT = 4999;

    private void Start()
    {
        Data = CSV_Importer.Quest_Design;

        if (Data_Manager.Main_Players_Data.Quest_Count >= MAX_QUEST_COUNT)
        {
            All_Quest_Complete_OBJ.gameObject.SetActive(true);
            HandObj.gameObject.SetActive(false);
            return;
        }

        Get_NextQuest();
    }

    private void Update()
    {
        if(Data_Manager.Main_Players_Data.Quest_Count <= MAX_QUEST_COUNT)
        {
            GetQuest();
        }    
    }

    /// <summary>
    /// 다음 퀘스트를 받아옵니다.
    /// </summary>
    public void Get_NextQuest()
    {
        monster_index = 0;
        
        now_quest = Data[Data_Manager.Main_Players_Data.Quest_Count];
        m_State = (Quest_Type)Enum.Parse(typeof(Quest_Type), now_quest["Key"].ToString());
        if (m_State == Quest_Type.Monster) GetEnemy = true;

        Title_Text.text = "퀘스트 - " + (Data_Manager.Main_Players_Data.Quest_Count + 1).ToString();
        Explain_Text.text = Localization_Counting(m_State);
        Reward_Text.text = now_quest["Reward"].ToString();
    }

    /// <summary>
    /// 실시간 유저의 퀘스트 진행상황
    /// </summary>
    void GetQuest()
    {
        if(Data_Manager.Main_Players_Data.Quest_Count >= MAX_QUEST_COUNT)
        {
            return;
        }

        Color color = Counting(m_State) >= Convert.ToInt32(now_quest["Value"]) ? Color.green : Color.red;

        Count_Text.text = "(" + Counting(m_State).ToString() + "/" + Convert.ToInt32(now_quest["Value"]) + ")";
        Count_Text.color = color;

        reward = Counting(m_State) >= Convert.ToInt32(now_quest["Value"]) ? true : false;

        if (HandObj.activeSelf != reward)
            HandObj.SetActive(reward);
    }

    public void GetQuestButton()
    {
        if(Data_Manager.Main_Players_Data.Quest_Count >= MAX_QUEST_COUNT)
        {
            All_Quest_Complete_OBJ.gameObject.SetActive(true);
            HandObj.gameObject.SetActive(false);
            return;
        }

        if (reward == false) return;

        Base_Manager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
        {
            value.GetComponent<Coin_Parent>().Init(Camera.main.ScreenToWorldPoint(transform.position), Coin_Type.Dia, Convert.ToInt32(now_quest["Reward"]));
        });

        Data_Manager.Main_Players_Data.Quest_Count++;
        Data_Manager.Main_Players_Data.EXP_Upgrade_Count = 0;
        Get_NextQuest();

    }

   
    /// <summary>
    /// 실제 유저의 퀘스트 진행상황을 카운트 합니다.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private int Counting(Quest_Type type)
    {
        switch (type)
        {
            case Quest_Type.Monster: return monster_index;
            case Quest_Type.Gold_DG: return Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1];
            case Quest_Type.Dia_DG: return Data_Manager.Main_Players_Data.Dungeon_Clear_Level[0];
            case Quest_Type.Upgrade: return Data_Manager.Main_Players_Data.EXP_Upgrade_Count;
            case Quest_Type.Hero: return Data_Manager.Main_Players_Data.Hero_Summon_Count;
            case Quest_Type.Stage: return Data_Manager.Main_Players_Data.Player_Stage;
        }
        return 0;
    }

    private string Localization_Counting(Quest_Type type)
    {
        switch (type)
        {
            case Quest_Type.Monster: return "몬스터 처치";
            case Quest_Type.Gold_DG: return "골드 던전 돌파";
            case Quest_Type.Dia_DG: return "보물 던전 돌파";
            case Quest_Type.Upgrade: return "경험치 획득하기";
            case Quest_Type.Hero: return "영웅 소환";
            case Quest_Type.Stage: return "스테이지 클리어";
        }
        return "";
    }
}
