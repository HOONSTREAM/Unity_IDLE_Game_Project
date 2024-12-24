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

    /// <summary>
    /// 유물 장착칸에, 모든아이템이 저장되어있는 딕셔너리에서 특정 string값을 통해, 일치하는 value값을 장착시킵니다.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="item_name"></param>
    public void Get_Item(int value, string item_name)
    {
        Base_Manager.Data.Main_Set_Item[value] = Base_Manager.Data.Data_Item_Dictionary[item_name];
    }

    /// <summary>
    /// 특정 value값의 배열의 유물장착을 해제합니다.
    /// </summary>
    /// <param name="value"></param>
    public void Disable_Item(int value)
    {
        Base_Manager.Data.Main_Set_Item[value] = null;
    }

}
