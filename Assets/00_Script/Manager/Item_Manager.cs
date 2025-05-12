using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 관리하는 매니저 입니다.
/// </summary>
public class Item_Manager 
{
    private List<Item_Scriptable> _cachedDropPool = new List<Item_Scriptable>(); // 드롭 아이템 후보군 미리 계산 하는 풀
    private int _lastStage = -1;

   /// <summary>
    /// 플레이어 해당 층 수에 따른, 드롭보상을 미리 계산합니다.
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
    /// 모든 아이템 데이터가 들어있는 딕셔너리를 순회하면서 일정 확률로 임시 리스트에 드랍 아이템을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public List<Item_Scriptable> Get_Drop_Set()
    {
        RefreshDropPool(); // 캐시된 드롭풀 갱신 여부 확인

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
    /// 유물 아이템을 장착했는지 확인합니다.
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
