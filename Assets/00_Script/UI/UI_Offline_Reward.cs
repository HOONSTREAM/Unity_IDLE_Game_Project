using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
        int TimeValue = (int)Utils.Offline_Timer_Check();
        double TimeValue_Double = Utils.Offline_Timer_Check();

        _money_reward_value = (Utils.Data.stageData.Get_DROP_MONEY() * TimeValue_Double);
        money_reward_value.text = StringMethod.ToCurrencyString(_money_reward_value);

        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        Offline_Time.text = span.Hours + "<color=#FFFF00>HR</color>" + span.Minutes + "<color=#FFFF00>MIN</color>";

        StartCoroutine(Instantiate_Offline_Item_Coroutine(TimeValue));
        
        return base.Init();
    }
   
    private IEnumerator Instantiate_Offline_Item_Coroutine(int TimeValue)
    {
        int index = 0;
        float delayBetweenItems = 0.05f;

        int batchSize = 1000; // 한번에 처리할 루프 수
        int count = 0;

        for (int i = 0; i < TimeValue; i++)
        {
            var Drop_items = Base_Manager.Item.Get_Drop_Set();
            foreach (var drop in Drop_items)
            {
                if (items.TryGetValue(drop.name, out var existing))
                {
                    existing.holder.Hero_Card_Amount++;
                }
                else
                {
                    Item_Holder newItem = new Item_Holder
                    {
                        Data = drop,
                        holder = new Holder { Hero_Card_Amount = 1 }
                    };
                    items.Add(drop.name, newItem);
                }
            }

            count++;
            if (count >= batchSize)
            {
                count = 0;
                yield return null; // 한 프레임 대기
            }
        }

        // 완료 후 UI 표시
        foreach (var item in items)
        {
            var go = Instantiate(UI_INVENTORY_PARTS_Item, Content);
            go.Init(item.Key, item.Value.holder);


            go.PlayAppearAnimation(index * delayBetweenItems);

            index++;

            yield return new WaitForSecondsRealtime(0.05f);
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
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
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
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
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
