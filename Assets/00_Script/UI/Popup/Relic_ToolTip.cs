using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Relic_ToolTip : MonoBehaviour
{
    
    private RectTransform Rect;
    [SerializeField]
    private Image Relic_Image;
    [SerializeField]
    private TextMeshProUGUI Relic_Name_Text, Rarity_Text;
    [SerializeField]
    private ParticleImage Legendary_Particle;

    private string start_percent;
    private string effect_percent = default;

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

 
    public void Show_Relic_ToolTip(Item_Scriptable item, Vector2 pos)
    {
        // 최종 위치 적용
        Rect.anchoredPosition = pos;

        
        Relic_Image.sprite = Utils.Get_Atlas(item.name);
        Relic_Name_Text.text = item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(item.rarity) + item.KO_rarity.ToString();


        if (item.rarity >= Rarity.Legendary)
        {
            Legendary_Particle.gameObject.SetActive(true);
        }

        else
        {
            Legendary_Particle.gameObject.SetActive(false);
        }
    }

  
}
