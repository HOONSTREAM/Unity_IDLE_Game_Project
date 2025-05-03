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
            if (!bro.IsSuccess())
            {
                Utils.Get_LoadingCanvas_ErrorUI(bro.ToString());
                return;
            }

            var userInfo = Backend.BMember.GetUserInfo();
            if (userInfo.IsSuccess() && userInfo.GetReturnValuetoJSON()["row"]["nickname"] != null)
            {
                // 닉네임 존재 → 데이터 로드 안전 시작
                StartCoroutine(SafeReadDataAndStartGame());
            }
            else
            {
                // 닉네임 없음 → 약관 동의 UI
                StartCoroutine(LoadLoginPolicyUI());
            }
        }
    }

    private IEnumerator LoadLoginPolicyUI()
    {
        yield return null; // 다음 프레임까지 기다림 → Graphics device가 살아있을 가능성이 매우 높음

        GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
        var canvas = GameObject.Find("Loading_CANVAS");
        if (canvas != null)
            go.transform.SetParent(canvas.transform, false);
        else
            Debug.LogWarning("Loading_CANVAS가 존재하지 않습니다.");
    }

    private IEnumerator SafeReadDataAndStartGame()
    {
        // 바로 ReadData 하지 않고 → 서버 세션 Sync 확인 (USER 테이블 조회) 후 진행
        bool isDataReady = false;

        Backend.GameData.Get("USER", new Where(), callback =>
        {
            if (callback.IsSuccess())
            {
                isDataReady = true;
            }
            else
            {
                Debug.LogWarning("USER 테이블 조회 실패. 세션 동기화 대기");
            }
        });

        // 세션 sync 될 때까지 대기 (최대 3초 제한)
        float timeout = 3.0f;
        while (!isDataReady && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        if (!isDataReady)
        {
            Debug.LogError("세션 준비 실패. 로그인 다시 시도 필요.");
            Utils.Get_LoadingCanvas_ErrorUI("서버 세션 준비 실패. 잠시 후 다시 시도해주세요.");
            yield break;
        }

        // USER 테이블 확인 성공 → 데이터 정상화됨 → ReadData + 메인 시작
        Base_Manager.BACKEND.ReadData();

        // 2초 대기 후 메인 게임 시작
        yield return new WaitForSecondsRealtime(2.0f);

        if (this != null && Loading_Scene.instance != null)
        {
            Loading_Scene.instance.Main_Game_Start();
            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("BGS", 1.0f);
        }
    }

}
       

    
    


