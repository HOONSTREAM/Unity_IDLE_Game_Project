using BackEnd.Functions;
using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Player_Manager;

public class UI_Dungeon : UI_Base
{

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
    private TextMeshProUGUI Now_User_DPS;
    [SerializeField]
    private TextMeshProUGUI Next_Goal_DPS;
    [SerializeField]
    private TextMeshProUGUI NOW_USER_DPS_LEVEL;
    [SerializeField]
    private TextMeshProUGUI DPS_REWARD_TEXT;
    [SerializeField]
    private Button[] Key01ArrowButton, Key02ArrowButton, Key03ArrowButton;
    [SerializeField]
    private TextMeshProUGUI User_Dia_Amount, User_Coin_Amount;

    private int[] Level = new int[3];
    

    public override bool Init()
    {

        Main_UI.Instance.FadeInOut(true, true, null);


        User_Dia_Amount.text = Data_Manager.Main_Players_Data.DiaMond.ToString();
        User_Coin_Amount.text = StringMethod.ToCurrencyString(Data_Manager.Main_Players_Data.Player_Money);

        for(int i = 0; i < Data_Manager.Main_Players_Data.Dungeon_Clear_Level.Length; i++) // 최고난이도 적용
        {
            if (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] >= Utils.DIA_AND_GOLD_DUNGEON_MAX_LEVEL)
            {
                Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] = Utils.DIA_AND_GOLD_DUNGEON_MAX_LEVEL;
            }
        }
  
        for(int i = 0; i< KeyTexts.Length; i++)
        {
            KeyTexts[i].text = "(" + (Data_Manager.Main_Players_Data.Daily_Enter_Key[i] + 
                Data_Manager.Main_Players_Data.User_Key_Assets[i]).ToString() + "/2)";            
            Dungeon_Levels[i].text = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] + 1).ToString();           
            Level[i] = Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i];

            Dungeon_Enter_Request_Key[i].color = (Data_Manager.Main_Players_Data.Daily_Enter_Key[i] +
                Data_Manager.Main_Players_Data.User_Key_Assets[i]) <= 0 ? Color.red : Color.green;

            if (i == 0)
                Dungeon_Level_Guide_Text[i].text = $"스테이지 <color=#FFFF00>{((Level[i] + 1) * Utils.DIA_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도";
            else if (i == 1)
                Dungeon_Level_Guide_Text[i].text = $"스테이지 <color=#FFFF00>{((Level[i] + 1) * Utils.GOLD_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도";
            else if (i == 2)
                Dungeon_Level_Guide_Text[i].text = $"스테이지 <color=#FFFF00>{((Level[i] + 1) * Utils.ENHANCEMENT_DUNGEON_FIRST_HARD)}</color>층 수준의 난이도";
        }

        for(int i = 0; i < 2; i++)
        {
            Dungeon_Enter_Request_Key[3].color = (Data_Manager.Main_Players_Data.Daily_Enter_Key[0] +
                Data_Manager.Main_Players_Data.User_Key_Assets[0]) <= 0 ? Color.red : Color.green;
            Dungeon_Enter_Request_Key[4].color = (Data_Manager.Main_Players_Data.Daily_Enter_Key[1] +
                Data_Manager.Main_Players_Data.User_Key_Assets[1]) <= 0 ? Color.red : Color.green;
            Dungeon_Enter_Request_Key[5].color = (Data_Manager.Main_Players_Data.Daily_Enter_Key[2] +
                Data_Manager.Main_Players_Data.User_Key_Assets[2]) <= 0 ? Color.red : Color.green;
        }

        
        int levelCount = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1] + 1); 
        var value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, levelCount, Utils.Data.stageData.DROP_MONEY) * 
            Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON;

        // 레벨디자인 필요
        Clear_Assets[0].text = ((Data_Manager.Main_Players_Data.Dungeon_Clear_Level[0] + 1) * 
            Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON).ToString();
        Clear_Assets[1].text = StringMethod.ToCurrencyString(value);
        Clear_Assets[2].text = $"{(int)(Data_Manager.Main_Players_Data.Dungeon_Clear_Level[2] / 3) + 3}";
        Clear_Assets[3].text = $"<color=##FFF00>{Utils.Set_Next_Tier_Name()}</color> 승급";

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

        Key03ArrowButton[0].onClick.AddListener(() => ArrowButton(2, -1));
        Key03ArrowButton[1].onClick.AddListener(() => ArrowButton(2, 1));


        DPS_Init();

        return base.Init();
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }
    private void DPS_Init()
    {

        foreach (var row in CSV_Importer.DPS_Design)
        {
            double requiredDPS = double.Parse(row["DPS_DMG"].ToString());

            if (Data_Manager.Main_Players_Data.USER_DPS < requiredDPS)
            {
                break;
            }

            int temp = Data_Manager.Main_Players_Data.USER_DPS_LEVEL;

            Data_Manager.Main_Players_Data.USER_DPS_LEVEL = int.Parse(row["DPS_LEVEL"].ToString());

            if (temp >= Data_Manager.Main_Players_Data.USER_DPS_LEVEL)
            {
                Data_Manager.Main_Players_Data.USER_DPS_LEVEL = temp;
            }

            if (Data_Manager.Main_Players_Data.USER_DPS_LEVEL >= Utils.DPS_DUNGEON_MAX_LEVEL)
            {
                Data_Manager.Main_Players_Data.USER_DPS_LEVEL = Utils.DPS_DUNGEON_MAX_LEVEL;
            }
        }

        if(Data_Manager.Main_Players_Data.USER_DPS_LEVEL < Utils.DPS_DUNGEON_MAX_LEVEL)
        {
            NOW_USER_DPS_LEVEL.text = $"<color=#FFFF00>{Data_Manager.Main_Players_Data.USER_DPS_LEVEL}</color>단계";
            Now_User_DPS.text = StringMethod.ToCurrencyString(Data_Manager.Main_Players_Data.USER_DPS);
            Next_Goal_DPS.text = StringMethod.ToCurrencyString(double.Parse(
                CSV_Importer.DPS_Design[Data_Manager.Main_Players_Data.USER_DPS_LEVEL + 1]
                ["DPS_DMG"].ToString()));

            var playerData = Data_Manager.Main_Players_Data;
            int currentLevel = playerData.USER_DPS_LEVEL;

            var claimedSet = new HashSet<int>();

            if (!string.IsNullOrEmpty(playerData.DPS_REWARD))
            {
                string[] tokens = playerData.DPS_REWARD.Split(',');
                foreach (var t in tokens)
                {
                    if (int.TryParse(t, out int claimed))
                        claimedSet.Add(claimed);
                }
            }

            int totalReward = 0;
            List<int> newlyClaimed = new List<int>();

            foreach (var row in CSV_Importer.DPS_REWARD_Design)
            {
                int level = int.Parse(row["DPS_LEVEL"].ToString());
                if (level > currentLevel) break;
                if (claimedSet.Contains(level)) continue;

                // 다이아 보상 값 읽기
                int reward = 0;
                if (int.TryParse(row["DIAMOND"].ToString(), out reward))
                {
                    totalReward += reward;
                    newlyClaimed.Add(level);
                }
            }

            if (newlyClaimed.Count == 0)
            {
                DPS_REWARD_TEXT.text = "0";
            }

            DPS_REWARD_TEXT.text = totalReward.ToString();
        }

        else
        {
            NOW_USER_DPS_LEVEL.text = "최고단계";
            DPS_REWARD_TEXT.text = "0";
            Now_User_DPS.text = StringMethod.ToCurrencyString(Data_Manager.Main_Players_Data.USER_DPS);
            Next_Goal_DPS.text = "최고 전투력 달성";
        }
        
    }
    public void Get_Dungeon(int value)
    {
        if(value == 2)
        {
            if(Data_Manager.Main_Players_Data.Player_Max_Stage < Utils.ENHANCEMENT_DUNGEON_FIRST_HARD)
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize($"{Utils.ENHANCEMENT_DUNGEON_FIRST_HARD}층 이상부터 진입가능합니다.");
                return;
            }
        }

        if(value == 4) // DPS
        {
            if(Data_Manager.Main_Players_Data.USER_DPS_LEVEL >= Utils.DPS_DUNGEON_MAX_LEVEL)
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("최고 단계에 달성하셨습니다.");
                return;
            }
        }

        var playerData = Data_Manager.Main_Players_Data;
        

        bool isTierDungeon = value == 3;
        bool isNormalDungeon = value != 3;
            
        if(value != 4)
        {
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

            if (isNormalDungeon)
            {
                Stage_Manager.Dungeon_Level = Level[value];
            }
        }
      

        if (Stage_Manager.isDead)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("방치모드 중엔, 던전에 진입할 수 없습니다.");
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
                Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 보스를 처치하세요!");
                break;
            case 3:              
                Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 처치하고 승급하세요!");
                break;
            case 4:                                 
                Base_Canvas.instance.Get_TOP_Popup().Initialize("제한시간 안에 최대한 높은 데미지를 가하세요!");
                break;
        }

        Utils.CloseAllPopupUI();
    }

    /// <summary>
    /// 클리어한 최고난이도로, 던전을 소탕처리합니다.
    /// </summary>
    /// <param name="value"></param>
    public void Dungeon_Sweep_Button(int value)
    {
        var playerData = Data_Manager.Main_Players_Data;
        var Dungeon_Clear_Level = playerData.Dungeon_Clear_Level[value];

        if (playerData.Daily_Enter_Key[value] + playerData.User_Key_Assets[value] <= 0)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("소탕 할 재화가 부족합니다.");
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

        switch (value)
        {
            case 0:

                if (Dungeon_Clear_Level== 0)
                {
                    Base_Canvas.instance.Get_TOP_Popup().Initialize("클리어 한 난이도가 없습니다.");
                    return;
                }

                if (Data_Manager.Main_Players_Data.User_Key_Assets[value] > 0)
                {
                    Data_Manager.Main_Players_Data.User_Key_Assets[value]--;
                }
                else
                {
                    Data_Manager.Main_Players_Data.Daily_Enter_Key[value]--;
                }


                playerData.Dungeon_Dia++;

                Data_Manager.Main_Players_Data.DiaMond += ((Dungeon_Clear_Level + 1) * Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON);
                Base_Manager.BACKEND.Log_Get_Dia("Dia_Dungeon_Sweep");
                _ = Base_Manager.BACKEND.WriteData();

                Base_Canvas.instance.Get_TOP_Popup().Initialize("보물창고 소탕이 완료되었습니다 !");
                Init();

                Base_Manager.SOUND.Play(Sound.BGS, "Victory");

                break;

            case 1:

                if (Dungeon_Clear_Level == 0)
                {
                    Base_Canvas.instance.Get_TOP_Popup().Initialize("클리어 한 난이도가 없습니다.");
                    return;
                }

                playerData.Dungeon_Gold++;


                if (Data_Manager.Main_Players_Data.User_Key_Assets[value] > 0)
                {
                    Data_Manager.Main_Players_Data.User_Key_Assets[value]--;
                }
                else
                {
                    Data_Manager.Main_Players_Data.Daily_Enter_Key[value]--;
                }


                var Gold_Value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, 
                    (Dungeon_Clear_Level + 1), Utils.Data.stageData.DROP_MONEY) * Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON;

                Data_Manager.Main_Players_Data.Player_Money += Gold_Value;

                _ = Base_Manager.BACKEND.WriteData();

                Base_Canvas.instance.Get_TOP_Popup().Initialize("골드창고 소탕이 완료되었습니다 !");
                Init();
                Base_Manager.SOUND.Play(Sound.BGS, "Victory");

                break;

            case 2:

                if (Dungeon_Clear_Level == 0)
                {
                    Base_Canvas.instance.Get_TOP_Popup().Initialize("클리어 한 난이도가 없습니다.");
                    return;
                }             

                if (Data_Manager.Main_Players_Data.User_Key_Assets[value] > 0)
                {
                    Data_Manager.Main_Players_Data.User_Key_Assets[value]--;
                }
                else
                {
                    Data_Manager.Main_Players_Data.Daily_Enter_Key[value]--;
                }
                
                int Bonus = (int)Dungeon_Clear_Level / 3;
                
                if(Bonus >= 25) // 보너스는 최대 25개
                {
                    Bonus = 25;
                }

                if(Dungeon_Clear_Level >= 199)
                {
                    Bonus = 30;
                }

                Base_Manager.Data.Item_Holder["Enhancement"].Hero_Card_Amount += (3 + Bonus);

                _ = Base_Manager.BACKEND.WriteData();

                Base_Canvas.instance.Get_TOP_Popup().Initialize("미스릴던전 소탕이 완료되었습니다 !");
                Init();
                Base_Manager.SOUND.Play(Sound.BGS, "Victory");

                break;


        }
        

    }
    public void ArrowButton(int KeyValue, int value)
    {
        Level[KeyValue] += value;


        if (Level[KeyValue] <= 0)
        {
            Level[KeyValue] = 0;
            Dungeon_Levels[KeyValue].text = (Level[KeyValue] + 1).ToString();
        }

        if (Level[KeyValue] >= Utils.DIA_AND_GOLD_DUNGEON_MAX_LEVEL)
        {
            Level[KeyValue] = Utils.DIA_AND_GOLD_DUNGEON_MAX_LEVEL;
            Dungeon_Levels[KeyValue].text = (Level[KeyValue] + 1).ToString();
            Base_Canvas.instance.Get_TOP_Popup().Initialize("최고 난이도에 도달하였습니다.");
        }



        else if (Level[KeyValue] > Data_Manager.Main_Players_Data.Dungeon_Clear_Level[KeyValue])
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("해당 난이도가 해금되지 않았습니다.");
            Level[KeyValue] = Data_Manager.Main_Players_Data.Dungeon_Clear_Level[KeyValue];
        }

        Dungeon_Levels[KeyValue].text = (Level[KeyValue] + 1).ToString();

        if (KeyValue == 0)
        {
            Clear_Assets[0].text = ((Level[KeyValue] + 1) * Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON).ToString();
        }

        else if (KeyValue == 1)
        {
            double Gold_Value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, (Level[KeyValue] + 1),
                Utils.Data.stageData.DROP_MONEY) * Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON;

            Clear_Assets[1].text = StringMethod.ToCurrencyString(Gold_Value);
        }

        else if (KeyValue == 2)
        {
            int Bonus = (Level[KeyValue]) / 3;

            Clear_Assets[2].text = $"{3 + Bonus}";
        }

        if (KeyValue == 0)
        {
            Dungeon_Level_Guide_Text[KeyValue].text =
                $"스테이지 <color=#FFFF00>{((Level[KeyValue] + 1) * Utils.DIA_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도";
        }
        else if (KeyValue == 1)
        {
            Dungeon_Level_Guide_Text[KeyValue].text =
                $"스테이지 <color=#FFFF00>{((Level[KeyValue] + 1) * Utils.GOLD_DUNGEON_MULTIPLE_HARD)}</color>층 수준의 난이도";
        }
        else if (KeyValue == 2)
        {
            Dungeon_Level_Guide_Text[KeyValue].text =
                $"스테이지 <color=#FFFF00>{((Level[KeyValue] + 1) * Utils.ENHANCEMENT_DUNGEON_FIRST_HARD)}</color>층 수준의 난이도";
        }
    }
    /// <summary>
    /// DPS던전의 보상단계를 체크하고, 보상합니다.
    /// </summary>
    public void Get_All_DPS_Rewards()
    {
        var playerData = Data_Manager.Main_Players_Data;
        int currentLevel = playerData.USER_DPS_LEVEL;

        if(playerData.USER_DPS_LEVEL >= Utils.DPS_DUNGEON_MAX_LEVEL)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("최고 전투력에 달성하였습니다.");
            return;
        }

        // 수령 이력 파싱
        var claimedSet = new HashSet<int>();
        if (!string.IsNullOrEmpty(playerData.DPS_REWARD))
        {
            string[] tokens = playerData.DPS_REWARD.Split(',');
            foreach (var t in tokens)
            {
                if (int.TryParse(t, out int claimed))
                    claimedSet.Add(claimed);
            }
        }

        int totalReward = 0;
        List<int> newlyClaimed = new List<int>();

        foreach (var row in CSV_Importer.DPS_REWARD_Design)
        {
            int level = int.Parse(row["DPS_LEVEL"].ToString());
            if (level > currentLevel) break;
            if (claimedSet.Contains(level)) continue;

            // 다이아 보상 값 읽기
            int reward = 0;
            if (int.TryParse(row["DIAMOND"].ToString(), out reward))
            {
                totalReward += reward;
                newlyClaimed.Add(level);
            }
        }

        if (newlyClaimed.Count == 0)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("수령할 수 있는 보상이 없습니다.");
            return;
        }

        // 다이아 지급
        playerData.DiaMond += totalReward;

        // 이력 업데이트
        claimedSet.UnionWith(newlyClaimed);
        playerData.DPS_REWARD = string.Join(",", claimedSet.OrderBy(x => x));

        // 서버 저장

        Base_Manager.BACKEND.Log_Get_Dia("DPS_Dungeon");
        _ = Base_Manager.BACKEND.WriteData();

        // 알림 및 효과
        Base_Canvas.instance.Get_TOP_Popup().Initialize($"총 {newlyClaimed.Count}개의 구간 보상 수령 완료! (+ <color=#FFFF00>{totalReward}</color> 다이아)");
        Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        Init();
    }

}


