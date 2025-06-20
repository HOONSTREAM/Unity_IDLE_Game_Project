using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 서버에서 관리 할 DB를 저장하는 용도로 사용하는 매니저 입니다.
/// </summary>
/// 

#region BackEnd
public class BackendGameData
{
    private static BackendGameData _instance = null;

    public static BackendGameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendGameData();
            }

            return _instance;
        }
    }

    /// <summary>
    /// 처음 회원가입을 하면, 유저 데이터를 초기화 합니다.
    /// </summary>
    public void Initialize_User_Data()
    {
        // Step 1. 게임 재화 초기화하기

        if (Data_Manager.Main_Players_Data == null)
        {
            Data_Manager.Main_Players_Data = new Data();
        }

        Data_Manager.Main_Players_Data.ATK = default;
        Data_Manager.Main_Players_Data.HP = default;
        Data_Manager.Main_Players_Data.Player_Tier = Player_Tier.Tier_Beginner;
        Data_Manager.Main_Players_Data.Player_Money = default;
        Data_Manager.Main_Players_Data.DiaMond = default;
        Data_Manager.Main_Players_Data.Player_Level = default;
        Data_Manager.Main_Players_Data.Last_Daily_Reset_Time = Utils.Get_Server_Time().ToString("yyyy-MM-dd");
        Data_Manager.Main_Players_Data.EXP = default;
        Data_Manager.Main_Players_Data.Player_Stage = 1;
        Data_Manager.Main_Players_Data.Player_Max_Stage = 1;
        Data_Manager.Main_Players_Data.EXP_Upgrade_Count = 0;
        Data_Manager.Main_Players_Data.Buff_Timers[0] = 0;
        Data_Manager.Main_Players_Data.Buff_Timers[1] = 0;
        Data_Manager.Main_Players_Data.Buff_Timers[2] = 0;
        Data_Manager.Main_Players_Data.ADS_Timer[0] = 0;
        Data_Manager.Main_Players_Data.ADS_Timer[1] = 0;
        Data_Manager.Main_Players_Data.buff_x2_speed = 0;       
        Data_Manager.Main_Players_Data.Quest_Count = default;
        Data_Manager.Main_Players_Data.Hero_Summon_Count = default;
        Data_Manager.Main_Players_Data.Hero_Pickup_Count = default;
        Data_Manager.Main_Players_Data.Relic_Pickup_Count = default;
        Data_Manager.Main_Players_Data.Relic_Summon_Count = default;
        Data_Manager.Main_Players_Data.StartDate = Utils.Get_Server_Time();
        Data_Manager.Main_Players_Data.EndDate = Utils.Get_Server_Time();
        Data_Manager.Main_Players_Data.Daily_Enter_Key[0] = 3;
        Data_Manager.Main_Players_Data.Daily_Enter_Key[1] = 3;
        Data_Manager.Main_Players_Data.Daily_Enter_Key[2] = 3;
        Data_Manager.Main_Players_Data.User_Key_Assets[0] = 0;
        Data_Manager.Main_Players_Data.User_Key_Assets[1] = 0;
        Data_Manager.Main_Players_Data.User_Key_Assets[2] = 0;
        Data_Manager.Main_Players_Data.Dungeon_Clear_Level[0] = 0;
        Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1] = 0;
        Data_Manager.Main_Players_Data.Dungeon_Clear_Level[2] = 0;
        Data_Manager.Main_Players_Data.isBuyADPackage = false;
        Data_Manager.Main_Players_Data.isBuyTodayPackage = false;
        Data_Manager.Main_Players_Data.isBuySTRONGPackage = false;
        Data_Manager.Main_Players_Data.isBuySTARTPackage = false;
        Data_Manager.Main_Players_Data.isBuyDIAMONDPackage = false;
        Data_Manager.Main_Players_Data.Event_Push_Alarm_Agree = false;
        Data_Manager.Main_Players_Data.Daily_Attendance = 1;
        Data_Manager.Main_Players_Data.Levelup = 0;
        Data_Manager.Main_Players_Data.Summon = 0;
        Data_Manager.Main_Players_Data.Relic = 0;
        Data_Manager.Main_Players_Data.Dungeon_Gold = 0;
        Data_Manager.Main_Players_Data.Dungeon_Dia = 0;
        Data_Manager.Main_Players_Data.Daily_Attendance_Clear = false;
        Data_Manager.Main_Players_Data.Level_up_Clear = false;
        Data_Manager.Main_Players_Data.Summon_Clear = false;
        Data_Manager.Main_Players_Data.Relic_Clear = false;
        Data_Manager.Main_Players_Data.Dungeon_Gold_Clear = false;
        Data_Manager.Main_Players_Data.Dungeon_Dia_Clear = false;
        Data_Manager.Main_Players_Data.ADS_Hero_Summon_Count = 0;
        Data_Manager.Main_Players_Data.ADS_Relic_Summon_Count = 0;
        Data_Manager.Main_Players_Data.Season = 0;
        Data_Manager.Main_Players_Data.USER_DPS = 0;
        Data_Manager.Main_Players_Data.USER_DPS_LEVEL = 0;
        Data_Manager.Main_Players_Data.DPS_REWARD = default;
        Data_Manager.Main_Players_Data.ADS_FREE_DIA = false;
        Data_Manager.Main_Players_Data.ADS_FREE_STEEL = false;
        Data_Manager.Main_Players_Data.FREE_DIA = false;
        Data_Manager.Main_Players_Data.FREE_COMB_SCROLL = false;
        Data_Manager.Main_Players_Data.DIA_GACHA_COUNT = 0;

        Data_Manager.Main_Players_Data.Attendance_Day = 0;
        Data_Manager.Main_Players_Data.Get_Attendance_Reward = false;
        Data_Manager.Main_Players_Data.Attendance_Last_Date = default;

        Data_Manager.Main_Players_Data.isBUY_DIA_PASS = false;
        Data_Manager.Main_Players_Data.DIA_PASS_ATTENDANCE_DAY = 0;
        Data_Manager.Main_Players_Data.Get_DIA_PASS_Reward = false;
        Data_Manager.Main_Players_Data.DIA_PASS_Last_Date = default;

        Param param = new Param();

        param.Add("SPEED", Data_Manager.Main_Players_Data.buff_x2_speed);
        param.Add("ATK", Data_Manager.Main_Players_Data.ATK);
        param.Add("HP", Data_Manager.Main_Players_Data.HP);
        param.Add("PLAYER_TIER", (int)Data_Manager.Main_Players_Data.Player_Tier);
        param.Add("PLAYER_MONEY", Data_Manager.Main_Players_Data.Player_Money);
        param.Add("DIAMOND", Data_Manager.Main_Players_Data.DiaMond);
        param.Add("PLAYER_LEVEL", Data_Manager.Main_Players_Data.Player_Level);
        param.Add("PLAYER_EXP", Data_Manager.Main_Players_Data.EXP);
        param.Add("PLAYER_STAGE", Data_Manager.Main_Players_Data.Player_Stage);
        param.Add("PLAYER_HIGH_STAGE", Data_Manager.Main_Players_Data.Player_Max_Stage);
        param.Add("EXP_UPGRADE_COUNT", Data_Manager.Main_Players_Data.EXP_Upgrade_Count);
        param.Add("BUFF_TIMER", Data_Manager.Main_Players_Data.Buff_Timers);            
        param.Add("QUEST_COUNT", Data_Manager.Main_Players_Data.Quest_Count);
        param.Add("HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.Hero_Summon_Count);
        param.Add("HERO_PICKUP_COUNT", Data_Manager.Main_Players_Data.Hero_Pickup_Count);
        param.Add("RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.Relic_Summon_Count);
        param.Add("RELIC_PICKUP_COUNT", Data_Manager.Main_Players_Data.Relic_Pickup_Count);
        param.Add("START_DATE", Data_Manager.Main_Players_Data.StartDate);
        param.Add("END_DATE", Data_Manager.Main_Players_Data.EndDate);
        param.Add("DAILY_ENTER_KEY", Data_Manager.Main_Players_Data.Daily_Enter_Key);
        param.Add("USER_KEY_ASSETS", Data_Manager.Main_Players_Data.User_Key_Assets);
        param.Add("DUNGEON_CLEAR_LEVEL", Data_Manager.Main_Players_Data.Dungeon_Clear_Level);
        param.Add("isBUY_AD_Package", Data_Manager.Main_Players_Data.isBuyADPackage);
        param.Add("isBUY_LAUNCH_EVENT", Data_Manager.Main_Players_Data.isBuyLAUNCH_EVENT);
        param.Add("isBUY_TODAY_Package", Data_Manager.Main_Players_Data.isBuyTodayPackage);
        param.Add("isBUY_STRONG_Package", Data_Manager.Main_Players_Data.isBuySTRONGPackage);
        param.Add("isBUY_START_Package", Data_Manager.Main_Players_Data.isBuySTARTPackage);
        param.Add("isBUY_DIAMOND_Package", Data_Manager.Main_Players_Data.isBuyDIAMONDPackage);
        param.Add("EVENT_PUSH_ALARM", Data_Manager.Main_Players_Data.Event_Push_Alarm_Agree);
        param.Add("Daily_Attendance", Data_Manager.Main_Players_Data.Daily_Attendance);
        param.Add("Level_up_Daily_Quest_Count", Data_Manager.Main_Players_Data.Levelup);
        param.Add("Summon_Daily_Quest_Count", Data_Manager.Main_Players_Data.Summon);
        param.Add("Relic_Daily_Quest_Count", Data_Manager.Main_Players_Data.Relic);
        param.Add("Dungeon_Gold_Clear_Count", Data_Manager.Main_Players_Data.Dungeon_Gold);
        param.Add("Dungeon_Dia_Clear_Count", Data_Manager.Main_Players_Data.Dungeon_Dia);
        param.Add("Daily_Attendance_Clear", Data_Manager.Main_Players_Data.Daily_Attendance_Clear);
        param.Add("Level_Up_Clear", Data_Manager.Main_Players_Data.Level_up_Clear);
        param.Add("Summon_Count_Clear", Data_Manager.Main_Players_Data.Summon_Clear);
        param.Add("Relic_Clear", Data_Manager.Main_Players_Data.Relic_Clear);
        param.Add("Dungeon_Gold_Clear", Data_Manager.Main_Players_Data.Dungeon_Gold_Clear);
        param.Add("Dungeon_Dia_Clear", Data_Manager.Main_Players_Data.Dungeon_Dia_Clear);
        param.Add("ADS_HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.ADS_Hero_Summon_Count);
        param.Add("ADS_RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.ADS_Relic_Summon_Count);
        param.Add("Fast_Mode", Data_Manager.Main_Players_Data.isFastMode);
        param.Add("SEASON", Data_Manager.Main_Players_Data.Season);
        param.Add("LAST_DAILY_RESET", Data_Manager.Main_Players_Data.Last_Daily_Reset_Time);
        param.Add("USER_DPS", Data_Manager.Main_Players_Data.USER_DPS);
        param.Add("USER_DPS_LEVEL", Data_Manager.Main_Players_Data.USER_DPS_LEVEL);
        param.Add("USER_DPS_REWARD", Data_Manager.Main_Players_Data.DPS_REWARD);
        param.Add("ADS_FREE_DIA", Data_Manager.Main_Players_Data.ADS_FREE_DIA);
        param.Add("ADS_FREE_STEEL", Data_Manager.Main_Players_Data.ADS_FREE_STEEL);
        param.Add("FREE_DIA", Data_Manager.Main_Players_Data.FREE_DIA);
        param.Add("FREE_COMB_SCROLL", Data_Manager.Main_Players_Data.FREE_COMB_SCROLL);
        param.Add("DIA_GACHA_COUNT", Data_Manager.Main_Players_Data.DIA_GACHA_COUNT);

        // 출석 데이터
        param.Add("Attendance_Day", Data_Manager.Main_Players_Data.Attendance_Day);
        param.Add("Get_Attendance_Reward", Data_Manager.Main_Players_Data.Get_Attendance_Reward);
        param.Add("Attendance_Date", Data_Manager.Main_Players_Data.Attendance_Last_Date);

        // 다이아 패스 데이터
        param.Add("is_BUY_DIA_PASS", Data_Manager.Main_Players_Data.isBUY_DIA_PASS);
        param.Add("DIA_ATTENDANCE_DAY", Data_Manager.Main_Players_Data.DIA_PASS_ATTENDANCE_DAY);
        param.Add("Get_DIA_PASS_REWARD", Data_Manager.Main_Players_Data.Get_DIA_PASS_Reward);
        param.Add("DIA_PASS_ATTENDANCE_DATE", Data_Manager.Main_Players_Data.DIA_PASS_Last_Date);


        var bro = Backend.GameData.Insert("USER", param);
        string inDate = bro.GetInDate();
        var _bro = Backend.URank.User.UpdateUserScore(Utils.LEADERBOARD_UUID, "USER", inDate, param); // 리더보드에 유저데이터를 등록합니다.
       
        if (bro.IsSuccess())
        {
            Debug.Log("유저 기본 데이터를 추가하는데 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("유저 기본 데이터를 추가하는데 실패했습니다. : " + bro);
        }

        if (_bro.IsSuccess())
        {
            Debug.Log("유저 기본 데이터를 리더보드에 추가하는데 성공했습니다. : " + _bro);
        }
        else
        {
            Debug.LogError("유저 기본 데이터를 리더보드에 추가하는데 실패했습니다. : " + _bro);
          
        }

        Param character_param = new Param();

        character_param.Add("character", Base_Manager.Data.character_Holder);
       
        var char_bro = Backend.GameData.Insert("CHARACTER",character_param);

        if (char_bro.IsSuccess())
        {
            Debug.Log("영웅 보유 데이터를 추가하는데 성공했습니다. : " + char_bro);
        }
        else
        {
            Debug.LogError("영웅 보유 데이터를 추가하는데 실패했습니다. : " + char_bro);
        }

        Param item_param = new Param();

        item_param.Add("Item", Base_Manager.Data.Item_Holder);

    
        var item_bro = Backend.GameData.Insert("ITEM", item_param);

        if (item_bro.IsSuccess())
        {
            Debug.Log("인벤토리 데이터를 추가하는데 성공했습니다. : " + item_bro);
        }
        else
        {
            Debug.LogError("인벤토리 데이터를 추가하는데 실패했습니다. : " + item_bro);
        }

        Param Status_item_param = new Param();

        Status_item_param.Add("status_Item", Base_Manager.Data.Status_Item_Holder);


        var Status_item_bro = Backend.GameData.Insert("STATUS_ITEM", Status_item_param);

        if (Status_item_bro.IsSuccess())
        {
            Debug.Log("성장장비 데이터를 추가하는데 성공했습니다. : " + Status_item_bro);
        }
        else
        {
            Debug.LogError("성장장비 데이터를 추가하는데 실패했습니다. : " + Status_item_bro);
        }

        Param smelt_param = new Param();

        smelt_param.Add("Smelt", Base_Manager.Data.User_Main_Data_Smelt_Array);
      
        var smelt_bro = Backend.GameData.Insert("SMELT", smelt_param);

        if (smelt_bro.IsSuccess())
        {
            Debug.Log("유저 영웅 각인 데이터를 추가하는데 성공했습니다. : " + smelt_bro);
        }
        else
        {
            Debug.LogError("유저 영웅 각인 데이터를 추가하는데 실패했습니다. : " + smelt_bro);
        }

        Param player_set_Hero_Param = new Param();

        player_set_Hero_Param.Add("Player_Set_Hero", Base_Manager.Character.Set_Character);
      
        var player_set_hero_bro = Backend.GameData.Insert("PLAYER_SET_HERO", player_set_Hero_Param);

        if (player_set_hero_bro.IsSuccess())
        {
            Debug.Log("유저 영웅 배치 데이터를 추가하는데 성공했습니다. : " + player_set_hero_bro);
        }
        else
        {
            Debug.LogError("유저 영웅 배치 데이터를 추가하는데 실패했습니다. : " + player_set_hero_bro);
        }

        Param player_Set_Relic = new Param();

        player_Set_Relic.Add("Player_Set_Relic", Base_Manager.Data.Main_Set_Item);
       
        var player_set_relic_bro = Backend.GameData.Insert("PLAYER_SET_RELIC", player_Set_Relic);

        if (player_set_relic_bro.IsSuccess())
        {
            Debug.Log("유저 유물 배치 데이터를 추가하는데 성공했습니다. : " + player_set_relic_bro);
        }
        else
        {
            Debug.LogError("유저 유물 배치 데이터를 추가하는데 실패했습니다. : " + player_set_relic_bro);
        }

    }
}
    #endregion

    [System.Serializable]
public class Percentage_Smelt
{
    public Rarity Rarity;
    public float Min;
    public float Max;
}
public class Character_Holder
{
    public Character_Scriptable Data;
    public Holder holder;
}

public class Smelt_Holder
{
    public Rarity rarity;
    public Smelt_Status smelt_holder;
    public float smelt_value;
}

public class Item_Holder
{
    public Item_Scriptable Data;
    public Holder holder;
}

public class Status_Holder
{
    public Status_Item_Scriptable Data;
    public Status_Item_Holder holder;
}

public class Status_Item_Holder
{    
    public int Enhancement;    
    public double Additional_ATK;
    public double Additional_HP;
    public double Additional_STR;
    public double Additional_DEX;
    public double Additional_VIT;
    public double Item_Level;
    public int Item_Amount;
}


public class Holder
{
    public int Hero_Level;
    public int Hero_Card_Amount;
}

public class Data
{
   
    public double ATK;
    public double HP;
    public double Player_Money;
    public Player_Tier Player_Tier;
    public int DiaMond;
    public int Player_Level;
    public double EXP;
    public int Player_Stage = 1;
    public int Player_Max_Stage = 1;
    public int EXP_Upgrade_Count;
    public float[] Buff_Timers = { 0.0f, 0.0f, 0.0f };
    public float buff_x2_speed;   
    public int Quest_Count;
    public int Season = 0; // 랭크시즌
    public double USER_DPS = 0;
    public int USER_DPS_LEVEL = 0;
    public string DPS_REWARD = default;
    
    /// <summary>
    /// 플레이어 유저의 소환 레벨 변수
    /// </summary>
    public int Hero_Summon_Count;
    /// <summary>
    /// 플레이어 유저의 확정 소환 카운트
    /// </summary>
    public int Hero_Pickup_Count;

    /// <summary>
    /// 플레이어 유저의 유물 소환 레벨 변수
    /// </summary>
    public int Relic_Summon_Count;
    /// <summary>
    /// 플레이어 유저의 유물 확정 소환 카운트
    /// </summary>
    public int Relic_Pickup_Count;

    /// <summary>
    /// 플레이어 유저의 게임 시작시간, 종료시간 기록
    /// </summary>
    public DateTime StartDate;
    public DateTime EndDate;

    public string Last_Daily_Reset_Time;

    //Dungeon
    public int[] Daily_Enter_Key = { 3, 3, 3 }; // 일일마다 초기화 되는 키
    public int[] User_Key_Assets = { 0, 0, 0 }; // 유저가 보상으로 얻은 키 
    public int[] Dungeon_Clear_Level = { 0, 0, 0 }; //유저가 최종적으로 클리어한 난이도

    //광고구매 여부
    public bool isBuyADPackage = false;
    public bool isBuyLAUNCH_EVENT = false;
    public bool isBuyTodayPackage = false;
    public bool isBuySTRONGPackage = false;
    public bool isBuySTARTPackage = false;
    public bool isBuyDIAMONDPackage = false;
    public int ADS_Hero_Summon_Count = 0;

    public bool ADS_FREE_DIA = false;
    public bool ADS_FREE_STEEL = false;
    public bool FREE_DIA = false;
    public bool FREE_COMB_SCROLL = false;
    public int DIA_GACHA_COUNT = 0;

    public int ADS_Relic_Summon_Count = 0;
    public float[] ADS_Timer = { 0.0f, 0.0f };

    //이벤트 푸시알람 동의 여부
    public bool Event_Push_Alarm_Agree = false;

    //일일퀘스트

    public int Daily_Attendance = 1;
    public int Levelup;
    public int Summon;
    public int Relic;
    public int Dungeon_Gold;
    public int Dungeon_Dia;

    public bool Daily_Attendance_Clear = false;
    public bool Level_up_Clear = false;
    public bool Summon_Clear = false;
    public bool Relic_Clear = false;
    public bool Dungeon_Gold_Clear = false;
    public bool Dungeon_Dia_Clear = false;
    public bool isFastMode = false;

    //출석
    public int Attendance_Day = 0;
    public bool Get_Attendance_Reward = false;
    public string Attendance_Last_Date = default;

    //다이아 패스 출석
    public bool isBUY_DIA_PASS = false;
    public int DIA_PASS_ATTENDANCE_DAY = 0;
    public bool Get_DIA_PASS_Reward = false;
    public string DIA_PASS_Last_Date = default;

}

public class Data_Manager
{

    public static Data Main_Players_Data = new Data();
    /// <summary>
    /// 플레이어가 현재 소유중인 영웅들을 관리합니다.
    /// </summary>
    public Dictionary<string, Character_Holder> Data_Character_Dictionary = new Dictionary<string, Character_Holder>();
    public Dictionary<string, Holder> Item_Holder = new Dictionary<string, Holder>();
    public Dictionary<string, Holder> character_Holder = new Dictionary<string, Holder>();
    public Dictionary<string, Item_Scriptable> Data_Item_Dictionary = new Dictionary<string, Item_Scriptable>(); // 장비를 포함한 모든 아이템 딕셔너리
    public Dictionary<string, Status_Item_Holder> Status_Item_Holder = new Dictionary<string, Status_Item_Holder>();
    public Dictionary<string, Status_Item_Scriptable> Status_Item_Dictionary = new Dictionary<string, Status_Item_Scriptable>(); // 스테이터스 성장장비 딕셔너리
    public Dictionary<string, Item_Scriptable> Data_Drop_Item_Dictionary = new Dictionary<string, Item_Scriptable>(); // 장비를 미 포함한 드롭아이템 딕셔너리
    public Item_Scriptable[] Main_Set_Item = new Item_Scriptable[9]; // 유물 장착칸   
    public List<Smelt_Holder> User_Main_Data_Smelt_Array = new List<Smelt_Holder>();


    public void Init()
    {
        Set_Character();
        Set_Item();
        Set_Status_Item();
    }
    public Character_Scriptable Get_Rarity_Character(Rarity rarity)
    {
        List<Character_Scriptable> Ch_Scriptable_Data = new List<Character_Scriptable>();

        foreach (var data in Data_Character_Dictionary)
        {
            if (data.Value.Data.Rarity == rarity && data.Value.Data.Main_Character == false)
            {
                Ch_Scriptable_Data.Add(data.Value.Data);
            }
        }

        return Ch_Scriptable_Data[UnityEngine.Random.Range(0, Ch_Scriptable_Data.Count)];

    }
    public Item_Scriptable Get_Rarity_Relic(Rarity rarity)
    {
        List<Item_Scriptable> item_Scriptable_Data = new List<Item_Scriptable>();

        foreach (var data in Data_Item_Dictionary)
        {
            if (data.Value.rarity == rarity && data.Value.ItemType == ItemType.Equipment)
            {
                item_Scriptable_Data.Add(data.Value);
            }
        }

        return item_Scriptable_Data[UnityEngine.Random.Range(0, item_Scriptable_Data.Count)];

    }
    private void Set_Character()
    {
        var datas = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");


        foreach (var data in datas)
        {
            var character = new Character_Holder();

            character.Data = data;
            Holder s_holder = new Holder();

            if (character_Holder.ContainsKey(data.Character_EN_Name))
            {
                s_holder = character_Holder[data.Character_EN_Name];
            }
            else
            {
                character_Holder.Add(data.Character_EN_Name, s_holder);
            }
            character.holder = s_holder;

            if (!Data_Character_Dictionary.ContainsKey(data.Character_EN_Name))
            {
                Data_Character_Dictionary.Add(data.Character_EN_Name, character);
            }
            
        }
    }
    private void Set_Item()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        foreach (var data in datas)
        {
            var item = new Item_Holder();

            item.Data = data;
            Holder s_holder = new Holder();

            if (Item_Holder.ContainsKey(data.name))
            {
                s_holder = Item_Holder[data.name];
            }
            else
            {
                Item_Holder.Add(data.name, s_holder);
            }
            item.holder = s_holder;

            if (!Data_Item_Dictionary.ContainsKey(data.name))
            {
                Data_Item_Dictionary.Add(data.name, item.Data);
            }

            if (!Data_Drop_Item_Dictionary.ContainsKey(data.name))
            {
                if(data.ItemType != ItemType.Equipment)
                {
                    Data_Drop_Item_Dictionary.Add(data.name, item.Data);
                }             
            }
            
        }
    }
    private void Set_Status_Item()
    {
        var datas = Resources.LoadAll<Status_Item_Scriptable>("Scriptable/Status_Item");

        foreach (var data in datas)
        {
            var item = new Status_Holder();

            item.Data = data;

            Status_Item_Holder holder = new Status_Item_Holder();

            if (Status_Item_Holder.ContainsKey(data.name))
            {
                holder = Status_Item_Holder[data.name];
            }
            else
            {
                Status_Item_Holder.Add(data.name, holder);
            }

            item.holder = holder;

            if (!Status_Item_Dictionary.ContainsKey(data.name))
            {
                Status_Item_Dictionary.Add(data.name, item.Data);
            }
        }
    }
    public void Set_Player_ATK_HP()
    {
        Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
        Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

        for (int i = 0; i < Spawner.m_players.Count; i++) // 영웅들 각각 ATK와 HP 세팅
        {
            Spawner.m_players[i].Set_ATK_HP_Sub_Hero();
        }
    }
    public float Get_smelt_value(Smelt_Status status)
    {
        float value = 0.0f;

        for (int i = 0; i < User_Main_Data_Smelt_Array.Count; i++)
        {
            if (User_Main_Data_Smelt_Array[i].smelt_holder == status)
            {
                value += User_Main_Data_Smelt_Array[i].smelt_value;
            }
        }

        return value;
    }

}





