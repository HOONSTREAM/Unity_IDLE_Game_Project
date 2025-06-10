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
    private TextMeshProUGUI Additional_ATK, Additional_HP, Additional_STR, Additional_DEX, Additional_INT;
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

    public void Show_Status_Item_ToolTip(Vector2 screenPos, Status_Item_Scriptable status_item)
    {
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();

        // �ǹ��� (0.5, 0.5) ����
        Rect.pivot = new Vector2(0.5f, 0.5f);

        // ��� ��ġ�� ����
        Rect.anchoredPosition = Vector2.zero;


        Item_Image.sprite = Utils.Get_Atlas(status_item.name);
        Item_Name_Text.text = Utils.String_Color_Rarity(status_item.rarity) + status_item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(status_item.rarity) + status_item.KO_rarity.ToString();
        Item_Level_Text.text = status_item.Item_Level.ToString();
        Item_Position_Text.text = status_item.Position.ToString();
        Item_Enhance_Level_Text.text = $"{Utils.String_Color_Rarity(status_item.rarity) + "+"} {Utils.String_Color_Rarity(status_item.rarity) + Base_Manager.Data.Status_Item_Holder[status_item.name].Enhancement.ToString()}";

        Base_ATK.text = $"���ݷ� : {status_item.Base_ATK.ToString()}";
        Base_HP.text = $"ü�� : {status_item.Base_HP.ToString()}"; 
        Base_STR.text = $"STR : {status_item.Base_STR.ToString()}";
        Base_DEX.text = $"DEX : {status_item.Base_DEX.ToString()}";
        Base_INT.text = $"INT : {status_item.Base_INT.ToString()}";

        Additional_ATK.text = $"�߰� ���ݷ� : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_ATK.ToString()}"; 
        Additional_HP.text = $"�߰� ü�� : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_HP.ToString()}";
        Additional_STR.text = $"�߰� STR : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_STR.ToString()}";
        Additional_DEX.text = $"�߰� DEX : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_DEX.ToString()}";
        Additional_INT.text = $"�߰� INT : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_INT.ToString()}";

        Set_Effect_Amount.text = $"��Ʈ ȿ�� : ( {Base_Manager.Data.Status_Item_Holder[status_item.name].Set_Effect_Amount.ToString()} / 2 )"; 
        Set_Effect_Weapon.text = status_item.Set_Effect_Weapon_Name;
        Set_Effect_ACC.text = status_item.Set_Effect_ACC_Name;
        Set_Effect_Description.text = status_item.Set_Effect_Description;
        Weapon_Description.text = status_item.Item_Description;

        switch (status_item.rarity)
        {
            case Rarity.Common:
                Legendary_Particle[0].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                break;
            case Rarity.UnCommon:
                Legendary_Particle[1].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
                break;
            case Rarity.Rare:
                Legendary_Particle[2].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
                break;
            case Rarity.Epic:
                Legendary_Particle[3].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0f, 0.5f, 1f);
                break;
            case Rarity.Legendary:
                Legendary_Particle[4].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
                break;
            case Rarity.Chaos:
                Legendary_Particle[5].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                break;

        }
    }

}



