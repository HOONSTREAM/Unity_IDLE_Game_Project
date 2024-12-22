using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 관리하는 매니저 입니다.
/// </summary>
public class Item_Manager 
{
    /// <summary>
    /// 모든 아이템 데이터가 들어있는 딕셔너리를 순회하면서 일정 확률로 임시 리스트에 드랍 아이템을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public List<Item_Scriptable> Get_Drop_Set()
    {
        List<Item_Scriptable> objs = new List<Item_Scriptable>();

        foreach(var data in Base_Manager.Data.Data_Item_Dictionary)
        {
            if(data.Value.ItemType == ItemType.Consumable) // 유물은 드랍하지 않도록 처리
            {
                float ValueCount = Random.Range(0.0f, 100.0f);
                if (ValueCount <= data.Value.Item_Chance)
                {
                    objs.Add(data.Value);
                }
            }
          
        }

        return objs;
    }
   
}
