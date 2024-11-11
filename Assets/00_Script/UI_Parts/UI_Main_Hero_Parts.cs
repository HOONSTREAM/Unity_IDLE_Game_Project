using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_Hero_Parts : MonoBehaviour
{
    [SerializeField]
    private GameObject LockIcon, PlusIcon;
    [SerializeField]
    private Image Char_Icon, FillImage;
    [SerializeField]
    private TextMeshProUGUI HP, MP;

    [Space(20f)]
    [SerializeField]
    private int Value;
    private Character_Scriptable Main_Data;


    public void Init_Data(Character_Scriptable data)
    {
        Main_Data = data;
        LockIcon.gameObject.SetActive(false);
        PlusIcon.gameObject.SetActive(false);
        Char_Icon.gameObject.SetActive(true);
        Char_Icon.sprite = Utils.Get_Atlas(data.M_Character_Name);
    }
}
