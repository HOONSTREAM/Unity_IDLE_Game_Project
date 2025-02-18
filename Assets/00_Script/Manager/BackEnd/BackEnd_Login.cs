using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    public void CustomSignUp(string id, string pw)
    {
        Debug.Log("회원가입을 요청합니다.");

        GameObject go = Instantiate(Resources.Load<GameObject>("UI/LOGIN_UI_POLICY"));
        go.transform.SetParent(GameObject.Find("Loading_CANVAS").gameObject.transform);

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입에 성공했습니다. : " + bro);         
            BackendGameData.Instance.Initialize_User_Data();
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
        }
    }

    public void Custom_Login_Policy_Agree()
    {
        Debug.Log("로그인을 요청합니다.");

        var bro = Backend.BMember.CustomLogin("user1", "1234");

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);

            Base_Manager.BACKEND.ReadData();

            Base_Manager.BACKEND.WriteData(); //서버에 저장된 데이터를 업데이트합니다.

            Loading_Scene.instance.Main_Game_Start_Custom_Account_Test();

           PlayerPrefs.SetFloat("BGM", 1.0f);
           PlayerPrefs.SetFloat("BGS", 1.0f);

        }
        else
        {
            Debug.LogError("로그인이 실패했습니다. : " + bro);
        }

    }

    public void Guest_Login()
    {
        Backend.BMember.DeleteGuestInfo();

        Backend.BMember.GuestLogin(callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("게스트 로그인 성공");
            }
            else
            {
                Debug.LogError("게스트 로그인 실패: " + callback.GetErrorCode() + " " + callback.GetMessage());
            }
        });
    }

    public void GoogleLogin(string idToken)
    {
        Backend.BMember.AuthorizeFederation(idToken, FederationType.Google, "google", callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("구글 로그인 성공");
            }
            else
            {
                Debug.LogError("구글 로그인 실패: " + callback.GetErrorCode() + " " + callback.GetMessage());
            }
        });
    }




}
