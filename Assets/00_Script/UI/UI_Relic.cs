using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Relic : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Relic_Parts> relic_parts = new List<UI_Relic_Parts>();
    private Dictionary<string, Item_Scriptable> _dict = new Dictionary<string, Item_Scriptable>(); // 유물장비 저장 딕셔너리
    private Item_Scriptable Item;
    private const int RELIC_SLOT_NUMBER = 9;

    public override bool Init()
    { 
        var Data = Base_Manager.Data.Data_Item_Dictionary; //모든 아이템 딕셔너리

        foreach (var data in Data)
        {
            if(data.Value.ItemType == ItemType.Equipment)
            {
                _dict.Add(data.Value.name, data.Value);
            }          
        }


        var sort_dict = _dict.OrderByDescending(x => x.Value.rarity);


        int value = 0;


        foreach (var data in sort_dict)
        {
            var Object = Instantiate(Parts, Content).GetComponent<UI_Relic_Parts>(); // Content를 부모오브젝트로 해서 Parts를 생성
            value++;
            relic_parts.Add(Object);
            int index = value;
            Object.Init(data.Value, this);
        }

        return base.Init();
    }

    
}
