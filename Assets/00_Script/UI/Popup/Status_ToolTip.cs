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
    private TextMeshProUGUI Base_ATK, Base_HP, Base_STR, Base_DEX, Base_VIT;
    [SerializeField]
    private TextMeshProUGUI Additional_ATK, Additional_HP, Additional_STR, Additional_DEX, Additional_VIT;   
    [SerializeField]
    private TextMeshProUGUI Weapon_Description;
    [SerializeField]
    private ParticleImage[] Legendary_Particle;

    private GameObject ToolTip_Background;
    private Status_Item_Scriptable Selected_Status_Item;

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

    public void Show_Status_Item_ToolTip(Vector2 screenPos, Status_Item_Scriptable status_item)
    {
        Selected_Status_Item = status_item;
        RectTransform canvasRect = Base_Canvas.instance.GetComponent<RectTransform>();

        // 피벗은 (0.5, 0.5) 기준
        Rect.pivot = new Vector2(0.5f, 0.5f);

        // 가운데 위치로 고정
        Rect.anchoredPosition = Vector2.zero;


        Item_Image.sprite = Utils.Get_Atlas(status_item.name);
        Item_Name_Text.text = Utils.String_Color_Rarity(status_item.rarity) + status_item.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(status_item.rarity) + status_item.KO_rarity.ToString();
        Item_Level_Text.text = status_item.Item_Level.ToString();
        Item_Position_Text.text = status_item.Position.ToString();
        Item_Enhance_Level_Text.text = $"{Utils.String_Color_Rarity(status_item.rarity) + "+"} {Utils.String_Color_Rarity(status_item.rarity) + Base_Manager.Data.Status_Item_Holder[status_item.name].Enhancement.ToString()}";

        Base_ATK.text = $"공격력 : {status_item.Base_ATK.ToString()}";
        Base_HP.text = $"체력 : {status_item.Base_HP.ToString()}"; 
        Base_STR.text = $"STR : {status_item.Base_STR.ToString()}";
        Base_DEX.text = $"DEX : {status_item.Base_DEX.ToString()}";
        Base_VIT.text = $"VIT : {status_item.Base_VIT.ToString()}";

        Additional_ATK.text = $"추가 공격력 : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_ATK.ToString()}"; 
        Additional_HP.text = $"추가 체력 : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_HP.ToString()}";
        Additional_STR.text = $"추가 STR : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_STR.ToString()}";
        Additional_DEX.text = $"추가 DEX : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_DEX.ToString()}";
        Additional_VIT.text = $"추가 VIT : {Base_Manager.Data.Status_Item_Holder[status_item.name].Additional_VIT.ToString()}";

        Weapon_Description.text = status_item.Item_Description;

        switch (status_item.rarity)
        {
            case Rarity.Common:
                Legendary_Particle[0].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
                break;
            case Rarity.UnCommon:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.4f);
                break;
            case Rarity.Rare:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 1f, 0.4f);
                break;
            case Rarity.Epic:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(false);
                Legendary_Particle[3].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0f, 0.5f, 0.4f);
                break;
            case Rarity.Legendary:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(false);
                Legendary_Particle[3].gameObject.SetActive(false);
                Legendary_Particle[4].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.4f);
                break;
            case Rarity.Chaos:
                Legendary_Particle[0].gameObject.SetActive(false);
                Legendary_Particle[1].gameObject.SetActive(false);
                Legendary_Particle[2].gameObject.SetActive(false);
                Legendary_Particle[3].gameObject.SetActive(false);
                Legendary_Particle[4].gameObject.SetActive(false);
                Legendary_Particle[5].gameObject.SetActive(true);
                this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.4f);
                break;

        }
    }

    public void Increase_Rarity()
    {
        if(Selected_Status_Item != null)
        {
            if (Selected_Status_Item.rarity == Rarity.Common && Selected_Status_Item.Position == "무기")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Weapon_2"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();
            }
            if (Selected_Status_Item.rarity == Rarity.Common && Selected_Status_Item.Position == "악세사리")
            {
                Base_Manager.Data.Status_Item_Holder[Selected_Status_Item.name].Item_Amount = 0;
                Base_Manager.Data.Status_Item_Holder["Ring_2"].Item_Amount = 1;
                Destroy(this.gameObject);
                Destroy(ToolTip_Background);
                GameObject.Find("@Status").gameObject.GetComponent<UI_Status>().Init();
            }
        }
        
    }

}



