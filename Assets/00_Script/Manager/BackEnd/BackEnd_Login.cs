using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    public void StartGoogleLogin()
    {
        TheBackend.ToolKit.GoogleLogin.Android.GoogleLogin(true, GoogleLoginCallback);
    }

    private void GoogleLoginCallback(bool isSuccess, string errorMessage, string token)
    {
        if (isSuccess == false)
        {
            Debug.LogError(errorMessage);
            return;
        }

        Debug.Log("구글 토큰 : " + token);
        var bro = Backend.BMember.AuthorizeFederation(token, FederationType.Google);
        Debug.Log("페데레이션 로그인 결과 : " + bro);

        var userInfo = Backend.BMember.GetUserInfo();
        if (userInfo.IsSuccess() && userInfo.GetReturnValuetoJSON()["row"]["nickname"] != null)
        {
            // 닉네임이 존재할 경우 바로 게임 시작
            Base_Manager.BACKEND.ReadData();
            _ = Base_Manager.BACKEND.WriteData();
            Loading_Scene.instance.Main_Game_Start();

            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("BGS", 1.0f);
        }
        else
        {
            // 닉네임이 없을 경우 약관 동의 + 닉네임 입력
            GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
            go.transform.SetParent(GameObject.Find("Loading_CANVAS").transform, false);
        }

    }
    
    public void Federation_Login_AfterCheck()
    {
        BackendReturnObject userInfoBro = Backend.BMember.GetUserInfo();
        if (!userInfoBro.IsSuccess())
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"유저 정보 조회를 실패하였습니다. 관리자에게 문의해주세요.";
            return;
        }

        var nicknameJson = userInfoBro.GetReturnValuetoJSON()["row"]["nickname"];

        if (nicknameJson != null && !string.IsNullOrEmpty(nicknameJson.ToString()))
        {
            Debug.Log("닉네임 존재 → 데이터 로드 후 게임 시작");

            Base_Manager.BACKEND.ReadData();
            _ = Base_Manager.BACKEND.WriteData();
            Loading_Scene.instance.Main_Game_Start();

            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("BGS", 1.0f);
        }
        else
        {
            Debug.Log("닉네임 없음 → 정책 동의 UI 생성");

            GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
            go.transform.SetParent(GameObject.Find("Loading_CANVAS").transform, false);
        }
    }

}
