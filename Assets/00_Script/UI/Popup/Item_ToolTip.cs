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

 
    public void Show_Item_ToolTip(Item_Scriptable item, Vector2 pos)
    {
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();
        Vector2 tooltipSize = Rect.sizeDelta;
        Vector2 canvasSize = canvasRect.rect.size;


        if (item.ItemType == ItemType.Equipment)
        {
            var effects = RelicEffectFactory.Get_Holding_Effects_Relic(item.name); // 유물 팩토리에서 보유효과를 가져옵니다.

            if (!CSV_Importer.Relic_CSV_DATA_AUTO_Map.TryGetValue(item.name.ToUpper(), out var csvData))
            {
                Debug.LogWarning($"유물 {item.name}의 CSV 데이터를 찾을 수 없습니다.");
                return;
            }

            int heroLevel = Base_Manager.Data.Item_Holder[item.name].Hero_Level;

            if (heroLevel < csvData.Count) // CSV 데이터 존재 여부 확인
            {
                start_percent = csvData[heroLevel]["start_percent"].ToString();

                if (csvData[heroLevel].TryGetValue("effect_percent", out object effectValue))
                {
                    effect_percent = effectValue.ToString();
                }
                else
                {
                    effect_percent = default;  // 기본값 처리
                }
            }
            else
            {
                Debug.LogWarning($"유물 {item.name}의 {heroLevel} 레벨 데이터가 없습니다.");
            }

        }


        string coloredStartPercent = $"<color=#FFFF00>{start_percent}</color>";
        string coloredEffectPercent = $"<color=#FFFF00>{effect_percent}</color>";


        // 최종 위치 적용
        Rect.anchoredPosition = pos;

        Vector2 clampedPos = pos;

        // 좌우 클램핑
        clampedPos.x = Mathf.Clamp(
            clampedPos.x,
            -canvasSize.x / 2 + tooltipSize.x / 2,
            canvasSize.x / 2 - tooltipSize.x / 2
        );

        // 상하 클램핑
        clampedPos.y = Mathf.Clamp(
            clampedPos.y,
            -canvasSize.y / 2 + tooltipSize.y / 2,
            canvasSize.y / 2 - tooltipSize.y / 2
        );

        // 최종 위치 적용
        Rect.anchoredPosition = clampedPos;



        Item_Image.sprite = Utils.Get_Atlas(item.name);
        Item_Name_Text.text = item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(item.rarity) + item.KO_rarity.ToString();

        if(item.ItemType == ItemType.Equipment)
        {
            Description_Text.text = string.Format(item.Item_Description, coloredStartPercent, coloredEffectPercent);
        }
        else
        {
            Description_Text.text = string.Format(item.Item_Description);
        }


        if (item.rarity == Rarity.Legendary)
        {
            Legendary_Particle.gameObject.SetActive(true);
        }
        else
        {
            Legendary_Particle.gameObject.SetActive(false);
        }
    }

  
}
