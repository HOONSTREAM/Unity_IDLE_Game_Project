using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� â���� Ȯ�� �� �� �ִ� ������ ���� ī�带 �����մϴ�.
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
        M_Rarity_Image.sprite = Utils.Get_Atlas(data.Rarity.ToString());
        M_character_Image.sprite = Utils.Get_Atlas(data.Character_EN_Name);
        M_character_Image.SetNativeSize();
        
        RectTransform rect = M_character_Image.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.5f, rect.sizeDelta.y / 2.5f);
        
        Get_Character_Check();
    }

    /// <summary>
    /// ĳ���� ��ȭ�� �̷�� �� �ڿ�, �ٷ� ����â���� ���� �� ���ֵ��� �մϴ�.
    /// </summary>
    public void Initialize()
    {
 
        M_Silder.fillAmount = (float)Base_Manager.Data.character_Holder[Character.name].Hero_Card_Amount / Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Character.name);
        M_Count.text = Base_Manager.Data.character_Holder[Character.name].Hero_Card_Amount.ToString() + "/" + Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(Character.name);
        M_Level.text = "LV." + (Base_Manager.Data.character_Holder[Character.name].Hero_Level + 1).ToString();
    }
  
    /// <summary>
    /// �������� ������ ��ġ���� ���� ����� �����մϴ�.
    /// </summary>
    public void Click_My_Hero()
    {
        parent.Get_Hero_Information(Character, this);       
    }

    /// <summary>
    /// ���� UI�� ���� ī�� �������� �˻��Ͽ�, ���������� ��Ÿ���ϴ�.
    /// </summary>
    /// <returns></returns>
    public bool Get_Character_Check()
    {
        bool Equip = false;

        for(int i = 0; i<Base_Manager.Character.Set_Character.Length; i++)
        {
            if(Base_Manager.Character.Set_Character[i] != null)
            {
                if (Base_Manager.Character.Set_Character[i].Data.Character_EN_Name == Character.Character_EN_Name) // C#������ ��ü�� ���� ��, �⺻������ �޸� �ּҸ� ���ϹǷ�,
                                                                                                                   // == �����ڴ� �⺻������ ���� ��ü(���� �޸� �ּ�)���� ���Ѵ�.
                                                                                                                   // ��, �������� ���ο� �����͸� �ҷ�����, ��ü�� ���ο� �ν��Ͻ��� �����ǹǷ�
                                                                                                                   // ���� �����͸� ���� ��ü������, ���� �ٸ� �޸� �ּҸ� �����־� == �����ڴ� false�̴�.
                {
                    Equip = true;
                }
            }
            
        }

        Eqiup_Hero_Image.gameObject.SetActive(Equip);
    
        return Equip;
    }

}
