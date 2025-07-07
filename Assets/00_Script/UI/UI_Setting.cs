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
    private Toggle Effect_Remove_Toggle;
    [SerializeField]
    private TextMeshProUGUI Gamer_id;
    [SerializeField]
    private TextMeshProUGUI user_nick_name;
    [SerializeField]
    private GameObject Announcement_Text_UI;
    [SerializeField]
    private TMP_InputField Coupon_Input;
    [SerializeField]
    private GameObject Coupon_UI;

    private bool canSave = true; // ���̺� �Լ��� 5�ʰ������� ȣ���� �� �ֵ��� ���� (�Լ� ����ȣ�� �� ���� ���� ��� �߻� ����)

    public override bool Init()
    {
        if (PlayerPrefs.GetInt("CAM") == 1)
        {
            Camera_Shake_Toggle.isOn = true;
        }

        if(PlayerPrefs.GetInt("EFFECT") == 1)
        {
            Effect_Remove_Toggle.isOn = true;
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

    #region COUPON
    public void Coupon()
    {
        if (Coupon_Input == null || string.IsNullOrEmpty(Coupon_Input.text)) return;

        string text = Coupon_Input.text;
        Coupon_Input.text = string.Empty;

        ReceiveCoupon(text);
    }

    private void ReceiveCoupon(string couponcode)
    {
        Backend.Coupon.UseCoupon(couponcode, (callback) =>
        {
            if (!callback.IsSuccess())
            {
                FailedToReceive(callback);
                return;
            }

            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["itemObject"];

                if(jsonData.Count <= 0)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("������ �������� �����ϴ�.");
                    return;
                }

                SaveToLocal(jsonData);

            }

            catch(System.Exception e)
            {
                Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"{e}");
            }

        });
    }
    private void FailedToReceive(BackendReturnObject callback)
    {
        if(callback.GetMessage().Contains("���� ����"))
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("���� ���� ������ �����Ǿ��ų�, �Ⱓ�� ���� �� �����Դϴ�.");
            return;
        }
        else if(callback.GetMessage().Contains("�̹� ����Ͻ� ����"))
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("�ش� ������ �̹� ����ϼ̽��ϴ�.");
            return;
        }
        else
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("���� �ڵ尡 �߸��Ǿ��ų�, �̹� ��� �� �����Դϴ�.");
            return;
        }

        //Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"���� ��� �� ������ �߻��Ͽ����ϴ�. :{callback}");

    }
    private void SaveToLocal(LitJson.JsonData items)
    {       
        try
        {
            string getItems = string.Empty;

            foreach(LitJson.JsonData item in items)
            {
                int itemId = int.Parse(item["item"]["ItemId"].ToString());
                string itemName = item["item"]["ItemName"].ToString();
                string itemInfo = item["item"]["ItemInfo"].ToString();
                int itemCount = int.Parse(item["itemCount"].ToString());

                if (itemName.Equals("Dia"))
                {
                    Data_Manager.Main_Players_Data.DiaMond += itemCount;
                    Base_Manager.BACKEND.Log_Get_Dia($"Coupon_{itemCount}");
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("���� ����� �Ϸ�Ǿ����ϴ�.");
                    _ = Base_Manager.BACKEND.WriteData();                   
                }
            }
        }
        
        catch(System.Exception e)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"���� ������ ���� �� ���� �߻� : {e}");
            Debug.LogError(e);
        }
    }

    #endregion
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
        if (!canSave)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("��� �� �ٽ� �õ����ּ���.");
            return;
        }

        _ = Base_Manager.BACKEND.WriteData();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("��� ������ �Ϸ�Ǿ����ϴ�.");

        canSave = false;
        StartCoroutine(SaveCooldown());
    }

    private IEnumerator SaveCooldown()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        canSave = true;
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
    public void Effect_Remove_Button()
    {
        if (Effect_Remove_Toggle.isOn)
        {
            PlayerPrefs.SetInt("EFFECT", 1);
            Utils.is_Skill_Effect_Save_Mode = true;
        }

        else
        {
            PlayerPrefs.SetInt("EFFECT", 0);
            Utils.is_Skill_Effect_Save_Mode = false;
        }
    }
    public void Get_Announcement_UI()
    {
        Announcement_Text_UI.gameObject.SetActive(true);
    }
    public override void DisableOBJ()
    {
        PlayerPrefs.SetFloat("BGM", BGM_Volume_Slider.value);
        PlayerPrefs.SetFloat("BGS", BGS_Volume_Slider.value);
        base.DisableOBJ();
    }
}

