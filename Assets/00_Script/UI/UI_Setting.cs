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

    private bool canSave = true; // 세이브 함수를 5초간격으로 호출할 수 있도록 규정 (함수 과다호출 및 과다 서버 비용 발생 방지)

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
        Base_Canvas.instance.Get_Toast_Popup().Initialize("클립보드에 UUID를 복사하였습니다.");
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
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("쿠폰에 아이템이 없습니다.");
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
        if(callback.GetMessage().Contains("전부 사용된"))
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("쿠폰 발행 개수가 소진되었거나, 기간이 만료 된 쿠폰입니다.");
            return;
        }
        else if(callback.GetMessage().Contains("이미 사용하신 쿠폰"))
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("해당 쿠폰은 이미 사용하셨습니다.");
            return;
        }
        else
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("쿠폰 코드가 잘못되었거나, 이미 사용 된 쿠폰입니다.");
            return;
        }

        //Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"쿠폰 사용 중 에러가 발생하였습니다. :{callback}");

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
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("쿠폰 사용이 완료되었습니다.");
                    _ = Base_Manager.BACKEND.WriteData();                   
                }
            }
        }
        
        catch(System.Exception e)
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize($"쿠폰 아이템 저장 중 에러 발생 : {e}");
            Debug.LogError(e);
        }
    }

    #endregion
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
        if (!canSave)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("잠시 후 다시 시도해주세요.");
            return;
        }

        _ = Base_Manager.BACKEND.WriteData();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("즉시 저장이 완료되었습니다.");

        canSave = false;
        StartCoroutine(SaveCooldown());
    }

    private IEnumerator SaveCooldown()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        canSave = true;
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

