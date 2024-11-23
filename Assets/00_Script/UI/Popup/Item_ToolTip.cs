using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_ToolTip : MonoBehaviour
{
    
    private RectTransform Rect;
    [SerializeField]
    private Image Item_Image;
    [SerializeField]
    private TextMeshProUGUI Item_Name_Text, Rarity_Text, Description_Text;

    private void Awake()
    {

        Rect = this.GetComponent<RectTransform>();
       
    }

    private void Update()
    {

        if (Input.GetMouseButton(0))
        {
            Base_Canvas.instance.item_tooltip = null;
            Destroy(this.gameObject);
        }


    }
           
    public void Show_Item_ToolTip(Item_Scriptable item, Vector2 pos)
    {
        Rect.pivot = Set_Pivot_Point(pos);

        Rect.anchoredPosition = pos;
        Item_Image.sprite = Utils.Get_Atlas(item.name);
        Item_Name_Text.text = item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(item.rarity) + item.rarity.ToString() + "등급</color>";
        Description_Text.text = item.Item_Description;
    }

    /// <summary>
    /// 아이템 툴팁이 잘리는 이슈를 방지하기 위해서 사용자가 터치한 스크린에 따라서 피벗을 조정합니다.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 Set_Pivot_Point(Vector2 pos)
    {
        float xPos = pos.x > Screen.width / 2 ? 1.0f : 0.0f;
        float yPos = pos.y > Screen.height / 2 ? 1.0f : 0.0f;


        return new Vector2(xPos, yPos);
    }
}
