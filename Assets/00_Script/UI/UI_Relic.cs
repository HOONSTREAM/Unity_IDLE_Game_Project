using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_Relic : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Relic_Parts> relic_parts = new List<UI_Relic_Parts>();
    private Dictionary<string, Item_Scriptable> _dict = new Dictionary<string, Item_Scriptable>(); // 유물장비 저장 딕셔너리
    private Item_Scriptable Item;
    private const int RELIC_SLOT_NUMBER = 9;
    public GameObject[] Relic_Panel_Objects;

    #region Relic_Info
    [Space(20f)]
    [Header("Relic_Information")]
    [Space(20f)]
    [SerializeField]
    private GameObject Relic_Information;
    [SerializeField]
    private TextMeshProUGUI Relic_Name_Text, Rarity_Text, Description_Text;   
    [SerializeField]
    private TextMeshProUGUI Level_Text, Slider_Count_Text;
    [SerializeField]
    private TextMeshProUGUI Holding_Effect_First, Holding_Effect_Second;
    [SerializeField]
    private TextMeshProUGUI Holding_Effect_Amount_First, Holding_Effect_Amount_Second;
    [SerializeField]
    private TextMeshProUGUI Skill_Name_Text, Skill_Description; //CSV 이용
    [SerializeField]
    private Image Slider_Count_Fill;
    [SerializeField]
    private Image Rarity_Image;
    [SerializeField]
    private Image Relic_Image;
    [SerializeField]
    private Image Skill_Image;
    [SerializeField]
    private ParticleImage Legendary_Image;
    [SerializeField]
    private Button Upgrade;

    private string start_percent;
    private string effect_percent = default;

    #endregion
    public override bool Init()
    { 
        var Data = Base_Manager.Data.Data_Item_Dictionary; //모든 아이템 딕셔너리

        foreach (var data in Data)
        {
            if(data.Value.ItemType == ItemType.Equipment)
            {
                _dict.Add(data.Value.name, data.Value);

                 //TODO : 제거필요
                //Holder holder = new Holder();
                //holder.Hero_Level = 0;
                //holder.Hero_Card_Amount = 1;
                //Base_Manager.Data.Item_Holder.Add(data.Value.name, holder);
                
            }
        }


        var sort_dict = _dict.OrderByDescending(x => x.Value.rarity);


        int value = 0;


        foreach (var data in sort_dict)
        {
            if (Base_Manager.Data.Item_Holder[data.Key].Hero_Card_Amount > 0)
            {
                var go = Instantiate(Parts, Content).GetComponent<UI_Relic_Parts>();
                value++;
                relic_parts.Add(go);
                int index = value;
                go.Init(data.Value, this);
            }
        }

        for (int i = 0; i< Relic_Panel_Objects.Length; i++)
        {
            if(i == 0)
            {
                Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(false);

                break;
            }

            Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(true);
            Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(false);
            Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(false);
        }

        Get_Equip_Relic_Check();

        return base.Init();
    }
    public void Initialize()
    {

        Set_Click(null);   

        for (int i = 0; i < relic_parts.Count; i++)
        {
            relic_parts[i].Get_Item_Check();
        }

        Delegate_Holder.Clear_Event();
        Relic_Manager.instance.Initalize();
        Get_Equip_Relic_Check();
        
    }
    
    /// <summary>
    /// 장착 유물 데이터를 전체 아이템 딕셔너리에서, 유물 장착 딕셔너리에 데이터를 옮깁니다.
    /// </summary>
    /// <param name="value"></param>
    public void Set_Item_Button(int value)
    {
        if (Relic_Panel_Objects[value].transform.GetChild(0).gameObject.activeSelf)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("레벨 달성 시, 유물 칸 잠금이 해제 됩니다.");
            return;
        }

        Debug.Log("Set_Item_Button 실행");
        Base_Manager.Item.Get_Item(value, Item.name);
        Initialize();
    }

    /// <summary>
    /// 장착 유물을 검사하여, 스프라이트 및 컬러를 수정합니다.
    /// </summary>
    public void Get_Equip_Relic_Check()
    {
        for(int i = 0; i<Relic_Panel_Objects.Length; i++)
        {
            if (Base_Manager.Data.Main_Set_Item[i] != null)
            {
                Debug.Log("getitemcheck if 구문 진입");
               
                Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(true);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(true);
                Relic_Panel_Objects[i].transform.GetChild(2).GetComponent<Image>().sprite = Utils.Get_Atlas(Base_Manager.Data.Main_Set_Item[i].name);            
                Relic_Panel_Objects[i].transform.GetChild(1).GetComponent<Image>().sprite = Utils.Get_Atlas(Base_Manager.Data.Main_Set_Item[i].rarity.ToString());
            }

            else
            {
                Debug.Log("getitemcheck else 구문 진입");
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public void Set_Click(UI_Relic_Parts parts)
    {


        if (parts == null)
        {
            for (int i = 0; i < relic_parts.Count; i++)
            {
                relic_parts[i].Lock_OBJ.SetActive(false);
                relic_parts[i].GetComponent<Outline>().enabled = false;
            }
        }

        else
        {
            for (int i = 0; i < Base_Manager.Data.Main_Set_Item.Length; i++)
            {
                var Data = Base_Manager.Data.Main_Set_Item[i];
                if (Data != null)
                {
                    if (Data == parts.item)
                    {
                        Base_Manager.Item.Disable_Item(i);
                        Initialize();
                        return;
                    }
                }
            }

            Item = parts.item;

            for (int i = 0; i < relic_parts.Count; i++)
            {
                relic_parts[i].Lock_OBJ.SetActive(true);
                relic_parts[i].GetComponent<Outline>().enabled = false;
            }

            parts.Lock_OBJ.SetActive(false);
            parts.GetComponent<Outline>().enabled = true;

        }

    }

    public void Get_Relic_Information(Item_Scriptable Data)
    {
        
        switch (Data.name)
        {
            case "SWORD":

                start_percent = CSV_Importer.RELIC_SWORD_Design[Base_Manager.Data.Item_Holder[Data.name].Hero_Level]["start_percent"].ToString();
                effect_percent = CSV_Importer.RELIC_SWORD_Design[Base_Manager.Data.Item_Holder[Data.name].Hero_Level]["effect_percent"].ToString();

                break;
            case "DICE":
                start_percent = CSV_Importer.DICE_Design[Base_Manager.Data.Item_Holder[Data.name].Hero_Level]["start_percent"].ToString();
                break;
        }

        Relic_Information.gameObject.SetActive(true);

        if (Data.rarity == Rarity.Legendary)
        {
            Legendary_Image.gameObject.SetActive(true);
        }
        else
        {
            Legendary_Image.gameObject.SetActive(false);
        }

        Relic_Name_Text.text = Data.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(Data.rarity) + Data.rarity.ToString();
        Description_Text.text = string.Format(Data.Item_Description, start_percent, effect_percent);
        Level_Text.text = "LV." + (Base_Manager.Data.Item_Holder[Data.name].Hero_Level + 1).ToString();
        Slider_Count_Text.text = "(" + Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount + "/" + Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name) + ")";
        Slider_Count_Fill.fillAmount = Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name);
        Relic_Image.sprite = Utils.Get_Atlas(Data.name);
        Rarity_Image.sprite = Utils.Get_Atlas(Data.rarity.ToString());

        Upgrade.onClick.RemoveAllListeners();
        Upgrade.onClick.AddListener(() => UpGrade_Button(Base_Manager.Data.Item_Holder[Data.name], Data));
    }

    public void UpGrade_Button(Holder holder, Item_Scriptable Data)
    {
        Debug.Log("강화버튼 클릭완료. 히어로 레벨 : " + holder.Hero_Level + "카드 양 : " + holder.Hero_Card_Amount);

        if (holder.Hero_Card_Amount >= Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name))
        {
            holder.Hero_Card_Amount -= Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name);
            holder.Hero_Level++;
        }
        Get_Relic_Information(Data);

        for (int i = 0; i < relic_parts.Count; i++)
        {
            relic_parts[i].Initialize();
        }

    }

    public void Disable_Relic_Information()
    {
        Relic_Information.gameObject.SetActive(false);
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }

}
