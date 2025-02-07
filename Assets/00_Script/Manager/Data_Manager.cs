using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �������� ���� �� DB�� �����ϴ� �뵵�� ����ϴ� �Ŵ��� �Դϴ�.
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
    /// ó�� ȸ�������� �ϸ�, ���� �����͸� �ʱ�ȭ �մϴ�.
    /// </summary>
    public void Initialize_User_Data()
    {
        // Step 1. ���� ��ȭ �ʱ�ȭ�ϱ�

        if (Data_Manager.Main_Players_Data == null)
        {
            Data_Manager.Main_Players_Data = new Data();
        }

        Debug.Log("�����͸� �ʱ�ȭ�մϴ�.");

        Data_Manager.Main_Players_Data.Nick_Name = default;
        Data_Manager.Main_Players_Data.ATK = default;
        Data_Manager.Main_Players_Data.HP = default;
        Data_Manager.Main_Players_Data.Player_Money = default;
        Data_Manager.Main_Players_Data.DiaMond = default;
        Data_Manager.Main_Players_Data.Player_Level = default;
        Data_Manager.Main_Players_Data.EXP = default;
        Data_Manager.Main_Players_Data.Player_Stage = 1;
        Data_Manager.Main_Players_Data.EXP_Upgrade_Count = 0;
        Data_Manager.Main_Players_Data.Buff_Timers[0] = 0;
        Data_Manager.Main_Players_Data.Buff_Timers[1] = 0;
        Data_Manager.Main_Players_Data.Buff_Timers[2] = 0;
        Data_Manager.Main_Players_Data.buff_x2_speed = 0;
        Data_Manager.Main_Players_Data.Buff_Level = default;
        Data_Manager.Main_Players_Data.Buff_Level_Count = default;
        Data_Manager.Main_Players_Data.Quest_Count = default;
        Data_Manager.Main_Players_Data.Hero_Summon_Count = default;
        Data_Manager.Main_Players_Data.Hero_Pickup_Count = default;
        Data_Manager.Main_Players_Data.Relic_Pickup_Count = default;
        Data_Manager.Main_Players_Data.Relic_Summon_Count = default;
        Data_Manager.Main_Players_Data.StartDate = DateTime.Now.ToString();
        Data_Manager.Main_Players_Data.EndDate = DateTime.Now.ToString();
        Data_Manager.Main_Players_Data.Daily_Enter_Key[0] = 3;
        Data_Manager.Main_Players_Data.Daily_Enter_Key[1] = 3;
        Data_Manager.Main_Players_Data.User_Key_Assets[0] = 0;
        Data_Manager.Main_Players_Data.User_Key_Assets[1] = 0;
        Data_Manager.Main_Players_Data.Dungeon_Clear_Level[0] = 0;
        Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1] = 0;

        Param param = new Param();

        param.Add("SPEED", Data_Manager.Main_Players_Data.buff_x2_speed);
        param.Add("NICK_NAME", Data_Manager.Main_Players_Data.Nick_Name);
        param.Add("ATK", Data_Manager.Main_Players_Data.ATK);
        param.Add("HP", Data_Manager.Main_Players_Data.HP);
        param.Add("PLAYER_MONEY", Data_Manager.Main_Players_Data.Player_Money);
        param.Add("DIAMOND", Data_Manager.Main_Players_Data.DiaMond);
        param.Add("PLAYER_LEVEL", Data_Manager.Main_Players_Data.Player_Level);
        param.Add("PLAYER_EXP", Data_Manager.Main_Players_Data.EXP);
        param.Add("PLAYER_STAGE", Data_Manager.Main_Players_Data.Player_Stage);
        param.Add("EXP_UPGRADE_COUNT", Data_Manager.Main_Players_Data.EXP_Upgrade_Count);
        param.Add("BUFF_TIMER", Data_Manager.Main_Players_Data.Buff_Timers);     
        param.Add("BUFF_LEVEL", Data_Manager.Main_Players_Data.Buff_Level);
        param.Add("BUFF_LEVEL_COUNT", Data_Manager.Main_Players_Data.Buff_Level_Count);
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
        

        Debug.Log("'USER' ���̺� ���ο� ������ ���� �߰��մϴ�.");

        var bro = Backend.GameData.Insert("USER", param);

        if (bro.IsSuccess())
        {
            Debug.Log("���� �⺻ �����͸� �߰��ϴµ� �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("���� �⺻ �����͸� �߰��ϴµ� �����߽��ϴ�. : " + bro);
        }

        Param character_param = new Param();

        character_param.Add("character", Base_Manager.Data.character_Holder);

        Debug.Log("CHARACTER ���̺� ���ο� ������ ���� �߰��մϴ�.");

        var char_bro = Backend.GameData.Insert("CHARACTER",character_param);

        if (char_bro.IsSuccess())
        {
            Debug.Log("���� ���� �����͸� �߰��ϴµ� �����߽��ϴ�. : " + char_bro);
        }
        else
        {
            Debug.LogError("���� ���� �����͸� �߰��ϴµ� �����߽��ϴ�. : " + char_bro);
        }

        Param item_param = new Param();

        item_param.Add("Item", Base_Manager.Data.Item_Holder);

        Debug.Log("ITEM ���̺� ���ο� ������ ���� �߰��մϴ�.");

        var item_bro = Backend.GameData.Insert("ITEM", item_param);

        if (item_bro.IsSuccess())
        {
            Debug.Log("�κ��丮 �����͸� �߰��ϴµ� �����߽��ϴ�. : " + item_bro);
        }
        else
        {
            Debug.LogError("�κ��丮 �����͸� �߰��ϴµ� �����߽��ϴ�. : " + item_bro);
        }

        Param smelt_param = new Param();

        smelt_param.Add("Smelt", Base_Manager.Data.User_Main_Data_Smelt_Array);

        Debug.Log("ITEM ���̺� ���ο� ������ ���� �߰��մϴ�.");

        var smelt_bro = Backend.GameData.Insert("SMELT", smelt_param);

        if (smelt_bro.IsSuccess())
        {
            Debug.Log("�κ��丮 �����͸� �߰��ϴµ� �����߽��ϴ�. : " + smelt_bro);
        }
        else
        {
            Debug.LogError("�κ��丮 �����͸� �߰��ϴµ� �����߽��ϴ�. : " + smelt_bro);
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

public class Holder
{
    public int Hero_Level;
    public int Hero_Card_Amount;
}

public class Data
{

    public string Nick_Name = default;
    public double ATK;
    public double HP;
    public double Player_Money;
    public int DiaMond;
    public int Player_Level;
    public double EXP;
    public int Player_Stage = 1;
    public int EXP_Upgrade_Count;
    public float[] Buff_Timers = { 0.0f, 0.0f, 0.0f };
    public float buff_x2_speed;
    public int Buff_Level, Buff_Level_Count;
    public int Quest_Count;


    /// <summary>
    /// �÷��̾� ������ ��ȯ ���� ����
    /// </summary>
    public int Hero_Summon_Count;
    /// <summary>
    /// �÷��̾� ������ Ȯ�� ��ȯ ī��Ʈ
    /// </summary>
    public int Hero_Pickup_Count;

    /// <summary>
    /// �÷��̾� ������ ���� ��ȯ ���� ����
    /// </summary>
    public int Relic_Summon_Count;
    /// <summary>
    /// �÷��̾� ������ ���� Ȯ�� ��ȯ ī��Ʈ
    /// </summary>
    public int Relic_Pickup_Count;

    /// <summary>
    /// �÷��̾� ������ ���� ���۽ð�, ����ð� ���
    /// </summary>
    public string StartDate;
    public string EndDate;

    //Dungeon
    public int[] Daily_Enter_Key = { 3, 3 }; // ���ϸ��� �ʱ�ȭ �Ǵ� Ű
    public int[] User_Key_Assets = { 0, 0 }; // ������ �������� ���� Ű 
    public int[] Dungeon_Clear_Level = { 0, 0 }; //������ ���������� Ŭ������ ���̵�

    //������ ����
    public bool isBuyADPackage = false;

}
public class Data_Manager
{

    public static Data Main_Players_Data = new Data();
    /// <summary>
    /// �÷��̾ ���� �������� �������� �����մϴ�.
    /// </summary>
    public Dictionary<string, Character_Holder> Data_Character_Dictionary = new Dictionary<string, Character_Holder>();
    public Dictionary<string, Holder> Item_Holder = new Dictionary<string, Holder>();
    public Dictionary<string, Holder> character_Holder = new Dictionary<string, Holder>();
    public Dictionary<string, Item_Scriptable> Data_Item_Dictionary = new Dictionary<string, Item_Scriptable>();
    public Item_Scriptable[] Main_Set_Item = new Item_Scriptable[9]; // ���� ����ĭ
    public List<Smelt_Holder> User_Main_Data_Smelt_Array = new List<Smelt_Holder>();


    public void Init()
    {

        Set_Character();
        Set_Item();
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

            if (character_Holder.ContainsKey(data.M_Character_Name))
            {
                s_holder = character_Holder[data.M_Character_Name];
            }
            else
            {
                character_Holder.Add(data.M_Character_Name, s_holder);
            }
            character.holder = s_holder;

            if (!Data_Character_Dictionary.ContainsKey(data.M_Character_Name))
            {
                Data_Character_Dictionary.Add(data.M_Character_Name, character);
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
            
        }
    }

    public void Set_Player_ATK_HP()
    {
        Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
        Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

        for (int i = 0; i < Spawner.m_players.Count; i++) // ������ ���� ATK�� HP ����
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





