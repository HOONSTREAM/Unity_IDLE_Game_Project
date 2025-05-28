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

 
    public void Show_Item_ToolTip(Item_Scriptable item, Vector2 screenPos)
    {
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();
        Canvas canvas = canvasRect.GetComponent<Canvas>();

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPos);

        Vector2 tooltipSize = Rect.sizeDelta;
        Vector2 canvasSize = canvasRect.rect.size;

        // 툴팁 오른쪽에 표시
        localPos += new Vector2(20f, 0);

        // 오른쪽 넘치면 왼쪽에 표시
        if (localPos.x + tooltipSize.x > canvasSize.x / 2)
        {
            Rect.pivot = new Vector2(1, 0); // 오른쪽 하단 기준으로 변경
            localPos.x -= 40f;
        }
        else
        {
            Rect.pivot = new Vector2(0, 0); // 왼쪽 하단 기준
        }

        // 수직 클램핑
        localPos.y = Mathf.Clamp(
            localPos.y,
            -canvasSize.y / 2 + tooltipSize.y,
            canvasSize.y / 2 - tooltipSize.y);

        Rect.localPosition = localPos;

        // ---- 이하 기존 데이터 표시 로직 유지 ----

        if (item.ItemType == ItemType.Equipment)
        {
            var effects = RelicEffectFactory.Get_Holding_Effects_Relic(item.name);

            if (!CSV_Importer.Relic_CSV_DATA_AUTO_Map.TryGetValue(item.name.ToUpper(), out var csvData))
            {
                Debug.LogWarning($"유물 {item.name}의 CSV 데이터를 찾을 수 없습니다.");
                return;
            }

            int heroLevel = Base_Manager.Data.Item_Holder[item.name].Hero_Level;

            if (heroLevel < csvData.Count)
            {
                start_percent = csvData[heroLevel]["start_percent"].ToString();

                if (csvData[heroLevel].TryGetValue("effect_percent", out object effectValue))
                {
                    effect_percent = effectValue.ToString();
                }
                else
                {
                    effect_percent = default;
                }
            }
            else
            {
                Debug.LogWarning($"유물 {item.name}의 {heroLevel} 레벨 데이터가 없습니다.");
            }
        }

        string coloredStartPercent = $"<color=#FFFF00>{start_percent}</color>";
        string coloredEffectPercent = $"<color=#FFFF00>{effect_percent}</color>";

        Item_Image.sprite = Utils.Get_Atlas(item.name);
        Item_Name_Text.text = item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(item.rarity) + item.KO_rarity.ToString();

        if (item.ItemType == ItemType.Equipment)
        {
            Description_Text.text = string.Format(item.Item_Description, coloredStartPercent, coloredEffectPercent);
        }
        else
        {
            Description_Text.text = string.Format(item.Item_Description);
        }

        Legendary_Particle.gameObject.SetActive(item.rarity == Rarity.Legendary);
    }

}

  

