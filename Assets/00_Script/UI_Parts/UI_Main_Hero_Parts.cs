using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인게임화면에 있는, 히어로 영웅 칸을 제어합니다.
/// </summary>
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

    public void State_Check(Player player)
    {
        FillImage.fillAmount = (float)player.MP / (float)Main_Data.MAX_MP;
        HP.text = StringMethod.ToCurrencyString(player.HP);
        MP.text = player.MP.ToString() + "/" + Main_Data.MAX_MP;
    }

}
