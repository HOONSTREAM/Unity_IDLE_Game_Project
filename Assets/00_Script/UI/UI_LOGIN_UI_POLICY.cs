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
        GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"��� ��å�� �����Ͽ����ϴ�..";
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
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"��å�� �������� ������, ������ ���� �� �� �����ϴ�.";
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
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = $"�г����� ����ֽ��ϴ�. �г����� �Է����ּ���.";
            return;
        }

        var checkBro = Backend.BMember.CheckNicknameDuplication(NickName_Text);

        if (!checkBro.IsSuccess())
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = "�г����� �ߺ��Ǿ����ϴ�: " + checkBro.GetMessage();                      
            return;
        }

        var nicknameBro = Backend.BMember.UpdateNickname(NickName_Text);

        if (nicknameBro.IsSuccess())
        {
            Debug.Log("�г��� ��� ����");
            BackendGameData.Instance.Initialize_User_Data();
            _ = Base_Manager.BACKEND.WriteData();
            Loading_Scene.instance.Main_Game_Start();

            PlayerPrefs.SetFloat("BGM", 1.0f);
            PlayerPrefs.SetFloat("BGS", 1.0f);
        }
        else
        {
            GameObject go = GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_UI.gameObject;
            go.gameObject.SetActive(true);
            go.transform.SetSiblingIndex(4);
            GameObject.Find("Loading_CANVAS").gameObject.GetComponent<Loading_Scene>().ERROR_TEXT.text = "�α��ο� �����߽��ϴ�. ���������� �����ϸ�, �����ڿ��� �����Ͻʽÿ�. : " + nicknameBro;
        }
    }
   
}
