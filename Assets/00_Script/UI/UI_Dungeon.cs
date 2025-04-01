using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dungeon : UI_Base
{

    [SerializeField]
    private TextMeshProUGUI Daily_Reset_Timer;
    [SerializeField]
    private TextMeshProUGUI[] KeyTexts;
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Enter_Request_Key;
    [SerializeField]
    private TextMeshProUGUI[] Clear_Assets; // 클리어 보상
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Levels; // 난이도

    [SerializeField] 
    private Button[] Key01ArrowButton, Key02ArrowButton;

    private int[] Level = new int[2];
    

    public override bool Init()
    {
        Main_UI.Instance.FadeInOut(true, true, null);

        for(int i = 0; i< KeyTexts.Length; i++)
        {
            KeyTexts[i].text = "(" + (Data_Manager.Main_Players_Data.Daily_Enter_Key[i] + Data_Manager.Main_Players_Data.User_Key_Assets[i]).ToString() + "/2)";
            Dungeon_Enter_Request_Key[i].color = (Data_Manager.Main_Players_Data.Daily_Enter_Key[i] + Data_Manager.Main_Players_Data.User_Key_Assets[i]) <= 0 ? Color.red : Color.green;
            Dungeon_Levels[i].text = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] + 1).ToString();
            Level[i] = Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i];
        }

        int levelCount = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1] + 1); 
        var value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, levelCount, Utils.Data.stageData.DROP_MONEY) * Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON;

        // 레벨디자인 필요
        Clear_Assets[0].text = ((Data_Manager.Main_Players_Data.Dungeon_Clear_Level[0] + 1) * Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON).ToString();
        Clear_Assets[1].text = StringMethod.ToCurrencyString(value);

        Key01ArrowButton[0].onClick.AddListener(() => ArrowButton(0, -1));
        Key01ArrowButton[1].onClick.AddListener(() => ArrowButton(0, 1));
        Key02ArrowButton[0].onClick.AddListener(() => ArrowButton(1, -1));
        Key02ArrowButton[1].onClick.AddListener(() => ArrowButton(1, 1));


        return base.Init();
    }

    private void Update()
    {
        Daily_Reset_Timer.text = Utils.NextDayTimer();
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }

    public void Get_Dungeon(int value)
    {
        if (Data_Manager.Main_Players_Data.Daily_Enter_Key[value] + Data_Manager.Main_Players_Data.User_Key_Assets[value] <= 0)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("입장할 수 있는 재화가 부족합니다.");
            return;
        }

        if (Stage_Manager.isDead)
        {           
            Base_Canvas.instance.Get_TOP_Popup().Initialize("훈련 중엔, 던전에 진입할 수 없습니다.");
            return;
        }
        if(Stage_Manager.M_State == Stage_State.Clear)
        {
            return;
        }

        if(Stage_Manager.isDungeon) // 던전 맵이 오픈되어 던전이 진행중 인지 확인
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("현재 던전공략이 진행 중 입니다.");
            return;
        }

        if(Stage_Manager.isDungeon_Map_Change == true)
        {
            return;
        }

        Stage_Manager.Dungeon_Level = Level[value];
 
        Base_Manager.Stage.State_Change(Stage_State.Dungeon, value);

       
        // 일일퀘스트 조건 상승
        if (value == 0)
        {
            Data_Manager.Main_Players_Data.Dungeon_Dia++;
            Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 모든 적을 소탕하세요!");
        }
        else
        {
            Data_Manager.Main_Players_Data.Dungeon_Gold++;
            Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 보스를 처치하세요!");
        }
    
        Utils.CloseAllPopupUI();
    }

    public void ArrowButton(int KeyValue, int value)
    {
        Level[KeyValue] += value;
        if (Level[KeyValue] <= 0)
        {
            Level[KeyValue] = 0;
        }
        else if (Level[KeyValue] > Data_Manager.Main_Players_Data.Dungeon_Clear_Level[KeyValue])
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("해당 난이도가 해금되지 않았습니다.");
            Level[KeyValue] = Data_Manager.Main_Players_Data.Dungeon_Clear_Level[KeyValue];
        }

        Dungeon_Levels[KeyValue].text = (Level[KeyValue] + 1).ToString();

        if(KeyValue == 0)
        {
            Clear_Assets[0].text = ((Level[KeyValue] + 1) * Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON).ToString();
        }

        else if(KeyValue == 1)
        {
            double Gold_Value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, (Level[KeyValue] + 1),
                Utils.Data.stageData.DROP_MONEY) * Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON;

            Clear_Assets[1].text = StringMethod.ToCurrencyString(Gold_Value);
        }

    }

}
