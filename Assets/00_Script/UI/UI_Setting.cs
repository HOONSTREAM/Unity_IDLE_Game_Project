using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Base
{
    [SerializeField]
    private Slider BGM_Volume_Slider, BGS_Volume_Slider;
    [SerializeField]
    private TextMeshProUGUI UUID;
    [SerializeField]
    private Toggle Camera_Shake_Toggle;
    [SerializeField]
    private TextMeshProUGUI Gamer_id;
    [SerializeField]
    private TextMeshProUGUI user_nick_name;


    public override bool Init()
    {
        if(PlayerPrefs.GetInt("CAM") == 1)
        {
            Camera_Shake_Toggle.isOn = true;
        }

        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        UUID.text = bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();
        Gamer_id.text = bro.GetReturnValuetoJSON()["row"]["gamerId"].ToString();
        user_nick_name.text = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        BGM_Volume_Slider.value = Base_Manager.SOUND.BGMValue;
        BGS_Volume_Slider.value = Base_Manager.SOUND.BGSValue;

        return base.Init();
    }

    private void Update()
    {
        Base_Manager.SOUND.BGMValue = BGM_Volume_Slider.value;
        Base_Manager.SOUND._audioSource[0].volume = BGM_Volume_Slider.value;
        Base_Manager.SOUND.BGSValue = BGS_Volume_Slider.value;
        Base_Manager.SOUND._audioSource[1].volume = BGS_Volume_Slider.value;
    }

    public void Get_User_Indate_UUID()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        GUIUtility.systemCopyBuffer = bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("클립보드에 UUID를 복사하였습니다.");
    }

    public void Get_User_Gamer_id()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        GUIUtility.systemCopyBuffer = bro.GetReturnValuetoJSON()["row"]["gamerId"].ToString();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("클립보드에 gamer_id를 복사하였습니다.");
    }

    public void Get_User_Nick_Name()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        GUIUtility.systemCopyBuffer = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("클립보드에 nickname를 복사하였습니다.");
    }

    public void Save_Button()
    {
        Base_Manager.BACKEND.WriteData();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("즉시 저장이 완료되었습니다.");
    }

    public void Get_Policy_URL(string url)
    {
        Application.OpenURL(url);
    }

    public void Camera_Shake_Button()
    {
        if (Camera_Shake_Toggle.isOn)
        {
            PlayerPrefs.SetInt("CAM", 1);
        }

        else
        {
            PlayerPrefs.SetInt("CAM", 0);
        }
    }

    public override void DisableOBJ()
    {
        PlayerPrefs.SetFloat("BGM", BGM_Volume_Slider.value);
        PlayerPrefs.SetFloat("BGS", BGS_Volume_Slider.value);
        base.DisableOBJ();
    }
}
