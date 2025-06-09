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
    private TextMeshProUGUI Set_Effect_Amount; // ��Ʈȿ�� Ȱ��ȭ �� 
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
            // UI�ý��ۿ��� ��ũ�� ��ǥ�� Ư�� UI���
            // (RectTransform{UI ���ο��� ��ġ�� �����ϴ� �ٽ� �Ӽ�!})�� ������ǥ�� ��ȯ�ϴ� ����� ����.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform, Input.mousePosition, null, out localMousePos);

            // ���� ���θ� Ŭ���� ��� �ƹ��͵� �� ��
            if (((RectTransform)transform).rect.Contains(localMousePos))
                return;

            // ���� �ٱ��� Ŭ���ϸ� ����
            Base_Canvas.instance.item_tooltip = null;          
            Destroy(ToolTip_Background);
            Destroy(gameObject);
        }

    }

    public void Show_Status_Item_ToolTip(Vector2 screenPos)
    {
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();

        // �ǹ��� (0.5, 0.5) ����
        Rect.pivot = new Vector2(0.5f, 0.5f);

        // ��� ��ġ�� ����
        Rect.anchoredPosition = Vector2.zero;
        
    }

}



