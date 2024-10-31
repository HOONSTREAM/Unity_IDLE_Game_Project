using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_Design", menuName = "Level Design/Level Design Data")]
public class Level_Design : ScriptableObject
{
    public LevelData levelData; // 타입객체패턴

    public float CalculateValue(float baseValue, int level, float value)
    {
        return baseValue * Mathf.Pow(level, value);
    }
}

[System.Serializable]
public class LevelData
{
    public int Current_Level;
    public float ATK, HP, EXP, MAX_EXP, LEVELUP_MONEY;

}
