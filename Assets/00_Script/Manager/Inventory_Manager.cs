using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유저가 획득한 아이템을 관리하는 매니저 입니다.
/// </summary>
public class Inventory_Manager 
{
    public Dictionary<string, Item> Items_Dict = new Dictionary<string, Item>(); 
   
    public void Get_Item(Item_Scriptable item)
    {
        if (Items_Dict.ContainsKey(item.name))
        {
            Items_Dict[item.name].Count++;

            return;
        }

        Items_Dict.Add(item.name, new Item { data = item, Count = 1 });
       
    }
}
