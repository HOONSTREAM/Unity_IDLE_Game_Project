using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status_Item_Data", menuName = "Status_Item Data/Status_Item")]

public class Status_Item_Scriptable : ScriptableObject
{
    public string Item_Name;
    public int Item_Level;
    public string Position;  
    public Rarity rarity;
    public string KO_rarity;
    public double Base_ATK;
    public double Base_HP;
    public double Base_STR;
    public double Base_DEX;
    public double Base_VIT;    
    public string Item_Description; // 아이템 설명

}
