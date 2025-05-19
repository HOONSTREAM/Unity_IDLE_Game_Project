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
    private TextMeshProUGUI[] Clear_Assets; // Ŭ���� ����
    [SerializeField]
    private TextMeshProUGUI Tier_Bonus_Text;
    [SerializeField]
    private Image Clear_Tier_Image;
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Levels; // ���̵�
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Level_Guide_Text;

    [SerializeField] 
    private Button[] Key01ArrowButton, Key02ArrowButton;

    private int[] Level = new int[2];
    

    public override bool Init()
    {

        Main_UI.Instance.FadeInOut(true, true, null);

        for(int i = 0; i < Data_Manager.Main_Players_Data.Dungeon_Clear_Level.Length; i++) // �ְ��̵� ����
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
                ? $"�������� <color=#FFFF00>{((Level[i] + 1) * Utils.DIA_DUNGEON_MULTIPLE_HARD)}</color>�� ������ ���̵�"
                : $"�������� <color=#FFFF00>{((Level[i] + 1) * Utils.GOLD_DUNGEON_MULTIPLE_HARD)}</color>�� ������ ���̵�";
        }

        
        int levelCount = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1] + 1); 
        var value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, levelCount, Utils.Data.stageData.DROP_MONEY) * Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON;

        // ���������� �ʿ�
        Clear_Assets[0].text = ((Data_Manager.Main_Players_Data.Dungeon_Clear_Level[0] + 1) * Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON).ToString();
        Clear_Assets[1].text = StringMethod.ToCurrencyString(value);
        Clear_Assets[2].text = $"<color=##FFF00>{Utils.Set_Next_Tier_Name()}</color> �±�";

        Player_Tier currentTier = Data_Manager.Main_Players_Data.Player_Tier;

        double tierMultiplier = TierBonusTable.GetBonusMultiplier(currentTier);
      
        Tier_Bonus_Text.text = $"Ƽ�� ���� : ���ݷ� <color=#FFFF00>{tierMultiplier}</color>�� ���� ���� ��";
       
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
            Base_Canvas.instance.Get_TOP_Popup().Initialize("�ְ� Ƽ� �����Ͽ����ϴ�.");
            return;
        }

        if (isNormalDungeon && playerData.Daily_Enter_Key[value] + playerData.User_Key_Assets[value] <= 0)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("������ �� �ִ� ��ȭ�� �����մϴ�.");
            return;
        }

        if (Stage_Manager.isDead)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("�Ʒ� �߿�, ������ ������ �� �����ϴ�.");
            return;
        }

        if (Stage_Manager.M_State == Stage_State.Clear || Stage_Manager.isDungeon || Stage_Manager.isDungeon_Map_Change)
        {
            if (Stage_Manager.isDungeon)
            {
                Base_Canvas.instance.Get_TOP_Popup().Initialize("���� ���������� ���� �� �Դϴ�.");
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
                Base_Canvas.instance.Get_TOP_Popup().Initialize("���ѽð� �ȿ� ��� ���� �����ϼ���!");
                break;

            case 1:
                playerData.Dungeon_Gold++;
                Base_Canvas.instance.Get_TOP_Popup().Initialize("���ѽð� �ȿ� ������ óġ�ϼ���!");
                break;

            case 2:
                Base_Canvas.instance.Get_TOP_Popup().Initialize("���ѽð� �ȿ� óġ�ϰ� �±��ϼ���!");
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
            Base_Canvas.instance.Get_TOP_Popup().Initialize("�ְ� ���̵��� �����Ͽ����ϴ�.");           
        }

        

        else if (Level[KeyValue] > Data_Manager.Main_Players_Data.Dungeon_Clear_Level[KeyValue])
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("�ش� ���̵��� �رݵ��� �ʾҽ��ϴ�.");
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
           ? $"�������� <color=#FFFF00>{((Level[KeyValue] + 1) * Utils.DIA_DUNGEON_MULTIPLE_HARD)}</color>�� ������ ���̵�"
           : $"�������� <color=#FFFF00>{((Level[KeyValue] + 1) * Utils.GOLD_DUNGEON_MULTIPLE_HARD)}</color>�� ������ ���̵�";
    }

}


