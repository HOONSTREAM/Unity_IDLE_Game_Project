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
        Base_Canvas.instance.Get_Toast_Popup().Initialize("Ŭ�����忡 UUID�� �����Ͽ����ϴ�.");
    }

    public void Get_User_Gamer_id()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        GUIUtility.systemCopyBuffer = bro.GetReturnValuetoJSON()["row"]["gamerId"].ToString();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("Ŭ�����忡 gamer_id�� �����Ͽ����ϴ�.");
    }

    public void Get_User_Nick_Name()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        GUIUtility.systemCopyBuffer = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("Ŭ�����忡 nickname�� �����Ͽ����ϴ�.");
    }

    #endregion
    /// <summary>
    /// ������ �����͸� ��� �����մϴ�.
    /// </summary>
    public void Save_Button()
    {
        _ = Base_Manager.BACKEND.WriteData();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("��� ������ �Ϸ�Ǿ����ϴ�.");
    }

    /// <summary>
    /// �α׾ƿ��� �����մϴ�. ���� ��ū�� �����Ǿ� �ڵ��α����� �Ұ��������ϴ�.
    /// </summary>
    public void SignOutGoogleLogin()
    {
        TheBackend.ToolKit.GoogleLogin.Android.GoogleSignOut(true,GoogleSignOutCallback);
    }
    private void GoogleSignOutCallback(bool isSuccess, string error)
    {
        if (isSuccess == false)
        {
            Debug.Log("���� �α׾ƿ� ���� ���� �߻� : " + error);
        }
        else
        {
            Debug.Log("�α׾ƿ� ����");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        }
    }

    /// <summary>
    /// ȸ��Ż�� �����մϴ�. 7���� �����Ⱓ�� �ֽ��ϴ�.
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
    /// 1��1���� ���並 �ҷ��ɴϴ�.
    /// </summary>
    public void Get_Web_Question_Button()
    {
        var bro = Backend.Question.GetQuestionAuthorize();
        string questionAuthorize = bro.GetReturnValuetoJSON()["authorize"].ToString();

#if UNITY_ANDROID
        TheBackend.ToolKit.Question.Android.OpenQuestionView(questionAuthorize, Backend.UserInDate, (error) =>
        {
            Debug.LogError("1:1 ����â Ȱ��ȭ�� ������ �߻��߽��ϴ� : " + error);
        });
#endif
    }


    public void Get_Policy_URL(string url)
    {
        Application.OpenURL(url);
    }

    /// <summary>
    /// ī�޶� ��鸲 �ɼ��� Ű�� �� �� �ֵ��� �����մϴ�.
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

