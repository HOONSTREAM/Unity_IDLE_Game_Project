using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅 창에서 확인 할 수 있는 각각의 영웅 카드를 제어합니다.
/// </summary>
public class UI_Heros_Parts : MonoBehaviour
{
    [SerializeField]
    private Image M_Silder, M_character_Image, M_Rarity_Image;
    [SerializeField]
    private TextMeshProUGUI M_Level, M_Count;
    [SerializeField]
    private GameObject Eqiup_Hero_Image;
    [SerializeField]
    private Button OnClickButton;
    [SerializeField]
    private GameObject Plus_Button, Minus_Button;   


    public GameObject Lock_OBJ;
    public Character_Scriptable Character;
    private UI_Heros parent;
    
    public void LockCheck(bool Lock)
    {
        switch (Lock)
        {
            case true:
                Lock_OBJ.SetActive(true);
                Plus_Button.SetActive(false);
                Minus_Button.SetActive(true);
                break;
            case false:
                Lock_OBJ.SetActive(false);
                Plus_Button.SetActive(true);
                Minus_Button.SetActive(false);
                break;

        }
    }
    public void Init(Character_Scriptable data, UI_Heros parentsBASE)
    {
        parent = parentsBASE; 
        Character = data;

        
        
        M_Level.text = "LV." + (Base_Manager.Data.character_Holder[data.name].Hero_Level + 1).ToString();
        M_Silder.fillAmount = (float)Base_Manager.Data.character_Holder[data.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(data.name);
        M_Count.text = Base_Manager.Data.character_Holder[data.name].Hero_Card_Amount.ToString() + "/" + Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(data.name);
        Debug.Log($"계산된 {data.name} 레벨업 필요량 카드 : " + Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(data.name));
        M_Rarity_Image.sprite = Utils.Get_Atlas(data.Rarity.ToString());
        M_character_Image.sprite = Utils.Get_Atlas(data.M_Character_Name);
        M_character_Image.SetNativeSize();
        
        RectTransform rect = M_character_Image.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.5f, rect.sizeDelta.y / 2.5f);

        Get_Character_Check();
    }

    /// <summary>
    /// 캐릭터 강화가 이루어 진 뒤에, 바로 영웅창에서 적용 할 수있도록 합니다.
    /// </summary>
    public void Initialize()
    {
 
        M_Silder.fillAmount = (float)Base_Manager.Data.character_Holder[Character.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Character.name);
        M_Count.text = Base_Manager.Data.character_Holder[Character.name].Hero_Card_Amount.ToString() + "/" + Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Character.name);
        M_Level.text = "LV." + (Base_Manager.Data.character_Holder[Character.name].Hero_Level + 1).ToString();
    }
    
    /// <summary>
    /// 영웅 카드 우측상단의 탭을 누르면 활성화 되는 로직을 처리합니다.
    /// </summary>
    public void Click_My_Button()
    {    
        Render_Manager.instance.HERO.Get_Particle(true);
        parent.Set_Click(this);
    }
    /// <summary>
    /// 보유중인 영웅을 터치했을 때의 기능을 구현합니다.
    /// </summary>
    public void Click_My_Hero()
    {
        parent.Get_Hero_Information(Character);
        Render_Manager.instance.HERO.Get_Particle(true);
    }
    public void Get_Character_Check()
    {
        bool Equip = false;

        for(int i = 0; i<Base_Manager.Character.Set_Character.Length; i++)
        {
            if(Base_Manager.Character.Set_Character[i] != null)
            {
                if (Base_Manager.Character.Set_Character[i].Data == Character)
                {
                    Equip = true;
                }
            }
            
        }

        Eqiup_Hero_Image.gameObject.SetActive(Equip);
        Minus_Button.gameObject.SetActive(Equip);
        Plus_Button.gameObject.SetActive(!Equip);

    }

}
