using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// ������̼� �α��� �����Ǹ� ���� �� �޼���.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pw"></param>
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

        Backend.BMember.CustomSignUp("user1", "1234");

        var bro = Backend.BMember.CustomLogin("user1", "1234");

        if (bro.IsSuccess())
        {
            Debug.Log("�α����� �����߽��ϴ�. : " + bro);

            BackendReturnObject user_info = Backend.GameData.GetMyData("USER", new Where());

            if (user_info.IsSuccess() && user_info.GetReturnValuetoJSON()["rows"].Count > 0)
            {
                Base_Manager.BACKEND.ReadData();

                Base_Manager.BACKEND.WriteData(); //������ ����� �����͸� ������Ʈ�մϴ�.

                Loading_Scene.instance.Main_Game_Start_Custom_Account_Test();

                PlayerPrefs.SetFloat("BGM", 1.0f);
                PlayerPrefs.SetFloat("BGS", 1.0f);
            }
        
            else
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
                go.transform.SetParent(GameObject.Find("Loading_CANVAS").gameObject.transform);
            }
            
        }
        else
        {
            Debug.LogError("�α����� �����߽��ϴ�. : " + bro);
        }

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
