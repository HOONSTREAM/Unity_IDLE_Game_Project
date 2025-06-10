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
    public double Base_INT;    
    public string Set_Effect_Weapon_Name; // 세트효과 무기 이름
    public string Set_Effect_ACC_Name; // 세트효과 악세사리 이름
    public string Set_Effect_Description; // 세트효과 설명
    public string Item_Description; // 아이템 설명

}
