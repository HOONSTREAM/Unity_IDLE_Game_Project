using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
public class Item_Manager 
{

    /// <summary>
    /// ���� �������� �����ߴ��� Ȯ���մϴ�.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Set_Item_Check(string name)
    {
        for (int i = 0; i < Base_Manager.Data.Main_Set_Item.Length; i++)
        {
            if (Base_Manager.Data.Main_Set_Item[i] != null)
            {
                if (Base_Manager.Data.Main_Set_Item[i].name == name)
                {
                    return true;
                }                                                  
            }
        }
        return false;
    }
    /// <summary>
    /// ��� ������ �����Ͱ� ����ִ� ��ųʸ��� ��ȸ�ϸ鼭 ���� Ȯ���� �ӽ� ����Ʈ�� ��� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public List<Item_Scriptable> Get_Drop_Set()
    {
        List<Item_Scriptable> objs = new List<Item_Scriptable>();

        foreach(var data in Base_Manager.Data.Data_Item_Dictionary)
        {
            if(data.Value.Minimum_Drop_Stage <= Data_Manager.Main_Players_Data.Player_Stage)
            {
                float ValueCount = Random.Range(0.0f, 100.0f);
                float Smelt_Value = Base_Manager.Player.Calculate_Item_Drop_Percentage();
                
                float Adjusted_Chance = data.Value.Item_Chance * (1 + (Smelt_Value / 100.0f));
            
                if (ValueCount <= Adjusted_Chance)
                {
                    objs.Add(data.Value);
                }
            }


        }

        return objs;
    }

    /// <summary>
    /// ���� ����ĭ��, ���������� ����Ǿ��ִ� ��ųʸ����� Ư�� string���� ����, ��ġ�ϴ� value���� ������ŵ�ϴ�.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="item_name"></param>
    public void Get_Item(int value, string item_name)
    {
        Base_Manager.Data.Main_Set_Item[value] = Base_Manager.Data.Data_Item_Dictionary[item_name];
    }

    /// <summary>
    /// Ư�� value���� �迭�� ���������� �����մϴ�.
    /// </summary>
    /// <param name="value"></param>
    public void Disable_Item(int value)
    {
        Base_Manager.Data.Main_Set_Item[value] = null;
    }

}
