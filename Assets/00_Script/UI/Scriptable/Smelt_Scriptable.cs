using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Smelt_Data", menuName = "Smelt Data/Smelt")]



public class Smelt_Scriptable : ScriptableObject
{
    [Header("Appear")]
    public float[] Smelt_Count_Value; // 1~5���� �ɷ�ġ�� �����ϰ� ������ Ȯ�� (1���� �����°�, 5���� �����°�)

    [Space(20f)]
    [Header("Percentage")]
    public Percentage_Smelt[] ATK_percentage; 
    public Percentage_Smelt[] HP_percentage; 
    public Percentage_Smelt[] MONEY_percentage;
    public Percentage_Smelt[] ITEM_percentage;
    public Percentage_Smelt[] SKILL_COOL_percntage;
    public Percentage_Smelt[] CRITICAL_PER_percentage; 
    public Percentage_Smelt[] CRITICAL_DMG_percentage;
    public Percentage_Smelt[] ATK_SPEED_percentage;

}
