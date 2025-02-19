using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Data", menuName = "Item Data/Item")]

public class Item_Scriptable : ScriptableObject
{
    public string Item_Name;
    public string Item_Description;
    public ItemType ItemType;
    public Rarity rarity;
    public string KO_rarity;
    public float Item_Chance; // 몇 퍼센트 확률로 이 아이템이 드랍 될 수 있는지에 대한 변수
    public int Minimum_Drop_Stage;
   
}
