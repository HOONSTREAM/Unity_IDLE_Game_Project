using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Scriptable", menuName = "Object/Character", order = int.MaxValue)]
public class Character_Scriptable : ScriptableObject
{
    public string M_Character_Name;
    public string Character_EN_Name;
    public float M_Attack_Range;
    public float M_Attack_Speed;
    public int MAX_MP;
    public Rarity Rarity;
    public string KO_Rarity;
    public bool Main_Character;
}
