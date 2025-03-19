using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Offline_Reward : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI Offline_Time;
    [SerializeField]
    private TextMeshProUGUI money_reward_value;

    [SerializeField]
    private Transform Content;
    [SerializeField]
    private UI_Inventory_Parts UI_INVENTORY_PARTS_Item;
    private Dictionary<string, Item_Holder> items = new Dictionary<string, Item_Holder>();

    private double _money_reward_value;

    public override bool Init()
    {
        //TODO :  오프라인 보상 레벨디자인 필요, 오프라인 보상은 각인효과 적용 X
        _money_reward_value = (Utils.Data.stageData.Get_DROP_MONEY() * Utils.Offline_Timer_Check()) / 3;
        money_reward_value.text = StringMethod.ToCurrencyString(_money_reward_value);

        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        Offline_Time.text = span.Hours + "<color=#FFFF00>HR</color>" + span.Minutes + "<color=#FFFF00>MIN</color>";

        Instantiate_Offline_Items();

        foreach(var item in items)
        {
            var go = Instantiate(UI_INVENTORY_PARTS_Item, Content);
            go.Init(item.Key, item.Value.holder);
        }

        return base.Init();
    }

    /// <summary>
    /// 오프라인 보상의 시간을 계산하여, 아이템을 생성합니다.
    /// </summary>
    private void Instantiate_Offline_Items()
    {

        int TimeValue = (int)Utils.Offline_Timer_Check() / 3;

        for (int i = 0; i < TimeValue; i++)
        {
            var Drop_items = Base_Manager.Item.Get_Drop_Set();

            for (int j = 0; j < Drop_items.Count; j++)
            {
                if (items.ContainsKey(Drop_items[j].name))
                {
                    items[Drop_items[j].name].holder.Hero_Card_Amount++;
                }

                else
                {
                    Item_Holder new_item = new Item_Holder();
                    new_item.Data = Drop_items[j];
                    new_item.holder = new Holder();
                    new_item.holder.Hero_Card_Amount = 1;
                    items.Add(Drop_items[j].name, new_item);
                }
            }
        }
    }

    /// <summary>
    /// 일반 보상으로 오프라인 보상을 수령합니다.
    /// </summary>
    public void Collect_Button()
    {
        Data_Manager.Main_Players_Data.Player_Money += _money_reward_value;

        foreach(var Item in items)
        {
            Base_Manager.Inventory.Get_Item(Item.Value.Data,Item.Value.holder.Hero_Card_Amount);
        }

        DisableOBJ();
        Main_UI.Instance.Level_Text_Check();
        _ = Base_Manager.BACKEND.WriteData();
    }
    /// <summary>
    /// 광고를 시청하고, 2배의 오프라인 보상을 수령합니다.
    /// </summary>
    public void ADS_Collect_Button()
    {
        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            Data_Manager.Main_Players_Data.Player_Money += (_money_reward_value * 2);

            foreach (var Item in items)
            {
                Base_Manager.Inventory.Get_Item(Item.Value.Data, (Item.Value.holder.Hero_Card_Amount*2));
            }
        });

        DisableOBJ();
        Main_UI.Instance.Level_Text_Check();
        _ = Base_Manager.BACKEND.WriteData();
    }

    /// <summary>
    /// 유저가 실수로 오프라인 보상창을 종료해도, 자동으로 기본 보상을 획득하도록 합니다.
    /// </summary>
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
