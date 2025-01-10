using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �������� ���� �� DB�� �����ϴ� �뵵�� ����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
/// 
public class Character_Holder
{
    public Character_Scriptable Data;
    public Holder holder;
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
    public double Player_Money;
    public int DiaMond;
    public int Player_Level;
    public double EXP;
    public int Player_Stage = 1;
    public float[] Buff_Timers = { 0.0f, 0.0f, 0.0f };
    public float buff_x2_speed = 0.0f;
    public int Buff_Level, Buff_Level_Count;

       
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

    public void Init()
    {
        
        Set_Character();
        Set_Item();      
    }

    public Character_Scriptable Get_Rarity_Character(Rarity rarity)
    {
        List<Character_Scriptable> Ch_Scriptable_Data = new List<Character_Scriptable> ();

        foreach(var data in Data_Character_Dictionary)
        {
            if(data.Value.Data.Rarity == rarity && data.Value.Data.Main_Character == false)
            {
                Ch_Scriptable_Data.Add(data.Value.Data);
            }
        }

        return Ch_Scriptable_Data[Random.Range(0, Ch_Scriptable_Data.Count)];

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

        return item_Scriptable_Data[Random.Range(0, item_Scriptable_Data.Count)];

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

            Data_Character_Dictionary.Add(data.M_Character_Name, character);
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

            Data_Item_Dictionary.Add(data.name, item.Data);
        }
    }

    public void Set_Player_ATK_HP()
    {        
        Base_Manager.Player.ATK = Utils.Data.levelData.Get_ATK();
        Base_Manager.Player.HP = Utils.Data.levelData.Get_HP();

        for (int i = 0; i < Spawner.m_players.Count; i++)
        {
            Spawner.m_players[i].Set_ATK_HP();
        }
    }

    public void Set_Hero_Holding_Effect(Character_Scriptable Data)
    {
        for(int i = 0; i<character_Holder.Count; i++)
        {
            if (character_Holder[Data.name].Hero_Card_Amount > 1)
            {
                Debug.Log($"{Data.name}�� ����ȿ���� ������ŵ�ϴ�.");
            }
        }
    }

}
