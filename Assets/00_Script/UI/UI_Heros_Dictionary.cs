using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ���� ��ġ â�� �ٷ�� ��ũ��Ʈ �Դϴ�.
/// </summary>
public class UI_Heros_Dictionary : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Heros_Parts_Dictionary> hero_parts = new List<UI_Heros_Parts_Dictionary>();
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
    private TextMeshProUGUI Holding_Effect_First, Holding_Effect_Second, Holding_Effect_Third;
    [SerializeField]
    private TextMeshProUGUI Holding_Effect_Amount_First, Holding_Effect_Amount_Second, Holding_Effect_Amount_Third;
    [SerializeField]
    private TextMeshProUGUI Skill_Name_Text, Skill_Description; //CSV �̿�
    [SerializeField]
    private Image Rarity_Image;
    [SerializeField]
    private Image Hero_Image;
    [SerializeField]
    private Image Skill_Image;
    [SerializeField]
    private ParticleImage Legendary_Image;
   
 
    private UI_Heros_Parts_Dictionary Clicked_Heros_Parts;
    #endregion

    public override bool Init()
    {
 
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
            var Object = Instantiate(Parts, Content).GetComponent<UI_Heros_Parts_Dictionary>(); // Content�� �θ������Ʈ�� �ؼ� Parts�� ����
            value++;
            hero_parts.Add(Object);
            int index = value;
            Object.Init(data.Value, this);
        }

        User_Hero_Amount.text = $"<color=#FFFF00>{Base_Manager.Data.Data_Character_Dictionary.Count - 1}</color>";
        return base.Init();
    }
    public override void DisableOBJ()
    {        
        Main_UI.Instance.Layer_Check(-1); // ��ư�� �ٽ� ���� ũ��� �ǵ����ϴ�.

        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            //Base_Manager.Stage.State_Change(Stage_State.Ready); // ��������� ü���� �Ǹ� �ٷ� �ݿ�����
            base.DisableOBJ();
        });

    }

    /// <summary>
    /// �÷��̾, ���� â���� Ư�� ������ ��ġ�������� ������ �����մϴ�.
    /// </summary>
    public void Set_Click(UI_Heros_Parts_Dictionary parts)
    {
       
        if (parts == null)
        {
            for (int i = 0; i < hero_parts.Count; i++)
            {                
                hero_parts[i].GetComponent<Outline>().enabled = false;
            }
        }

        else
        {
                      
            for (int i = 0; i < hero_parts.Count; i++)
            {                
                hero_parts[i].GetComponent<Outline>().enabled = false;
            }            
            parts.GetComponent<Outline>().enabled = true;
        }

    }
   
    public void Initialize()
    {     
        Set_Click(null);            
    } 
    public void Get_Hero_Information(Character_Scriptable Data, UI_Heros_Parts_Dictionary parts)
    {
        if(Data.Character_EN_Name == "Cleric")
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("���� ���� 'Ŭ����' �Դϴ�.");
            return;
        }
        Clicked_Heros_Parts = parts;

        
        var effects = HeroEffectFactory.Get_Holding_Effects(Data.name); // ����� ���丮���� ����ȿ���� �����ɴϴ�.

        Holding_Effect_Amount_First.text = (effects[0].ApplyEffect(Data) * 100).ToString("0.00"); // ���� �ɷ�ġ ������ �ƴ�, �ۼ�Ʈ�� ��Ÿ���� ���� X100�� �մϴ�.
        Holding_Effect_Amount_Second.text = (effects[1].ApplyEffect(Data) * 100).ToString("0.00"); //  ���� �ɷ�ġ ������ �ƴ�, �ۼ�Ʈ�� ��Ÿ���� ���� X100�� �մϴ�.
        Holding_Effect_Amount_Third.text = (effects[2].ApplyEffect(Data) * 100).ToString("0.00"); //  ���� �ɷ�ġ ������ �ƴ�, �ۼ�Ʈ�� ��Ÿ���� ���� X100�� �մϴ�.

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
        double hp = Base_Manager.Player.Get_HP(Data.Rarity, Base_Manager.Data.character_Holder[Data.name], Data.name);

        ATK.text = StringMethod.ToCurrencyString(atk);
        HP.text = StringMethod.ToCurrencyString(hp);
        Ability.text = StringMethod.ToCurrencyString(atk+hp);

            
        Hero_Image.sprite = Utils.Get_Atlas(Data.name);
        Rarity_Image.sprite = Utils.Get_Atlas(Data.Rarity.ToString());

        //��ų
        
        Skill_Description.text = CSV_Importer.Hero_Skill_Design[heroID]["Skill_DES"].ToString();
        Skill_Image.sprite = Resources.Load<Sprite>(CSV_Importer.Hero_Skill_Design[heroID]["Skill_Image"].ToString());
        Skill_Name_Text.text = CSV_Importer.Hero_Skill_Design[heroID]["Skill_Name"].ToString();
        
       
    }
    public void Disable_Hero_Information()
    {
        Hero_Information.gameObject.SetActive(false);
    }

}
