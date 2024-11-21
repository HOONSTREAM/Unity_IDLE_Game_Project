using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public Item_Scriptable data;
    public int Count;
}

/// <summary>
/// 아이템을 관리하는 매니저 입니다.
/// </summary>
public class Item_Manager 
{
    private Dictionary<string, Item_Scriptable> Item_Datas = new Dictionary<string, Item_Scriptable>();

    public void Init()
    {
        var Datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        for(int i = 0; Datas.Length > i; i++)
        {
            Item_Datas.Add(Datas[i].name, Datas[i]);          
        }
    }

    public List<Item_Scriptable> Get_Drop_Set()
    {
        List<Item_Scriptable> objs = new List<Item_Scriptable>();

        foreach(var data in Item_Datas)
        {
            float ValueCount = Random.Range(0.0f, 100.0f);
            if(ValueCount <= data.Value.Item_Chance)
            {
                objs.Add(data.Value);
            }
        }

        return objs;
    }
   
}
