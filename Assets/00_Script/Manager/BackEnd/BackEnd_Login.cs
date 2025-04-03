using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// �׽�Ʈ�޼���, ������̼� �����Ǹ� ����
    /// </summary>
    private void Custom_Sign_Up_Initialize()
    {
        Backend.BMember.CustomSignUp("user6", "1234");
        BackendGameData.Instance.Initialize_User_Data();
        Custom_Login_Policy_Agree();
    }

    /// <summary>
    /// ������̼� �����Ǹ� ����
    /// </summary>
    public void Custom_Login_Policy_Agree()
    {
        Debug.Log("�α����� ��û�մϴ�.");

        
        Backend.BMember.CustomLogin("user6", "1234", (BackendReturnObject bro) =>
        {
            if (bro.IsSuccess())
            {
                Debug.Log("�α����� �����߽��ϴ�. : " + bro);

                BackendReturnObject user_info = Backend.GameData.GetMyData("USER", new Where());
                BackendReturnObject nickname_info = Backend.BMember.GetUserInfo();
                if (user_info.IsSuccess())
                {
                    if (nickname_info.GetReturnValuetoJSON()["row"]["nickname"] != null)
                    {
                        Base_Manager.BACKEND.ReadData();

                        _ = Base_Manager.BACKEND.WriteData(); //������ ����� �����͸� ������Ʈ�մϴ�.

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
                    GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
                    go.transform.SetParent(GameObject.Find("Loading_CANVAS").gameObject.transform);
                }

            }
            else
            {
                Debug.LogError("�α����� �����߽��ϴ�. ȸ�������� �����մϴ�. : " + bro);
                Custom_Sign_Up_Initialize();

            }
        });
        

       

    }

}
