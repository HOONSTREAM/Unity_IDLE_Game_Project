using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{

    public enum Inventory_State { ALL, EQUIPMENT, CONSUMABLE, ETC }
    [SerializeField]
    private Inventory_State NOW_Inventory_State;
    [SerializeField]
    private RectTransform Bar; // 인벤토리의 메뉴 (전체아이템, 장비, 소비, 기타)
    [SerializeField]
    private Button[] Inven_Top_Buttons;

    [SerializeField] 
    private Transform Content;
    [SerializeField] 
    private UI_Inventory_Parts Item_Parts;
    [SerializeField]
    private RectTransform Top_Content;

    public override bool Init()
    {
        var sort_Dictionary = Base_Manager.Data.Data_Item_Dictionary.OrderByDescending(x => x.Value.rarity);

        foreach(var item in sort_Dictionary)
        {
            
            if (Base_Manager.Data.Item_Holder[item.Key].Hero_Card_Amount > 0)
            {
                Instantiate(Item_Parts, Content).Init(item.Key, Base_Manager.Data.Item_Holder[item.Key]);
            }
                        
        }

        for(int i = 0; i< Inven_Top_Buttons.Length; i++)
        {
            int index = i;
            Inven_Top_Buttons[index].onClick.AddListener(() => Item_Inventory_Menu_Check((Inventory_State)index));
        }

        return base.Init();
    }

    public void Item_Inventory_Menu_Check(Inventory_State state)
    {
        NOW_Inventory_State = state;
        StartCoroutine(Bar_Movement_Coroutine(Inven_Top_Buttons[(int)state].GetComponent<RectTransform>().anchoredPosition));
    }

    /// <summary>
    /// 유저가 원하는 메뉴를 누르면 바가 움직이는 기능을 구현합니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Bar_Movement_Coroutine(Vector2 endPos) //float endXPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = Bar.anchoredPosition;
        Vector2 end = new Vector2(endPos.x, Top_Content.anchoredPosition.y);


        float startX = Bar.sizeDelta.x;
        //float endX = endXPos;

        while(percent < 1)
        {

            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
           // float LerpPosX = Mathf.Lerp(startX, endX, percent);
            
            Bar.anchoredPosition = LerpPos;

            //Bar.sizeDelta = new Vector2(LerpPosX, Bar.sizeDelta.y);

            yield return null;
        }
    }
}
