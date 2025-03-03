using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_ToolTip : MonoBehaviour
{
    
    private RectTransform Rect;
    [SerializeField]
    private Image Item_Image;
    [SerializeField]
    private TextMeshProUGUI Item_Name_Text, Rarity_Text, Description_Text;
    [SerializeField]
    private ParticleImage Legendary_Particle;

    private void Awake()
    {

        Rect = this.GetComponent<RectTransform>();
       
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePos;
            // ScreenPointToLocalPointInRectangle
            // UI시스템에서 스크린 좌표를 특정 UI요소
            // (RectTransform{UI 내부에서 위치를 결정하는 핵심 속성!})의 로컬좌표로 변환하는 기능을 수행.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform, Input.mousePosition, null, out localMousePos);

            // 툴팁 내부를 클릭한 경우 아무것도 안 함
            if (((RectTransform)transform).rect.Contains(localMousePos))
                return;

            // 툴팁 바깥을 클릭하면 제거
            Base_Canvas.instance.item_tooltip = null;
            Destroy(gameObject);
        }


    }
           
    public void Show_Item_ToolTip(Item_Scriptable item, Vector2 pos)
    {
        Rect.pivot = Set_Pivot_Point(pos);

        Rect.anchoredPosition = pos;
        Item_Image.sprite = Utils.Get_Atlas(item.name);
        Item_Name_Text.text = item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(item.rarity) + item.KO_rarity.ToString();
        Description_Text.text = string.Format(item.Item_Description,15,30);

        if(item.rarity == Rarity.Legendary)
        {
            Legendary_Particle.gameObject.SetActive(true);
        }
        else
        {
            Legendary_Particle.gameObject.SetActive(false);
        }
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
