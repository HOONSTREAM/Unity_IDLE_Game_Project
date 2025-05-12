using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
public class Item_Manager 
{
    private List<Item_Scriptable> _cachedDropPool = new List<Item_Scriptable>(); // ��� ������ �ĺ��� �̸� ��� �ϴ� Ǯ
    private int _lastStage = -1;

   /// <summary>
    /// �÷��̾� �ش� �� ���� ����, ��Ӻ����� �̸� ����մϴ�.
    /// </summary>
    public void RefreshDropPool()
    {
        int currentStage = Data_Manager.Main_Players_Data.Player_Stage;

        if (_lastStage == currentStage)
        {
            return;
        }
            
        _cachedDropPool.Clear();

        foreach (var data in Base_Manager.Data.Data_Item_Dictionary)
        {
            if (data.Value.Minimum_Drop_Stage <= currentStage)
            {
                _cachedDropPool.Add(data.Value);
            }
        }

        _lastStage = currentStage;

    }

    /// <summary>
    /// ��� ������ �����Ͱ� ����ִ� ��ųʸ��� ��ȸ�ϸ鼭 ���� Ȯ���� �ӽ� ����Ʈ�� ��� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public List<Item_Scriptable> Get_Drop_Set()
    {
        RefreshDropPool(); // ĳ�õ� ���Ǯ ���� ���� Ȯ��

        List<Item_Scriptable> drops = new List<Item_Scriptable>();

        float smeltBonus = Base_Manager.Player.Calculate_Item_Drop_Percentage();

        foreach (var item in _cachedDropPool)
        {
            float adjustedChance = item.Item_Chance * (1 + smeltBonus / 100f);

            if (Random.value * 100f <= adjustedChance)
            {
                drops.Add(item);
            }
        }

        return drops;
    }

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
