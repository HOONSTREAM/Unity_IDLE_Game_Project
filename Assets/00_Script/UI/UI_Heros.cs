using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    #region 영웅배치패널
    public Image[] Main_Set_Panel_Hero_Image;
    public GameObject[] Legendary_Particle;
    public TextMeshProUGUI[] Main_Set_Hero_Name;
    public GameObject[] Hero_Set_Buttons;
    private Coroutine flickerCoroutine;
    private bool isFlickering = false;
    private Color normalColor = Color.white;
    private Color alertColor = Color.red;
    #endregion

    #region Hero_Infomation
    [Space(20f)]
    [Header("Hero_Information")]
    [Space(20f)]
    [SerializeField]
    private GameObject Hero_Information;
    [SerializeField]
    private GameObject Hero_Guide_Infomation;
    [SerializeField]
    private TextMeshProUGUI Hero_Name_Text, Rarity_Text, Description_Text;
    [SerializeField]
    private TextMeshProUGUI Ability, ATK, HP;
    [SerializeField]
    private TextMeshProUGUI Level_Text, Slider_Count_Text;
    [SerializeField]
    private TextMeshProUGUI Holding_Effect_First, Holding_Effect_Second, Holding_Effect_Third;
    [SerializeField]
    private TextMeshProUGUI Holding_Effect_Amount_First, Holding_Effect_Amount_Second, Holding_Effect_Amount_Third;
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
    [SerializeField]
    private TextMeshProUGUI Summon_Button_Text;


    private bool is_Hero_inBattle = false;
    private UI_Heros_Parts Clicked_Heros_Parts;
    #endregion

    public override bool Init()
    {
        // 영웅배치패널 초기화

        Main_Set_Hero_Panel();

        Main_UI.Instance.FadeInOut(true, true, null);

        var Data = Base_Manager.Data.Data_Character_Dictionary;

        foreach (var data in Data)
        {
            _dict.Add(data.Value.Data.Character_EN_Name, data.Value.Data);
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

        User_Hero_Amount.text = $"{value} / <color=#FFFF00>{Base_Manager.Data.Data_Character_Dictionary.Count - 1}</color>";
        return base.Init();
    }
    public override void DisableOBJ()
    {        
        Main_UI.Instance.Layer_Check(-1); // 버튼을 다시 원래 크기로 되돌립니다.

        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            //Base_Manager.Stage.State_Change(Stage_State.Ready); // 영웅명단이 체인지 되면 바로 반영보류
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
            StartButtonFlicker();

            for (int i = 0; i < Base_Manager.Character.Set_Character.Length; i++)
            {
                var Data = Base_Manager.Character.Set_Character[i];

                if (Data != null) // 만약 배치 버튼을 눌렀을 때, 이미 배치가 되어있다면 캐릭터 배치 해제 (배치/해제 버튼)
                {
                    if (Data.Data.Character_EN_Name == parts.Character.Character_EN_Name)
                    {
                        Base_Manager.Character.Disable_Character(i);                        
                        Initialize();
                        StopButtonFlicker();
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
    /// 영웅 UI에 영웅배치를 합니다.
    /// </summary>
    /// <param name="value"></param>
    public void Set_Character_Button(int value)
    {
        Debug.Log("Set_Character_Button실행");
        Base_Manager.Character.Get_Character(value, Character.Character_EN_Name);
        Initialize();
        Main_Set_Hero_Panel();
        StopButtonFlicker();
    }
    public void Initialize()
    {     
        Set_Click(null);    
        
        for (int i = 0; i < hero_parts.Count; i++)
        {
            hero_parts[i].Get_Character_Check();
        }

        Main_UI.Instance.Set_Character_Data();
    }
    /// <summary>
    /// 영웅창에서, 메인에 영웅을 배치 후 UI 텍스트,이미지,레전더리를 세팅합니다.
    /// </summary>
    public void Main_Set_Hero_Panel()
    {
        for (int i = 0; i < Main_Set_Panel_Hero_Image.Length; i++)
        {
            if (Base_Manager.Character.Set_Character[i] != null)
            {
                Main_Set_Panel_Hero_Image[i].sprite = Utils.Get_Atlas(Base_Manager.Character.Set_Character[i].Data.Character_EN_Name);
                Main_Set_Panel_Hero_Image[i].color = new Color(255, 255, 255, 255);
                Main_Set_Hero_Name[i].text = Base_Manager.Character.Set_Character[i].Data.M_Character_Name;

                if (Base_Manager.Character.Set_Character[i].Data.Rarity >= Rarity.Legendary)
                {
                    Legendary_Particle[i].gameObject.SetActive(true);
                }
                else
                {
                    Legendary_Particle[i].gameObject.SetActive(false);
                }
            }
            else
            {
                Main_Set_Panel_Hero_Image[i].sprite = null;
                Legendary_Particle[i].gameObject.SetActive(false);
                Main_Set_Panel_Hero_Image[i].color = new Color(255, 255, 255, 0);
                Main_Set_Hero_Name[i].text = " ";
            }
        }
    }
    public void Get_Hero_Information(Character_Scriptable Data, UI_Heros_Parts parts)
    {
        Clicked_Heros_Parts = parts;

        Summon_Button_Text.text = Clicked_Heros_Parts.Get_Character_Check() ? "해제" : "배치";


        var effects = HeroEffectFactory.Get_Holding_Effects(Data.name); // 히어로 팩토리에서 보유효과를 가져옵니다.

        Holding_Effect_Amount_First.text = (effects[0].ApplyEffect(Data) * 100).ToString("0.00"); // 직접 능력치 적용이 아닌, 퍼센트로 나타내기 위해 X100을 합니다.
        Holding_Effect_Amount_Second.text = (effects[1].ApplyEffect(Data) * 100).ToString("0.00"); //  직접 능력치 적용이 아닌, 퍼센트로 나타내기 위해 X100을 합니다.
        Holding_Effect_Amount_Third.text = (effects[2].ApplyEffect(Data) * 100).ToString("0.00"); //  직접 능력치 적용이 아닌, 퍼센트로 나타내기 위해 X100을 합니다.

        Holding_Effect_First.text = effects[0].Get_Effect_Name();
        Holding_Effect_Second.text = effects[1].Get_Effect_Name();
        Holding_Effect_Third.text = effects[2].Get_Effect_Name();

        Hero_Information.gameObject.SetActive(true);

        Legendary_Image.gameObject.SetActive(Data.Rarity >= Rarity.Legendary);

        int heroID = Hero_Enum_Mapper.GetHeroID(Data.name);

        Hero_Name_Text.text = Data.M_Character_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(Data.Rarity) + Data.KO_Rarity.ToString();
        Description_Text.text = CSV_Importer.Hero_DES_Design[heroID]["Hero_DES"].ToString();
        double atk = Base_Manager.Player.Get_ATK(Data.Rarity, Base_Manager.Data.character_Holder[Data.name], Data.name);
        double hp = Base_Manager.Player.Get_HP(Data.Rarity, Base_Manager.Data.character_Holder[Data.name]);

        ATK.text = StringMethod.ToCurrencyString(atk);
        HP.text = StringMethod.ToCurrencyString(hp);
        Ability.text = StringMethod.ToCurrencyString(atk+hp);

        Level_Text.text = "LV." + (Base_Manager.Data.character_Holder[Data.name].Hero_Level + 1).ToString();
        Slider_Count_Text.text = "(" + Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount + "/" + Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name) + ")";
        Slider_Count_Fill.fillAmount = Base_Manager.Data.character_Holder[Data.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name);
        Hero_Image.sprite = Utils.Get_Atlas(Data.name);
        Rarity_Image.sprite = Utils.Get_Atlas(Data.Rarity.ToString());

        //스킬
        
        Skill_Description.text = CSV_Importer.Hero_Skill_Design[heroID]["Skill_DES"].ToString();
        Skill_Image.sprite = Resources.Load<Sprite>(CSV_Importer.Hero_Skill_Design[heroID]["Skill_Image"].ToString());
        Skill_Name_Text.text = CSV_Importer.Hero_Skill_Design[heroID]["Skill_Name"].ToString();
        

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

        if (holder.Hero_Card_Amount >= Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name))
        {
            
            holder.Hero_Card_Amount -= Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Data.name);

            if (holder.Hero_Card_Amount == 0)
            {
                holder.Hero_Card_Amount += 1;
            }

            holder.Hero_Level++;
            Base_Manager.BACKEND.Log_Hero_Upgrade(Data, holder);
            Base_Canvas.instance.Get_TOP_Popup().Initialize("강화에 성공하여, 능력이 강화됩니다 !");
            Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        }
        else
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("영웅 강화에 필요한 카드가 부족합니다.");
        }
        
        Get_Hero_Information(Data, Clicked_Heros_Parts);

        for(int i = 0; i< hero_parts.Count; i++)
        {
            hero_parts[i].Initialize();
        }

       
        

        _ = Base_Manager.BACKEND.WriteData();
    
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

        Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        Base_Canvas.instance.Get_TOP_Popup().Initialize("강화에 성공하여, 능력이 강화됩니다 !");

        _ = Base_Manager.BACKEND.WriteData();
    }

    public void Guide_Button()
    {
        Hero_Guide_Infomation.gameObject.SetActive(true);
    }
    public void Disable_Hero_Guide_Information()
    {
        Hero_Guide_Infomation.gameObject.SetActive(false);
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

    /// <summary>
    /// 영웅정보창에서, 소환버튼을 눌러, 영웅배치를 준비합니다.
    /// Clicked_Heros_Parts는, 영웅정보창이 열리는 메서드를 통해 UI_Heros_Parts에서 this로 참조받습니다.
    /// </summary>
    public void Set_Heros_In_MainGame()
    {
        Base_Canvas.instance.Get_Toast_Popup().Initialize("버튼을 눌러 영웅을 배치하세요.");       
        Set_Click(Clicked_Heros_Parts);
        Hero_Information.gameObject.SetActive(false);
        Main_Set_Hero_Panel();
    }


    #region 배치버튼깜빡임 효과 메서드
    private void StartButtonFlicker()
    {
        if (isFlickering) return;

        isFlickering = true;
        flickerCoroutine = StartCoroutine(FlickerButtons());
    }

    private IEnumerator FlickerButtons()
    {
        float interval = 1.0f;

        while (isFlickering)
        {
            foreach (var btn in Hero_Set_Buttons)
            {
                btn.GetComponent<Image>().color = alertColor;
            }

            yield return new WaitForSeconds(interval);

            foreach (var btn in Hero_Set_Buttons)
            {
                btn.GetComponent<Image>().color = normalColor;
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private void StopButtonFlicker()
    {
        if (!isFlickering) return;

        isFlickering = false;

        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);

        // 버튼 색상 복구
        foreach (var btn in Hero_Set_Buttons)
        {
            btn.GetComponent<Image>().color = normalColor;
        }
    }
    #endregion

}
