using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅 창에서 확인 할 수 있는 각각의 영웅 카드를 제어합니다.
/// </summary>
public class UI_Heros_Parts_Dictionary : MonoBehaviour
{
    [SerializeField]
    private Image M_character_Image, M_Rarity_Image;
    [SerializeField]
    private TextMeshProUGUI M_Level;
    public Character_Scriptable Character;
    private UI_Heros_Dictionary parent_dictionary_Heros;


    public void Init(Character_Scriptable data, UI_Heros_Dictionary parentsBASE)
    {

        parent_dictionary_Heros = parentsBASE;

        Character = data;


        M_Level.text = "LV." + (Base_Manager.Data.character_Holder[data.name].Hero_Level + 1).ToString();
        M_character_Image.sprite = Utils.Get_Atlas(data.Character_EN_Name);
        M_Rarity_Image.sprite = Utils.Get_Atlas(data.Rarity.ToString());
        M_character_Image.SetNativeSize();

        RectTransform rect = M_character_Image.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.5f, rect.sizeDelta.y / 2.5f);

    }


    /// <summary>
    /// 캐릭터 강화가 이루어 진 뒤에, 바로 영웅창에서 적용 할 수있도록 합니다.
    /// </summary>
    public void Initialize()
    {
        M_Level.text = "LV." + (Base_Manager.Data.character_Holder[Character.name].Hero_Level + 1).ToString();
    }

    public void Click_My_Hero_Dictionary()
    {
        parent_dictionary_Heros.Get_Hero_Information(Character, this);
    }
}

   
