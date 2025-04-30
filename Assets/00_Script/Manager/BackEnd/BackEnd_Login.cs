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
                _ = Base_Manager.BACKEND.WriteData();
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
                    // �г����� ���� ��� ��� ���� + �г��� �Է�
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
       

    
    


