using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// 테스트메서드, 페더레이션 구현되면 삭제
    /// </summary>
    private void Custom_Sign_Up_Initialize()
    {
        Backend.BMember.CustomSignUp("user6", "1234");
        BackendGameData.Instance.Initialize_User_Data();
        Custom_Login_Policy_Agree();
    }

    /// <summary>
    /// 페더레이션 구현되면 수정
    /// </summary>
    public void Custom_Login_Policy_Agree()
    {
        Debug.Log("로그인을 요청합니다.");

        
        Backend.BMember.CustomLogin("user6", "1234", (BackendReturnObject bro) =>
        {
            if (bro.IsSuccess())
            {
                Debug.Log("로그인이 성공했습니다. : " + bro);

                BackendReturnObject user_info = Backend.GameData.GetMyData("USER", new Where());
                BackendReturnObject nickname_info = Backend.BMember.GetUserInfo();
                if (user_info.IsSuccess())
                {
                    if (nickname_info.GetReturnValuetoJSON()["row"]["nickname"] != null)
                    {
                        Base_Manager.BACKEND.ReadData();

                        _ = Base_Manager.BACKEND.WriteData(); //서버에 저장된 데이터를 업데이트합니다.

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
                Debug.LogError("로그인이 실패했습니다. 회원가입을 진행합니다. : " + bro);
                Custom_Sign_Up_Initialize();

            }
        });
        

       

    }

}
