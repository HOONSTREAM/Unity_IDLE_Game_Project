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
            Debug.Log("ȸ�����Կ� �����߽��ϴ�");
        }

        BackendReturnObject bros = Backend.BMember.CustomLogin("user1", "1234");

        if (bros.IsSuccess())
        {
            Debug.Log("�α��ο� �����߽��ϴ�");

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
                // �г����� ���� ��� ��� ���� + �г��� �Է�
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
            Utils.Get_LoadingCanvas_ErrorUI($"�α��ο� �����Ͽ����ϴ� : {errorMessage}.");
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().TapToStart_Object.gameObject.SetActive(true);
            return;
        }

        Debug.Log("���� ��ū : " + token);

        var bro = Backend.BMember.AuthorizeFederation(token, FederationType.Google); // ����
        {
            if (!bro.IsSuccess())
            {
                Utils.Get_LoadingCanvas_ErrorUI(bro.ToString());
                return;
            }

            var userInfo = Backend.BMember.GetUserInfo();
            if (userInfo.IsSuccess() && userInfo.GetReturnValuetoJSON()["row"]["nickname"] != null)
            {
                // �г��� ���� �� ������ �ε� ���� ����
                CheckUserTableAndStart();
            }
            else
            {
                // �г��� ���� �� ��� ���� UI
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
                Debug.LogError("USER ���̺� ��ȸ ����. �α��� �ٽ� �õ� �ʿ�.");
                Utils.Get_LoadingCanvas_ErrorUI("���� ���� �غ� ����. ��� �� �ٽ� �õ����ּ���.");
                return;
            }

            // USER ���̺� Ȯ�� ���� �� ������ �а� ���� ����
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
        yield return null; // �Ǵ� new WaitForEndOfFrame()

        GameObject prefab = Resources.Load<GameObject>("UI/LOGIN_UI_POLICY");
        if (prefab == null)
        {
            Debug.LogError("LOGIN_UI_POLICY ���ҽ��� ã�� �� �����ϴ�.");
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
            Debug.LogWarning("Loading_CANVAS�� �������� �ʽ��ϴ�.");
        }
    }


}
       

    
    


