using BackEnd;
using PimDeWitte.UnityMainThreadDispatcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    private bool isGoogleLoginRunning = false;

    public void StartGoogleLogin()
    {
        if (isGoogleLoginRunning)
        {           
            return;
        }

        isGoogleLoginRunning = true;

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
        isGoogleLoginRunning = false; 

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
            if (!bro.IsSuccess())
            {
                Utils.Get_LoadingCanvas_ErrorUI(bro.ToString());
                return;
            }

            var userInfo = Backend.BMember.GetUserInfo();
            if (userInfo.IsSuccess() && userInfo.GetReturnValuetoJSON()["row"]["nickname"] != null)
            {
                // 닉네임 존재 → 데이터 로드 안전 시작
                CheckUserTableAndStart();
            }
            else
            {
                // 닉네임 없음 → 약관 동의 UI
                ShowPolicyUI_Delayed();
            }
        }

    }

    private void CheckUserTableAndStart()
    {
        Backend.GameData.Get("USER", new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("USER 테이블 조회 실패. 로그인 다시 시도 필요.");
                Utils.Get_LoadingCanvas_ErrorUI("서버 세션 준비 실패. 잠시 후 다시 시도해주세요.");
                return;
            }

            // USER 테이블 확인 성공 → 데이터 읽고 메인 시작
            Base_Manager.BACKEND.ReadData();

            StartMainGame();
        });
    }

    private void StartMainGame()
    {
        Loading_Scene.instance.Main_Game_Start();
        PlayerPrefs.SetFloat("BGM", 1.0f);
        PlayerPrefs.SetFloat("BGS", 1.0f);
    }

    public void ShowPolicyUI_Delayed()
    {
        StartCoroutine(DelayedPolicyUI());
    }

    private IEnumerator DelayedPolicyUI()
    {
        yield return null; // 또는 new WaitForEndOfFrame()

        GameObject prefab = Resources.Load<GameObject>("UI/LOGIN_UI_POLICY");
        if (prefab == null)
        {
            Debug.LogError("LOGIN_UI_POLICY 리소스를 찾을 수 없습니다.");
            yield break;
        }

        GameObject go = Instantiate(prefab);

        var canvas = GameObject.Find("Loading_CANVAS");
        if (canvas != null)
        {
            go.transform.SetParent(canvas.transform, false);
        }
        else
        {
            Debug.LogWarning("Loading_CANVAS가 존재하지 않습니다.");
        }
    }


}
       

    
    


