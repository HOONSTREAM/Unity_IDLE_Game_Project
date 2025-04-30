using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    public void StartGoogleLogin()
    {
        TheBackend.ToolKit.GoogleLogin.Android.GoogleLogin(GoogleLoginCallback);
    }

    public void Test_Custom_Login()
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp("user1", "1234");
        if (bro.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다");
        }

        BackendReturnObject bros = Backend.BMember.CustomLogin("user1", "1234");
        if (bros.IsSuccess())
        {
            Debug.Log("로그인에 성공했습니다");

            var userInfo = Backend.BMember.GetUserInfo();

            if (userInfo.IsSuccess() && userInfo.GetReturnValuetoJSON()["row"]["nickname"] != null)
            {
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


    }

    private void GoogleLoginCallback(bool isSuccess, string errorMessage, string token)
    {
        if (isSuccess == false)
        {
            Debug.LogError(errorMessage);
            Utils.Get_LoadingCanvas_ErrorUI($"로그인에 실패하였습니다 : {errorMessage}.");
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().TapToStart_Object.gameObject.SetActive(true);
            return;
        }

        Debug.Log("구글 토큰 : " + token);

        var bro = Backend.BMember.AuthorizeFederation(token, FederationType.Google); // 동기
        {
            if (bro.IsSuccess())
            {
                var userInfo = Backend.BMember.GetUserInfo();

                if (userInfo.IsSuccess() && userInfo.GetReturnValuetoJSON()["row"]["nickname"] != null)
                {
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

            else
            {
                Utils.Get_LoadingCanvas_ErrorUI($"{bro.ToString()}");
            }
        }
    }
}
       

    
    


