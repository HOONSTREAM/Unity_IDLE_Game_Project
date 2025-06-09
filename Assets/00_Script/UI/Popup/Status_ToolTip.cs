using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Status_ToolTip : MonoBehaviour
{

    private RectTransform Rect;
    [SerializeField]
    private Image Item_Image;
    [SerializeField]
    private TextMeshProUGUI Item_Name_Text, Rarity_Text, Item_Level_Text, Item_Position_Text, Item_Enhance_Level_Text;
    [SerializeField]
    private TextMeshProUGUI Base_ATK, Base_HP, Base_STR, Base_DEX, Base_INT;
    [SerializeField]
    private TextMeshProUGUI Addtional_ATK, Addtional_HP, BasAddtional_DEX, Addtional_INT;
    [SerializeField]
    private TextMeshProUGUI Set_Effect_Amount; // 세트효과 활성화 수 
    [SerializeField]
    private TextMeshProUGUI Set_Effect_Weapon, Set_Effect_ACC, Set_Effect_Description;
    [SerializeField]
    private TextMeshProUGUI Weapon_Description;
    [SerializeField]
    private ParticleImage[] Legendary_Particle;

    private GameObject ToolTip_Background;

    private string start_percent;
    private string effect_percent = default;

    private void Awake()
    {
        Rect = this.GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        int tooltipIndex = transform.GetSiblingIndex();
        ToolTip_Background = Instantiate(Resources.Load<GameObject>("UI/ToolTip_Background"), Base_Canvas.instance.transform);
        ToolTip_Background.transform.SetSiblingIndex(tooltipIndex - 1);
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
            Destroy(ToolTip_Background);
            Destroy(gameObject);
        }

    }

    public void Show_Status_Item_ToolTip(Vector2 screenPos)
    {
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();

        // 피벗은 (0.5, 0.5) 기준
        Rect.pivot = new Vector2(0.5f, 0.5f);

        // 가운데 위치로 고정
        Rect.anchoredPosition = Vector2.zero;
        
    }

}



