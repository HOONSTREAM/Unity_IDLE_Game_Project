using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���������� ���� �� ĳ������ �ɷ�ġ�� ���� �����������Դϴ�.
/// �������������� �̿��մϴ�.
/// </summary>
[CreateAssetMenu(fileName = "Level_Design", menuName = "Level Design/Level Design Data")]
public class Level_Design : ScriptableObject
{
    public LevelData levelData; // Ÿ�԰�ü����
    [Space(20f)]
    public StageData stageData;
    [Space(20f)]
    public HeroCardData heroCardData;
    [Space(20f)]
    public Dual_Blader_Effect_Data Dual_Effect_Data;
    [Space(20f)]
    public PalaDin_Effect_Data PalaDin_Effect_Data;

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

    public double Get_ATK() => Utils.CalculateValue(Base_ATK, Data_Manager.Main_Players_Data.Player_Level, ATK);
    public double Get_HP() => Utils.CalculateValue(Base_HP, Data_Manager.Main_Players_Data.Player_Level, HP);
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
public class Dual_Blader_Effect_Data
{
    public int Current_Level;
    [Range(0.0f, 10.0f)]
    public float ALL_ATK, ALL_DROP;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public float Base_ATK;
    public int Base_DROP;
   

    public double Get_ALL_ATK(Character_Scriptable Data) => Utils.CalculateValue(Base_ATK, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_ATK);

}

[System.Serializable]
public class PalaDin_Effect_Data
{
    public int Current_Level;
    [Range(0.0f, 10.0f)]
    public float ALL_ATK;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public float Base_ATK;
    


    public double Get_ALL_ATK(Character_Scriptable Data) => Utils.CalculateValue(Base_ATK, Base_Manager.Data.character_Holder[Data.name].Hero_Level, ALL_ATK);

}
