using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
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
    [SerializeField]
    private GameObject Get_Ready_Object;

    private Character_Scriptable Main_Data = null;

    public double LastKnownHP { get; set; } = -1;
    public int LastKnownMP { get; set; } = -1;
    public void Initialize()
    {
        Reset_Main_Hero_Parts();
    }

    /// <summary>
    /// 메인 UI 하단의 영웅 페이지의 영웅 세팅을 진행합니다.
    /// </summary>
    public void Reset_Main_Hero_Parts()
    {
        Main_Data = null;
        HP.gameObject.SetActive(false);
        FillImage.transform.parent.gameObject.SetActive(false);
        Get_Ready_Object.gameObject.SetActive(false);
        PlusIcon.gameObject.SetActive(true);
        Char_Icon.gameObject.SetActive(false);
    }

    public void Press_Plus_button_Open_Hero_Set_Popup()
    {
        Base_Canvas.instance.Get_UI("@Heros");
    }

    public void Init_Data(Character_Scriptable data, bool Ready)
    {
        Main_Data = data;
        LockIcon.gameObject.SetActive(false);
        PlusIcon.gameObject.SetActive(false);
        Char_Icon.gameObject.SetActive(true);
        Char_Icon.sprite = Utils.Get_Atlas(data.Character_EN_Name);
        HP.gameObject.SetActive(!Ready);
        FillImage.transform.parent.gameObject.SetActive(!Ready);
        Get_Ready_Object.gameObject.SetActive(Ready);       
    }

    public void State_Check(Player player)
    {   
        
        
        FillImage.fillAmount = (float)player.MP / (float)Main_Data.MAX_MP;

        if (player.HP <= 0)
        {
            player.HP = 0;
        }

        HP.text = StringMethod.ToCurrencyString(player.HP);      
        MP.text = player.MP.ToString() + "/" + Main_Data.MAX_MP;
    }

}
