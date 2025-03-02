using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private GameObject Policy_Frame;
    [SerializeField]
    private GameObject Set_NickName_Panel;
    [SerializeField]
    private TMP_InputField NickName_InputField;
    private string NickName_Text;

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
        Policy_Frame.gameObject.SetActive(false);
        isAgree = true;

        if (this.isAgree)
        {
            Set_NickName_Panel.gameObject.SetActive(true);
        }

         Data_Manager.Main_Players_Data.Event_Push_Alarm_Agree = true;
    }
    public void Ok_Button()
    {
        if (Service_Toggle.isOn && Privacy_Policy_Agree_Toggle.isOn)
        {
            Debug.Log("정책에 동의하였습니다.");
            Policy_Frame.gameObject.SetActive(false);
            isAgree = true;

            if (this.isAgree)
            {
                Set_NickName_Panel.gameObject.SetActive(true);             
            }

        }

        else
        {
            Debug.Log("정책에 동의하지 않으면, 게임을 진행할 수 없습니다.");
            return;
        }
    }

    public void Set_User_NickName()
    {
        NickName_Text = NickName_InputField.text;
        First_Custom_Login_TestMethod();
    }

    private void First_Custom_Login_TestMethod()
    {
        var login_bro = Backend.BMember.CustomLogin("user1", "1234");

        if (login_bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + login_bro);

            Backend.BMember.UpdateNickname(NickName_Text);

            //Base_Manager.BACKEND.ReadData();

            Base_Manager.BACKEND.WriteData(); //서버에 저장된 데이터를 업데이트합니다.

            Loading_Scene.instance.Main_Game_Start_Custom_Account_Test();

            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("BGS", 1.0f);

        }
        else
        {
            Debug.LogError("로그인이 실패했습니다. : " + login_bro);
        }
    }

    
}
