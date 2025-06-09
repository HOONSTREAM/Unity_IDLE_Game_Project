using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨이 오를때마다 변경 될 캐릭터의 능력치에 관한 레벨디자인입니다.
/// 지수증가공식을 이용합니다.
/// </summary>
[CreateAssetMenu(fileName = "Level_Design", menuName = "Level Design/Level Design Data")]
public class Level_Design : ScriptableObject
{
    public LevelData levelData; // 타입객체패턴
    [Space(20f)]
    public StageData stageData;
    [Space(20f)]
    public HeroCardData heroCardData;    
    [Space(20f)]
    public Holding_Effect_Data Holding_Effect_Data;
    [Space(20f)]
    public Holding_Effect_Data_Relic Hoiding_Effect_Data_Relic;

}

[System.Serializable]
public class LevelData
{
    public int Current_Level;
    [Range(0.0f, 10.0f)]
    public float ATK, HP, EXP, MAX_EXP, LEVELUP_MONEY;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public int Base_ATK;
    public int Base_HP;
    public int Base_EXP;
    public int Base_MAX_EXP;
    public int Base_LEVELUP_MONEY;

    public double Get_Levelup_Next_ATK() => Utils.CalculateValue(Base_ATK, Data_Manager.Main_Players_Data.Player_Level, ATK);   
    public double Get_Levelup_Next_HP() => Utils.CalculateValue(Base_HP, Data_Manager.Main_Players_Data.Player_Level, HP);
    public double Get_Levelup_Next_LEVEL_ATK() => Utils.CalculateValue(Base_ATK, Data_Manager.Main_Players_Data.Player_Level + 1, ATK);
    public double Get_Levelup_Next_LEVEL_HP() => Utils.CalculateValue(Base_HP, Data_Manager.Main_Players_Data.Player_Level + 1, HP);
    public double Get_EXP() => Utils.CalculateValue(Base_EXP, Data_Manager.Main_Players_Data.Player_Level, EXP);
    public double Get_MAXEXP() => Utils.CalculateValue(Base_MAX_EXP, Data_Manager.Main_Players_Data.Player_Level, MAX_EXP);
    public double Get_LEVELUP_MONEY() => Utils.CalculateValue(Base_LEVELUP_MONEY, Data_Manager.Main_Players_Data.Player_Level, LEVELUP_MONEY);


}

[System.Serializable]
public class StageData
{
    public int Current_Stage;
    [Range(0.0f, 10.0f)]
    public float MONSTER_ATK, MONSTER_HP, DROP_MONEY;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public int Base_MONSTER_ATK;
    public int Base_MONSTER_HP;
    public int Base_DROP_MONEY;

    public double Get_ATK(int Dungeon_Value = 0) => Utils.CalculateValue(Base_MONSTER_ATK, Dungeon_Value == 0 ? Data_Manager.Main_Players_Data.Player_Stage : Dungeon_Value, MONSTER_ATK);
    public double Get_HP(int Dungeon_Value = 0) => Utils.CalculateValue(Base_MONSTER_HP, Dungeon_Value == 0 ? Data_Manager.Main_Players_Data.Player_Stage : Dungeon_Value, MONSTER_HP);
    public double Get_DROP_MONEY(int Dungeon_Value = 0) => Utils.CalculateValue(Base_DROP_MONEY, Dungeon_Value == 0 ? Data_Manager.Main_Players_Data.Player_Stage : Dungeon_Value, DROP_MONEY);
}

[System.Serializable]

public class HeroCardData
{
    public int Current_Card_Amount;
    [Range(0.0f, 10.0f)]
    public float Levelup_Card_Amount;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public int Base_Levelup_Card_Amount;

    public int Get_LEVELUP_Card_Amount(string name)
    {
        return (int)Utils.CalculateValue(Base_Levelup_Card_Amount, (Base_Manager.Data.character_Holder[name].Hero_Level),
            Levelup_Card_Amount);
    }

    public int Get_LEVELUP_Relic_Card_Amount(string name)
    {
        return (int)Utils.CalculateValue(Base_Levelup_Card_Amount, (Base_Manager.Data.Item_Holder[name].Hero_Level),
            Levelup_Card_Amount);
    }
}

[System.Serializable]
public class Holding_Effect_Data
{
    public int Current_Level;
    [Range(0.0f, 10.0f)]
    public float ALL_ATK;
    public float ALL_HP;
    public float ALL_ATK_SPEED;
    public float ALL_GOLD_DROP;
    public float ALL_ITEM_DROP;
    public float ALL_CRI_DMG;
    public float ALL_CRI_PCT;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public float Base_ATK;
    public float Base_HP;
    public float Base_ATK_SPEED;
    public float Base_GOLD_DROP;
    public float Base_ITEM_DROP;
    public float Base_CRI_DMG;
    public float Base_CRI_PCT;


    public double Get_NONE_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        return 0.0;
    }
    public double Get_ALL_ATK_Holding_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {          
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_ATK, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_ATK);
        double Application_Rarity_Value = Value * (((int)Data.Rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_ATK_SPEED_Holding_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {          
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_ATK_SPEED, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_ATK_SPEED);
        double Application_Rarity_Value = Value * (((int)Data.Rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_HP_Holding_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {         
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_HP, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_HP);
        double Application_Rarity_Value = Value * (((int)Data.Rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_GOLD_DROP_Holding_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_GOLD_DROP, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_GOLD_DROP);
        double Application_Rarity_Value = Value * (((int)Data.Rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_ITEM_DROP_Holding_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_ITEM_DROP, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_ITEM_DROP);
        double Application_Rarity_Value = Value * (((int)Data.Rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_CRI_DMG_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_CRI_DMG, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_CRI_DMG);
        double Application_Rarity_Value = Value * (((int)Data.Rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_CRI_PERCENT_Effect(Character_Scriptable Data)
    {
        if (Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_CRI_PCT, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_CRI_PCT);
        double Application_Rarity_Value = Value * (((int)Data.Rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }


}

[System.Serializable]
public class Holding_Effect_Data_Relic
{
    public int Current_Level;
    [Range(0.0f, 10.0f)]
    public float ALL_ATK;
    public float ALL_HP;
    public float ALL_ATK_SPEED;
    public float ALL_GOLD_DROP;
    public float ALL_ITEM_DROP;
    public float ALL_CRI_DMG;
    public float ALL_CRI_PCT;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public float Base_ATK;
    public float Base_HP;
    public float Base_ATK_SPEED;
    public float Base_GOLD_DROP;
    public float Base_ITEM_DROP;
    public float Base_CRI_DMG;
    public float Base_CRI_PCT;


    public double Get_ALL_ATK_Holding_Effect_Relic(Item_Scriptable Data)
    {
        if (Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_ATK, Base_Manager.Data.Item_Holder[Data.name].Hero_Level, ALL_ATK);
        double Application_Rarity_Value = Value * (((int)Data.rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_ATK_SPEED_Holding_Effect_Relic(Item_Scriptable Data)
    {
        if (Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_ATK_SPEED, Base_Manager.Data.Item_Holder[Data.name].Hero_Level, ALL_ATK_SPEED);
        double Application_Rarity_Value = Value * (((int)Data.rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_HP_Holding_Effect_Relic(Item_Scriptable Data)
    {
        if (Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_HP, Base_Manager.Data.Item_Holder[Data.name].Hero_Level, ALL_HP);
        double Application_Rarity_Value = Value * (((int)Data.rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_GOLD_DROP_Holding_Effect_Relic(Item_Scriptable Data)
    {
        if (Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount <= 0)
        {
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_GOLD_DROP, Base_Manager.Data.Item_Holder[Data.name].Hero_Level, ALL_GOLD_DROP);
        double Application_Rarity_Value = Value * (((int)Data.rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_ITEM_DROP_Holding_Effect_Relic(Item_Scriptable Data)
    {
        if (Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount <= 0)
        {           
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_ITEM_DROP, Base_Manager.Data.Item_Holder[Data.name].Hero_Level, ALL_ITEM_DROP);
        double Application_Rarity_Value = Value * (((int)Data.rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_CRI_DMG_Effect_Relic(Item_Scriptable Data)
    {
        if (Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount <= 0)
        {           
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_CRI_DMG, Base_Manager.Data.Item_Holder[Data.name].Hero_Level, ALL_CRI_DMG);
        double Application_Rarity_Value = Value * (((int)Data.rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }

    public double Get_ALL_CRI_PERCENT_Effect_Relic(Item_Scriptable Data)
    {
        if (Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount <= 0)
        {          
            return 0.0; // 보유하지 않으면 효과 적용 안 함 (기본값 반환)
        }

        double Value = Utils.CalculateValue(Base_CRI_PCT, Base_Manager.Data.Item_Holder[Data.name].Hero_Level, ALL_CRI_PCT);
        double Application_Rarity_Value = Value * (((int)Data.rarity + 1) * 1.2);

        return Application_Rarity_Value;
    }


}

