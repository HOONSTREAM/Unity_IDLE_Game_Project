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

        GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
        go.gameObject.SetActive(true);
        go.transform.SetSiblingIndex(4);
        GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"모든 정책에 동의하였습니다..";
        Policy_Frame.gameObject.SetActive(false);
        isAgree = true;

        if (this.isAgree)
        {
            Set_NickName_Panel.gameObject.SetActive(true);
        }

         Utils.is_push_alarm_agree = true;
    }
    public void Ok_Button()
    {
        if (Service_Toggle.isOn && Privacy_Policy_Agree_Toggle.isOn && Event_Toggle.isOn)
        {
            Policy_Frame.gameObject.SetActive(false);
            isAgree = true;

            if (this.isAgree)
            {
                Set_NickName_Panel.gameObject.SetActive(true);
            }

            Utils.is_push_alarm_agree = true;
        }

        else if (Service_Toggle.isOn && Privacy_Policy_Agree_Toggle.isOn)
        {
            Policy_Frame.gameObject.SetActive(false);
            isAgree = true;

            if (this.isAgree)
            {
                Set_NickName_Panel.gameObject.SetActive(true);
            }
        }

        else
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"정책에 동의하지 않으면, 게임을 진행 할 수 없습니다.";
            return;
        }
    }

    public void Get_Policy_URL(string url)
    {
        Application.OpenURL(url);
    }

    public void Game_Quit()
    {
        Application.Quit();
    }

    public void Set_User_NickName()
    {
        NickName_Text = NickName_InputField.text;

        if (string.IsNullOrEmpty(NickName_Text))
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"닉네임이 비어있습니다. 닉네임을 입력해주세요.";
            return;
        }

        var checkBro = Backend.BMember.CheckNicknameDuplication(NickName_Text);

        if (!checkBro.IsSuccess())
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = "닉네임이 중복되었습니다: " + checkBro.GetMessage();                      
            return;
        }

        First_Custom_Login_TestMethod();
    }

    private void First_Custom_Login_TestMethod()
    {
        var login_bro = Backend.BMember.CustomLogin("user8", "1234");   

        if (login_bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + login_bro);

            Backend.BMember.UpdateNickname(NickName_Text);
   
            _ = Base_Manager.BACKEND.WriteData();

            Loading_Scene.instance.Main_Game_Start_Custom_Account_Test();

            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("BGS", 1.0f);
            
        }
        else
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = "로그인에 실패했습니다. 지속적으로 실패하면, 관리자에게 문의하십시오. : " + login_bro;
        }
    }

    
}
