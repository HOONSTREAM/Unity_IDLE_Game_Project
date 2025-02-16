using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LOGIN_UI_POLICY : MonoBehaviour
{
    [SerializeField]
    private Toggle Service_Toggle;
    [SerializeField]
    private Toggle Privacy_Policy_Agree_Toggle;
    [SerializeField]
    private Toggle Event_Toggle;

    public bool isAgree = false;

    private void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
    }
    public void All_Select_Button()
    {
        Service_Toggle.isOn = true;
        Privacy_Policy_Agree_Toggle.isOn = true;
        Event_Toggle.isOn = true;

        Debug.Log("모든 정책에 동의하였습니다.");
        this.gameObject.SetActive(false);
        isAgree = true;

        if (this.isAgree)
        {
            //CustomSignUp("user2", "1234");

            
        }

        // Data_Manager.Main_Players_Data.Event_Push_Alarm_Agree = true;
    }

    public void Ok_Button()
    {
        if(Service_Toggle.isOn && Privacy_Policy_Agree_Toggle.isOn)
        {
            Debug.Log("정책에 동의하였습니다.");
            this.gameObject.SetActive(false);
            isAgree = true;

            if (this.isAgree)
            {
                //CustomSignUp("user2", "1234");

                var bro = Backend.BMember.CustomLogin("user1", "1234");

                if (bro.IsSuccess())
                {
                    Debug.Log("로그인이 성공했습니다. : " + bro);

                    Base_Manager.BACKEND.ReadData();

                    Base_Manager.BACKEND.WriteData(); //서버에 저장된 데이터를 업데이트합니다.

                    Loading_Scene.instance.Main_Game_Start_Custom_Account_Test();

                }
                else
                {
                    Debug.LogError("로그인이 실패했습니다. : " + bro);
                }
            }

        }

        else
        {
            Debug.Log("정책에 동의하지 않으면, 게임을 진행할 수 없습니다.");
            return;
        }
    }
}
