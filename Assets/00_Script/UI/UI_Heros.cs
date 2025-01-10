using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;
using static UnityEditor.Progress;

/// <summary>
/// 영웅 배치 창을 다루는 스크립트 입니다.
/// </summary>
public class UI_Heros : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Heros_Parts> hero_parts = new List<UI_Heros_Parts>();
    private Dictionary<string, Character_Scriptable> _dict = new Dictionary<string, Character_Scriptable>();
    private Character_Scriptable Character;
    [SerializeField]
    private TextMeshProUGUI User_Hero_Amount;

    #region Hero_Infomation
    [Space(20f)]
    [Header("Hero_Information")]
    [Space(20f)]
    [SerializeField]
    private GameObject Hero_Information;
    [SerializeField]
    private TextMeshProUGUI Hero_Name_Text, Rarity_Text, Description_Text;
    [SerializeField]
    private TextMeshProUGUI Ability, ATK, HP;
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
    private Image Hero_Image;
    [SerializeField]
    private Image Skill_Image;
    [SerializeField]
    private ParticleImage Legendary_Image;
    [SerializeField]
    private Button Upgrade;
    #endregion

    public override bool Init()
    {

        InitButtons();
        Render_Manager.instance.HERO.Init_Hero();

        Main_UI.Instance.FadeInOut(true, true, null);


        var Data = Base_Manager.Data.Data_Character_Dictionary;

        foreach (var data in Data)
        {
            _dict.Add(data.Value.Data.M_Character_Name, data.Value.Data);
        }
      

        var sort_dict = _dict.OrderByDescending(x => x.Value.Rarity);


        int value = 0;


        foreach (var data in sort_dict)
        {
            if (Base_Manager.Data.character_Holder[data.Key].Hero_Card_Amount > 0)
            {
                var Object = Instantiate(Parts, Content).GetComponent<UI_Heros_Parts>(); // Content를 부모오브젝트로 해서 Parts를 생성
                value++;
                hero_parts.Add(Object);
                int index = value;
                Object.Init(data.Value, this);
            }
        }

        User_Hero_Amount.text = value.ToString();
        return base.Init();
    }
    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1); // 버튼을 다시 원래 크기로 되돌립니다.

        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });

    }

    /// <summary>
    /// 플레이어가, 영웅 창에서 특정 영웅을 터치했을때의 동작을 정의합니다.
    /// </summary>
    public void Set_Click(UI_Heros_Parts parts)
    {
       
        if (parts == null)
        {
            for (int i = 0; i < hero_parts.Count; i++)
            {
                hero_parts[i].Lock_OBJ.SetActive(false);
                hero_parts[i].GetComponent<Outline>().enabled = false;
            }
        }

        else
        {
            for (int i = 0; i < Base_Manager.Character.Set_Character.Length; i++)
            {
                var Data = Base_Manager.Character.Set_Character[i];
                if (Data != null)
                {
                    if (Data.Data == parts.Character)
                    {
                        Base_Manager.Character.Disable_Character(i);
                        Initialize();
                        return; 
                    }
                }
            }

            Character = parts.Character;

            for (int i = 0; i < hero_parts.Count; i++)
            {
                hero_parts[i].Lock_OBJ.SetActive(true);
                hero_parts[i].GetComponent<Outline>().enabled = false;
            }

            parts.Lock_OBJ.SetActive(false);
            parts.GetComponent<Outline>().enabled = true;

        }

    }
    /// <summary>
    /// 영웅을 클릭 후, 플러스 버튼이 생성되면, 영웅을 등록하기 위해, 플러스 버튼 위에 보이지 않는 가상의 버튼을 설정합니다.
    /// </summary>
    public void InitButtons()
    {
        for(int i = 0; i<Render_Manager.instance.HERO.Circles.Length; i++)
        {
            int index = i;
            var go = new GameObject("Button").AddComponent<Button>();
            go.onClick.AddListener(()=> Set_Character_Button(index));

            go.transform.SetParent(this.transform); // 해당 오브젝트를 UI_Heros 팝업 하단에 자식오브젝트로 설정
            go.gameObject.AddComponent<Image>();
            //go.gameObject.AddComponent<RectTransform>(); 버튼에는 이미 RectTransform이 붙어있음.
            
            RectTransform rect = go.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            rect.sizeDelta = new Vector2(150.0f, 150.0f);
            go.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.01f);

            go.transform.position = Render_Manager.instance.ReturnScreenPoint(Render_Manager.instance.HERO.Circles[i]);        
        }
        
    }
    public void Set_Character_Button(int value)
    {
        Base_Manager.Character.Get_Character(value, Character.M_Character_Name);
        Initialize();
    }
    public void Initialize()
    {
        Render_Manager.instance.HERO.Get_Particle(false);
        Set_Click(null);
        Render_Manager.instance.HERO.Init_Hero();

        for (int i = 0; i < hero_parts.Count; i++)
        {
            hero_parts[i].Get_Character_Check();
        }

        Main_UI.Instance.Set_Character_Data();
    }
    public void Get_Hero_Information(Character_Scriptable Data)
    {
        Hero_Name name = Hero_Name.Dual_Blader;

        switch (Data.name)
        {
            case "Dual_Blader":
                name = Hero_Name.Dual_Blader;
                Holding_Effect_Amount_First.text = Utils.Data.Dual_Effect_Data.Get_ALL_ATK(Data).ToString("0.00");
                Holding_Effect_Amount_Second.text = "0";
                Holding_Effect_First.text = "아군 전체 물리공격력";
                Holding_Effect_Second.text = "아이템 드랍확률";
                break;
            case "Hunter":
                name = Hero_Name.Hunter;
                break;
            case "Elemental_Master_White":
                name = Hero_Name.Elemental_Master_White;
                break;
            case "PalaDin":
                name = Hero_Name.PalaDin;
                Holding_Effect_Amount_First.text = Utils.Data.PalaDin_Effect_Data.Get_ALL_ATK(Data).ToString("0.00");
                Holding_Effect_Amount_Second.text = "0";
                Holding_Effect_First.text = "아군 전체 물리공격력";
                Holding_Effect_Second.text = "아이템 드랍확률";              
                break;


        }

        Hero_Information.gameObject.SetActive(true);

        if(Data.Rarity == Rarity.Legendary)
        {
            Legendary_Image.gameObject.SetActive(true);
        }
        else
        {
            Legendary_Image.gameObject.SetActive(false);
        }

        Hero_Name_Text.text = Data.M_Character_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(Data.Rarity) + Data.Rarity.ToString();
        Description_Text.text = CSV_Importer.Hero_DES_Design[(int)name]["Hero_DES"].ToString();
        double atk = Base_Manager.Player.Get_ATK(Data.Rarity, Base_Manager.Data.character_Holder[Data.name]);
        double hp = Base_Manager.Player.Get_HP(Data.Rarity, Base_Manager.Data.character_Holder[Data.name]);

        ATK.text = StringMethod.ToCurrencyString(atk);
        HP.text = StringMethod.ToCurrencyString(hp);
        Ability.text = string.Format("{0:0}", ((int)atk + hp));

        Level_Text.text = "LV." + (Base_Manager.Data.character_Holder[Data.name].Hero_Level + 1).ToString();
        Slider_Count_Text.text = "(" + Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount + "/" + Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name) + ")";
        Slider_Count_Fill.fillAmount = Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name);
        Hero_Image.sprite = Utils.Get_Atlas(Data.name);
        Rarity_Image.sprite = Utils.Get_Atlas(Data.Rarity.ToString());

        //스킬
        
        Skill_Description.text = CSV_Importer.Hero_Skill_Design[(int)name]["Skill_DES"].ToString();
        Skill_Image.sprite = Resources.Load<Sprite>(CSV_Importer.Hero_Skill_Design[(int)name]["Skill_Image"].ToString());
        Skill_Name_Text.text = CSV_Importer.Hero_Skill_Design[(int)name]["Skill_Name"].ToString();
        

        Upgrade.onClick.RemoveAllListeners();
        Upgrade.onClick.AddListener(() => UpGrade_Button((Base_Manager.Data.character_Holder[Data.name]),Data));
    }
    public void Disable_Hero_Information()
    {
        Hero_Information.gameObject.SetActive(false);
    }
    /// <summary>
    /// 캐릭터 카드 갯수를 초과하여, 강화하는 로직을 구현합니다.
    /// </summary>
    public void UpGrade_Button(Holder holder, Character_Scriptable Data)
    {     
        Debug.Log("강화버튼 클릭완료. 히어로 레벨 : " + holder.Hero_Level + "카드 양 : " + holder.Hero_Card_Amount);

        if (holder.Hero_Card_Amount >= Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name))
        {
            holder.Hero_Card_Amount -= Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name);
            holder.Hero_Level++;
        }
        Get_Hero_Information(Data);

        for(int i = 0; i< hero_parts.Count; i++)
        {
            hero_parts[i].Initialize();
        }
    
    }
    
    /// <summary>
    /// 영웅 배치화면에서, 일괄강화의 로직을 구현합니다.
    /// </summary>
    public void All_UpGrade_Button()
    {
        if(Get_All_Upgrade() == false)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("레벨업이 가능한 영웅이 없습니다.");
            
            return;
        }

        Base_Canvas.instance.Get_UI("UpGrade_Panel");
        Utils.UI_Holder.Peek().GetComponent<UI_Upgrade>().Initialize(this);
    }

    /// <summary>
    /// 소유중인 모든 영웅을 순회하여 레벨업이 가능한 영웅들이 있는지 검사하고, 하나라도 있으면 true를 반환합니다.
    /// </summary>
    /// <returns></returns>
    private bool Get_All_Upgrade()
    {
        bool Can_Upgrade = false;

        foreach(var Data in Base_Manager.Data.Data_Character_Dictionary)
        {
            if (Data.Value.holder.Hero_Card_Amount >= Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.Value.Data.name))
            {
                Can_Upgrade = true;

            }
        }

        return Can_Upgrade;
    }

}
