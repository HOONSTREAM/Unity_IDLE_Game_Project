using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ȹ���� �������� �����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
public class Inventory_Manager 
{
  
    /// <summary>
    /// ������ �÷��̾�� �������� �����մϴ�.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="Drop_count"></param>
    public void Get_Item(Item_Scriptable item, int Drop_count = 1)
    {
        if (Base_Manager.Data.Item_Holder.ContainsKey(item.name))
        {
            Base_Manager.Data.Item_Holder[item.name].Hero_Card_Amount += Drop_count;

            return;
        }

        else // ���� ������ ����
        {
            Holder holder = new Holder();
            holder.Hero_Card_Amount = Drop_count;
            holder.Hero_Level = 0;
            Base_Manager.Data.Item_Holder.Add(item.name, holder);
        }

        _=Base_Manager.BACKEND.WriteData();

    }
}
