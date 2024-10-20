using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heros_Parts : MonoBehaviour
{
    [SerializeField]
    private Image M_Silder, M_character_Image, M_Rarity_Image;
    [SerializeField]
    private TextMeshProUGUI M_Level, M_Count;


    public void Init(Character_Scriptable data)
    {
        M_Rarity_Image.sprite = Utils.Get_Atlas(data.Rarity.ToString());
        M_character_Image.sprite = Utils.Get_Atlas(data.M_Character_Name);
        M_character_Image.SetNativeSize();
        
        RectTransform rect = M_character_Image.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.5f, rect.sizeDelta.y / 2.5f);
    }

}
