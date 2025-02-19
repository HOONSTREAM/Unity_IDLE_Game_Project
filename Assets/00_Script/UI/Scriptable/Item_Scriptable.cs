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
    public float Item_Chance; // �� �ۼ�Ʈ Ȯ���� �� �������� ��� �� �� �ִ����� ���� ����
    public int Minimum_Drop_Stage;
   
}
