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

        Debug.Log("���� ��ū : " + token);
        var bro = Backend.BMember.AuthorizeFederation(token, FederationType.Google);
        Debug.Log("�䵥���̼� �α��� ��� : " + bro);

        var userInfo = Backend.BMember.GetUserInfo();
        if (userInfo.IsSuccess() && userInfo.GetReturnValuetoJSON()["row"]["nickname"] != null)
        {
            // �г����� ������ ��� �ٷ� ���� ����
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
    
    public void Federation_Login_AfterCheck()
    {
        BackendReturnObject userInfoBro = Backend.BMember.GetUserInfo();
        if (!userInfoBro.IsSuccess())
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"���� ���� ��ȸ�� �����Ͽ����ϴ�. �����ڿ��� �������ּ���.";
            return;
        }

        var nicknameJson = userInfoBro.GetReturnValuetoJSON()["row"]["nickname"];

        if (nicknameJson != null && !string.IsNullOrEmpty(nicknameJson.ToString()))
        {
            Debug.Log("�г��� ���� �� ������ �ε� �� ���� ����");

            Base_Manager.BACKEND.ReadData();
            _ = Base_Manager.BACKEND.WriteData();
            Loading_Scene.instance.Main_Game_Start();

            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("BGS", 1.0f);
        }
        else
        {
            Debug.Log("�г��� ���� �� ��å ���� UI ����");

            GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
            go.transform.SetParent(GameObject.Find("Loading_CANVAS").transform, false);
        }
    }

}
