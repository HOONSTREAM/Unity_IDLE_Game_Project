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

        Debug.Log("��� ��å�� �����Ͽ����ϴ�.");
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
            Debug.Log("��å�� �����Ͽ����ϴ�.");
            this.gameObject.SetActive(false);
            isAgree = true;

            if (this.isAgree)
            {
                //CustomSignUp("user2", "1234");

                var bro = Backend.BMember.CustomLogin("user1", "1234");

                if (bro.IsSuccess())
                {
                    Debug.Log("�α����� �����߽��ϴ�. : " + bro);

                    Base_Manager.BACKEND.ReadData();

                    Base_Manager.BACKEND.WriteData(); //������ ����� �����͸� ������Ʈ�մϴ�.

                    Loading_Scene.instance.Main_Game_Start_Custom_Account_Test();

                }
                else
                {
                    Debug.LogError("�α����� �����߽��ϴ�. : " + bro);
                }
            }

        }

        else
        {
            Debug.Log("��å�� �������� ������, ������ ������ �� �����ϴ�.");
            return;
        }
    }
}
