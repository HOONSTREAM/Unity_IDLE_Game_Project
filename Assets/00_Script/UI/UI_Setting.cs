using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TheBackend.ToolKit.GoogleLogin.Android;

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
        if (PlayerPrefs.GetInt("CAM") == 1)
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

    #region User_Info_Copy
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

    #endregion
    /// <summary>
    /// 유저의 데이터를 즉시 저장합니다.
    /// </summary>
    public void Save_Button()
    {
        _ = Base_Manager.BACKEND.WriteData();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("즉시 저장이 완료되었습니다.");
    }

    /// <summary>
    /// 로그아웃을 진행합니다. 기존 토큰이 삭제되어 자동로그인이 불가능해집니다.
    /// </summary>
    public void SignOutGoogleLogin()
    {
        TheBackend.ToolKit.GoogleLogin.Android.GoogleSignOut(true,GoogleSignOutCallback);
    }
    private void GoogleSignOutCallback(bool isSuccess, string error)
    {
        if (isSuccess == false)
        {
            Debug.Log("구글 로그아웃 에러 응답 발생 : " + error);
        }
        else
        {
            Debug.Log("로그아웃 성공");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        }
    }

    /// <summary>
    /// 회원탈퇴를 진행합니다. 7일의 유예기간이 있습니다.
    /// </summary>
    public void Sign_Out_Button()
    {
        Backend.BMember.WithdrawAccount(24 * 7);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
    /// <summary>
    /// 1대1문의 웹뷰를 불러옵니다.
    /// </summary>
    public void Get_Web_Question_Button()
    {
        var bro = Backend.Question.GetQuestionAuthorize();
        string questionAuthorize = bro.GetReturnValuetoJSON()["authorize"].ToString();

#if UNITY_ANDROID
        TheBackend.ToolKit.Question.Android.OpenQuestionView(questionAuthorize, Backend.UserInDate, (error) =>
        {
            Debug.LogError("1:1 문의창 활성화중 에러가 발생했습니다 : " + error);
        });
#endif
    }


    public void Get_Policy_URL(string url)
    {
        Application.OpenURL(url);
    }

    /// <summary>
    /// 카메라 흔들림 옵션을 키고 끌 수 있도록 제어합니다.
    /// </summary>
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

