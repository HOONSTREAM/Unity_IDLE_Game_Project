using BackEnd.Functions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Player_Manager;

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
    private TextMeshProUGUI Tier_Bonus_Text;
    [SerializeField]
    private Image Clear_Tier_Image;
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Levels; // 난이도
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Level_Guide_Text;

    [SerializeField] 
    private Button[] Key01ArrowButton, Key02ArrowButton;

    private int[] Level = new int[2];
    

    public override bool Init()
    {

        Main_UI.Instance.FadeInOut(true, true, null);

        for(int i = 0; i < Data_Manager.Main_Players_Data.Dungeon_Clear_Level.Length; i++) // 최고난이도 적용
        {
            if (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] >= 99)
            {
                Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] = 99;
            }
        }
  
        for(int i = 0; i< KeyTexts.Length; i++)
        {
            KeyTexts[i].text = "(" + (Data_Manager.Main_Players_Data.Daily_Enter_Key[i] + Data_Manager.Main_Players_Data.User_Key_Assets[i]).ToString() + "/2)";
            Dungeon_Enter_Request_Key[i].color = (Data_Manager.Main_Players_Data.Daily_Enter_Key[i] + 
                Data_Manager.Main_Players_Data.User_Key_Assets[i]) <= 0 ? Color.red : Color.green;
            Dungeon_Levels[i].text = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] + 1).ToString();           
            Level[i] = Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i];

            Dungeon_Level_Guide_Text[i].text = i == 0
                ? $"스테이지 <color=#FFFF00>{((Level[i] + 1) * Utils.DIA_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도"
                : $"스테이지 <color=#FFFF00>{((Level[i] + 1) * Utils.GOLD_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도";
        }

        
        int levelCount = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1] + 1); 
        var value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, levelCount, Utils.Data.stageData.DROP_MONEY) * Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON;

        // 레벨디자인 필요
        Clear_Assets[0].text = ((Data_Manager.Main_Players_Data.Dungeon_Clear_Level[0] + 1) * Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON).ToString();
        Clear_Assets[1].text = StringMethod.ToCurrencyString(value);
        Clear_Assets[2].text = $"<color=##FFF00>{Utils.Set_Next_Tier_Name()}</color> 승급";

        Player_Tier currentTier = Data_Manager.Main_Players_Data.Player_Tier;

        double tierMultiplier = TierBonusTable.GetBonusMultiplier(currentTier);
      
        Tier_Bonus_Text.text = $"티어 보상 : 공격력 <color=#FFFF00>{tierMultiplier}</color>배 버프 적용 중";
       
        Player_Tier next_tier = Data_Manager.Main_Players_Data.Player_Tier + 1;
        if(next_tier >= Player_Tier.Tier_Challenger_10) { next_tier = Player_Tier.Tier_Challenger_10; }
        Clear_Tier_Image.sprite = Utils.Get_Atlas(next_tier.ToString());

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
        var playerData = Data_Manager.Main_Players_Data;
        

        bool isTierDungeon = value == 2;
        bool isNormalDungeon = value != 2;

        if (isTierDungeon && playerData.Player_Tier >= Player_Tier.Tier_Challenger_10)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("최고 티어에 도달하였습니다.");
            return;
        }

        if (isNormalDungeon && playerData.Daily_Enter_Key[value] + playerData.User_Key_Assets[value] <= 0)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("입장할 수 있는 재화가 부족합니다.");
            return;
        }

        if (Stage_Manager.isDead)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("훈련 중엔, 던전에 진입할 수 없습니다.");
            return;
        }

        if (Stage_Manager.M_State == Stage_State.Clear || Stage_Manager.isDungeon || Stage_Manager.isDungeon_Map_Change)
        {
            if (Stage_Manager.isDungeon)
            {
                Base_Canvas.instance.Get_TOP_Popup().Initialize("현재 던전공략이 진행 중 입니다.");
            }
            return;
        }

        if (isNormalDungeon)
        {
            Stage_Manager.Dungeon_Level = Level[value];
        }

        Base_Manager.Stage.State_Change(Stage_State.Dungeon, value);

        switch (value)
        {
            case 0:
                playerData.Dungeon_Dia++;
                Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 모든 적을 소탕하세요!");
                break;

            case 1:
                playerData.Dungeon_Gold++;
                Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 보스를 처치하세요!");
                break;

            case 2:
                Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 처치하고 승급하세요!");
                break;
        }

        Utils.CloseAllPopupUI();
    }
    public void ArrowButton(int KeyValue, int value)
    {
        Level[KeyValue] += value;

        
        if (Level[KeyValue] <= 0)
        {
            Level[KeyValue] = 0;
            Dungeon_Levels[KeyValue].text = (Level[KeyValue] + 1).ToString();
        }

        if (Level[KeyValue] >= 99)
        {
            Level[KeyValue] = 99;
            Dungeon_Levels[KeyValue].text = (Level[KeyValue] + 1).ToString();
            Base_Canvas.instance.Get_TOP_Popup().Initialize("최고 난이도에 도달하였습니다.");           
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

        Dungeon_Level_Guide_Text[KeyValue].text = KeyValue == 0
           ? $"스테이지 <color=#FFFF00>{((Level[KeyValue] + 1) * Utils.DIA_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도"
           : $"스테이지 <color=#FFFF00>{((Level[KeyValue] + 1) * Utils.GOLD_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도";
    }

}


