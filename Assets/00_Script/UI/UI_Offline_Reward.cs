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
        int sampleInterval = 10;
        int iterations = TimeValue / sampleInterval;

        for (int i = 0; i < iterations; i++)
        {
            var Drop_items = Base_Manager.Item.Get_Drop_Set();
            foreach (var drop in Drop_items)
            {
                if (items.TryGetValue(drop.name, out var existing))
                {
                    existing.holder.Hero_Card_Amount += sampleInterval;
                }
                else
                {
                    items.Add(drop.name, new Item_Holder
                    {
                        Data = drop,
                        holder = new Holder { Hero_Card_Amount = sampleInterval }
                    });
                }
            }

            if (i % 100 == 0)
                yield return null;
        }

        int index = 0;
        int perFrameInstantiate = 5;
        List<Item_Holder> itemList = new List<Item_Holder>(items.Values);

        for (int i = 0; i < itemList.Count; i++)
        {
            var go = Instantiate(UI_INVENTORY_PARTS_Item, Content);
            go.Init(itemList[i].Data.name, itemList[i].holder);

            if (index < 10) // 처음 10개만 애니메이션
                go.PlayAppearAnimation(index * 0.05f);

            index++;

            if (index % perFrameInstantiate == 0)
                yield return null; // 5개 단위로 1프레임 쉬기
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
