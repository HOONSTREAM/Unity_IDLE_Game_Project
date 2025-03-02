using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
   
    public void Custom_Login_Policy_Agree()
    {
        Debug.Log("로그인을 요청합니다.");

        Backend.BMember.CustomSignUp("user1", "1234");
        var bro = Backend.BMember.CustomLogin("user1", "1234");

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);

            BackendReturnObject user_info = Backend.GameData.GetMyData("USER", new Where());

            if (user_info.IsSuccess() && user_info.GetReturnValuetoJSON()["rows"].Count > 0)
            {
                Base_Manager.BACKEND.ReadData();

                Base_Manager.BACKEND.WriteData(); //서버에 저장된 데이터를 업데이트합니다.

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
            Debug.LogError("로그인이 실패했습니다. : " + bro);
        }

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
