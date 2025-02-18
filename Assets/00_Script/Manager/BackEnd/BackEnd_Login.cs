using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    public void CustomSignUp(string id, string pw)
    {
        Debug.Log("ȸ�������� ��û�մϴ�.");

        GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
        go.transform.SetParent(GameObject.Find("Loading_CANVAS").gameObject.transform);

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("ȸ�����Կ� �����߽��ϴ�. : " + bro);         
            BackendGameData.Instance.Initialize_User_Data();
        }
        else
        {
            Debug.LogError("ȸ�����Կ� �����߽��ϴ�. : " + bro);
        }
    }

    public void Custom_Login_Policy_Agree()
    {
        Debug.Log("�α����� ��û�մϴ�.");

        var bro = Backend.BMember.CustomLogin("user1", "1234");

        if (bro.IsSuccess())
        {
            Debug.Log("�α����� �����߽��ϴ�. : " + bro);

            Base_Manager.BACKEND.ReadData();

            Base_Manager.BACKEND.WriteData(); //������ ����� �����͸� ������Ʈ�մϴ�.

            Loading_Scene.instance.Main_Game_Start_Custom_Account_Test();

           PlayerPrefs.SetFloat("BGM", 1.0f);
           PlayerPrefs.SetFloat("BGS", 1.0f);

        }
        else
        {
            Debug.LogError("�α����� �����߽��ϴ�. : " + bro);
        }

    }

    public void Guest_Login()
    {
        Backend.BMember.DeleteGuestInfo();

        Backend.BMember.GuestLogin(callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("�Խ�Ʈ �α��� ����");
            }
            else
            {
                Debug.LogError("�Խ�Ʈ �α��� ����: " + callback.GetErrorCode() + " " + callback.GetMessage());
            }
        });
    }

    public void GoogleLogin(string idToken)
    {
        Backend.BMember.AuthorizeFederation(idToken, FederationType.Google, "google", callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("���� �α��� ����");
            }
            else
            {
                Debug.LogError("���� �α��� ����: " + callback.GetErrorCode() + " " + callback.GetMessage());
            }
        });
    }




}
