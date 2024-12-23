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

    public double Get_ATK() => Utils.CalculateValue(Base_MONSTER_ATK, Data_Manager.Main_Players_Data.Player_Stage, MONSTER_ATK);
    public double Get_HP() => Utils.CalculateValue(Base_MONSTER_HP, Data_Manager.Main_Players_Data.Player_Stage, MONSTER_HP);
    public double Get_DROP_MONEY() => Utils.CalculateValue(Base_DROP_MONEY, Data_Manager.Main_Players_Data.Player_Stage, DROP_MONEY);
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
