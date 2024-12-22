using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유저가 획득한 아이템을 관리하는 매니저 입니다.
/// </summary>
public class Inventory_Manager 
{
  
    public void Get_Item(Item_Scriptable item, int Drop_count = 1)
    {
        if (Base_Manager.Data.Item_Holder.ContainsKey(item.name))
        {
            Base_Manager.Data.Item_Holder[item.name].Hero_Card_Amount += Drop_count;

            return;
        }

    }
}
