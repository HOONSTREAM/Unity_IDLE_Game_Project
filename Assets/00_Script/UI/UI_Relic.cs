using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Relic : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Relic_Parts> relic_parts = new List<UI_Relic_Parts>();
    private Dictionary<string, Item_Scriptable> _dict = new Dictionary<string, Item_Scriptable>(); // 유물장비 저장 딕셔너리
    private Item_Scriptable Item;
    private const int RELIC_SLOT_NUMBER = 9;
    private const int MAX_RELIC_LEVEL = 149;
    public GameObject[] Relic_Panel_Objects;


    private UI_Relic_Parts Clicked_Relic_Parts;

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
    [SerializeField]
    private TextMeshProUGUI Total_Relic_Text;

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

        Total_Relic_Text.text = $"{value} / <color=#FFFF00>{_dict.Count}</color>";

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
    /// 실제로 유물 장착을 담당합니다.
    /// </summary>
    /// <param name="value"></param>
    public void Set_Item_Button(int value)
    {
        int player_Level = (Data_Manager.Main_Players_Data.Player_Level + 1); // 플레이어 레벨 가져오기
        int require_Level = value * 30;

        if (player_Level < require_Level)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize($"{require_Level}레벨 달성 시, 유물 칸 잠금이 해제 됩니다.");
            return;
        }
        Base_Manager.Item.Get_Item(value, Item.name);
        Initialize();
        Clicked_Relic_Parts = null; // 장착 완료시 선택 된 유물 해제\
        Item = null;
    }

    /// <summary>
    /// 장착 유물을 검사하여, 스프라이트 및 컬러를 수정합니다.
    /// </summary>
    public void Get_Equip_Relic_Check()
    {
        int player_Level = (Data_Manager.Main_Players_Data.Player_Level + 1); // 플레이어 레벨 가져오기
        
        for (int i = 0; i < Relic_Panel_Objects.Length; i++)
        {
            bool isUnlocked = player_Level >= (i * 30); // 레벨 30마다 한 칸 열림 (0, 30, 60, 90 ...)

            if (isUnlocked) // 특정 레벨에 달성하여 유물 장착칸이 해금 된 경우
            {
                Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(false); // 잠금 해제
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(true);  // 유물 배경 활성화
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(true);  // 유물 이미지 활성화
            }
            else // 레벨을 달성하지 못하여 해금되지 않은 경우
            {
                Relic_Panel_Objects[i].transform.GetChild(0).gameObject.SetActive(true);  // 잠금 유지
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(false);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(false);
            }

            if (Base_Manager.Data.Main_Set_Item[i] != null) // 유물이 장착된 경우
            {
                Relic_Panel_Objects[i].transform.GetChild(1).gameObject.SetActive(true);
                Relic_Panel_Objects[i].transform.GetChild(2).gameObject.SetActive(true);
                Relic_Panel_Objects[i].transform.GetChild(2).GetComponent<Image>().sprite = Utils.Get_Atlas(Base_Manager.Data.Main_Set_Item[i].name);
                Relic_Panel_Objects[i].transform.GetChild(1).GetComponent<Image>().sprite = Utils.Get_Atlas(Base_Manager.Data.Main_Set_Item[i].rarity.ToString());
            }

            else // 장착해제했거나, 장착되지 않았을 경우
            {
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
                    if (Data.Item_Name == parts.item.Item_Name) // 아이템이 중복되었다면 배치 해제 (배치/해제 버튼)
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
            Base_Canvas.instance.Get_TOP_Popup().Initialize("빈 칸을 눌러 유물을 장착하세요.");
        }

    }
    public void Get_Relic_Information(Item_Scriptable Data, UI_Relic_Parts parts)
    {

        Clicked_Relic_Parts = parts;

        var effects = RelicEffectFactory.Get_Holding_Effects_Relic(Data.name); // 유물 팩토리에서 보유효과를 가져옵니다.

        if (!CSV_Importer.Relic_CSV_DATA_AUTO_Map.TryGetValue(Data.name.ToUpper(), out var csvData))
        {
            Debug.LogWarning($"유물 {Data.name}의 CSV 데이터를 찾을 수 없습니다.");
            return;
        }

        int heroLevel = Base_Manager.Data.Item_Holder[Data.name].Hero_Level;

        if(heroLevel >= MAX_RELIC_LEVEL) // 최종레벨 점검
        {
            Base_Manager.Data.Item_Holder[Data.name].Hero_Level = MAX_RELIC_LEVEL;
        }

        if (heroLevel < csvData.Count) // CSV 데이터 존재 여부 확인
        {
            start_percent = csvData[heroLevel]["start_percent"].ToString();

            if (csvData[heroLevel].TryGetValue("effect_percent", out object effectValue))
            {
                effect_percent = effectValue.ToString();
            }
            else
            {
                effect_percent = default;  // 기본값 처리
            }
        }
        else
        {
            Debug.LogWarning($"유물 {Data.name}의 {heroLevel} 레벨 데이터가 없습니다.");
        }

        Holding_Effect_Amount_First.text = (effects[0].ApplyEffect(Data) * 100).ToString("0.00");
        Holding_Effect_Amount_Second.text = (effects[1].ApplyEffect(Data) * 100).ToString("0.00");
        Holding_Effect_First.text = effects[0].Get_Effect_Name();
        Holding_Effect_Second.text = effects[1].Get_Effect_Name();

        Relic_Information.gameObject.SetActive(true);

        Legendary_Image.gameObject.SetActive(Data.rarity == Rarity.Legendary);

        int RelicID = Relic_Enum_Mapper.GetRelicID(Data.name);

        Relic_Name_Text.text = Data.Item_Name;
        Rarity_Text.text = Utils.String_Color_Rarity(Data.rarity) + Data.KO_rarity.ToString();
        Description_Text.text = CSV_Importer.Relic_DES_Design[RelicID]["RELIC_DES"].ToString();
        Level_Text.text = "LV." + (Base_Manager.Data.Item_Holder[Data.name].Hero_Level + 1).ToString();
        Slider_Count_Text.text = "(" + Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount + "/" + Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name) + ")";
        Slider_Count_Fill.fillAmount = Base_Manager.Data.Item_Holder[Data.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name);
        Relic_Image.sprite = Utils.Get_Atlas(Data.name);
        Rarity_Image.sprite = Utils.Get_Atlas(Data.rarity.ToString());

       
        #region 유물 별 특정 설명
        if (Data.name == "GOLD_REWARD")
        {
            var String_Value = double.Parse(effect_percent);
            Debug.Log(String_Value);
            var FormattedValue = StringMethod.ToCurrencyString(String_Value);
            Debug.Log(FormattedValue);

            Skill_Description.text = string.Format(CSV_Importer.Relic_Skill_Design[RelicID]["Skill_DES"].ToString(),
                start_percent, FormattedValue.ToString());
        }

        else if (Data.name == "GOLD_PER_ATK")
        {
            double Gold_Amount = Data_Manager.Main_Players_Data.Player_Money;
            double atkBonus = Gold_Amount / 10000000 * double.Parse(effect_percent);

            // 최대 증가량: 유물 레벨당 0.5 (즉, 50%)
            double maxBonus = (Base_Manager.Data.Item_Holder["GOLD_PER_ATK"].Hero_Level + 1) * 0.5;

            if (atkBonus >= maxBonus)
            {
                atkBonus = maxBonus;
            }

            Skill_Description.text = string.Format(
                CSV_Importer.Relic_Skill_Design[RelicID]["Skill_DES"].ToString(),
                start_percent,
                effect_percent,
                (atkBonus * 100).ToString("F2")  // % 단위로 표시
            );
        }
        #endregion

        else
        {
            Skill_Description.text = string.Format(CSV_Importer.Relic_Skill_Design[RelicID]["Skill_DES"].ToString(), start_percent, effect_percent);
        }

        Skill_Image.sprite = Resources.Load<Sprite>(CSV_Importer.Relic_Skill_Design[RelicID]["Skill_Image"].ToString());
        Skill_Name_Text.text = CSV_Importer.Relic_Skill_Design[RelicID]["Skill_Name"].ToString();


        Upgrade.onClick.RemoveAllListeners();
        Upgrade.onClick.AddListener(() => UpGrade_Button(Base_Manager.Data.Item_Holder[Data.name], Data));
    }
    public void Set_Relic_In_MainGame()
    {       
        Set_Click(Clicked_Relic_Parts);
        Relic_Information.gameObject.SetActive(false);
    }  
    public void UpGrade_Button(Holder holder, Item_Scriptable Data)
    {
        if(holder.Hero_Level >= MAX_RELIC_LEVEL)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("최고 레벨입니다.");
            return;
        }

        if (holder.Hero_Card_Amount >= Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name))
        {
            holder.Hero_Card_Amount -= Utils.Data.heroCardData.Get_LEVELUP_Relic_Card_Amount(Data.name);
            holder.Hero_Level++;

            if (holder.Hero_Card_Amount == 0)
            {
                holder.Hero_Card_Amount += 1;
                
            }

            Base_Canvas.instance.Get_TOP_Popup().Initialize("강화에 성공하여, 능력이 강화됩니다 !");

            if(Data.rarity >= Rarity.Epic)
            {
                Utils.SendSystemLikeMessage($"★ <color=#FFFF00>유물 [{Data.Item_Name}]</color>을(를) 강화했습니다!");
            }
            
            Base_Manager.SOUND.Play(Sound.BGS, "Victory");
            Base_Manager.Player.MarkRelicEffectDirty(); // 기존 유물 보유효과 재 캐싱
        }
        Get_Relic_Information(Data, Clicked_Relic_Parts);

        for (int i = 0; i < relic_parts.Count; i++)
        {
            relic_parts[i].Initialize();
        }

       
       

        _ = Base_Manager.BACKEND.WriteData();

    }
    public void Disable_Relic_Information()
    {
        Clicked_Relic_Parts = null;
        Item = null;
        Relic_Information.gameObject.SetActive(false);
    }
    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }

}
