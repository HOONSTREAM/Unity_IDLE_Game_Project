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
    public int Base_ATK;
    public int Base_HP;
    public int Base_DROP_MONEY;

}
