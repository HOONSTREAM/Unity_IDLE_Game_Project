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
            // UI�ý��ۿ��� ��ũ�� ��ǥ�� Ư�� UI���
            // (RectTransform{UI ���ο��� ��ġ�� �����ϴ� �ٽ� �Ӽ�!})�� ������ǥ�� ��ȯ�ϴ� ����� ����.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform, Input.mousePosition, null, out localMousePos);

            // ���� ���θ� Ŭ���� ��� �ƹ��͵� �� ��
            if (((RectTransform)transform).rect.Contains(localMousePos))
                return;

            // ���� �ٱ��� Ŭ���ϸ� ����
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
            var effects = RelicEffectFactory.Get_Holding_Effects_Relic(item.name); // ���� ���丮���� ����ȿ���� �����ɴϴ�.

            if (!CSV_Importer.Relic_CSV_DATA_AUTO_Map.TryGetValue(item.name.ToUpper(), out var csvData))
            {
                Debug.LogWarning($"���� {item.name}�� CSV �����͸� ã�� �� �����ϴ�.");
                return;
            }

            int heroLevel = Base_Manager.Data.Item_Holder[item.name].Hero_Level;

            if (heroLevel < csvData.Count) // CSV ������ ���� ���� Ȯ��
            {
                start_percent = csvData[heroLevel]["start_percent"].ToString();

                if (csvData[heroLevel].TryGetValue("effect_percent", out object effectValue))
                {
                    effect_percent = effectValue.ToString();
                }
                else
                {
                    effect_percent = default;  // �⺻�� ó��
                }
            }
            else
            {
                Debug.LogWarning($"���� {item.name}�� {heroLevel} ���� �����Ͱ� �����ϴ�.");
            }

        }


        string coloredStartPercent = $"<color=#FFFF00>{start_percent}</color>";
        string coloredEffectPercent = $"<color=#FFFF00>{effect_percent}</color>";


        // ���� ��ġ ����
        Rect.anchoredPosition = pos;

        Vector2 clampedPos = pos;

        // �¿� Ŭ����
        clampedPos.x = Mathf.Clamp(
            clampedPos.x,
            -canvasSize.x / 2 + tooltipSize.x / 2,
            canvasSize.x / 2 - tooltipSize.x / 2
        );

        // ���� Ŭ����
        clampedPos.y = Mathf.Clamp(
            clampedPos.y,
            -canvasSize.y / 2 + tooltipSize.y / 2,
            canvasSize.y / 2 - tooltipSize.y / 2
        );

        // ���� ��ġ ����
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
