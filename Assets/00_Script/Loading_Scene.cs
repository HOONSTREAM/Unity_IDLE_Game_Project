using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using BackEnd;
using System;
using Unity.VisualScripting;

public class Loading_Scene : MonoBehaviour
{

    public static Loading_Scene instance = null;

    private GameObject sliderParent;
    public Slider slider;
    public TextMeshProUGUI version_text;
    private AsyncOperation asyncOperation;
    public GameObject TapToStart_Object;


    [SerializeField]
    private GameObject Title_Object;
    [SerializeField]
    private TextMeshProUGUI Auto_Login_Success_Text_First;
    [SerializeField]
    private TextMeshProUGUI Auto_Login_Success_Second;

    [Space(20f)]
    [Header("ERROR_UI")]   
    public GameObject ERROR_UI;    
    public TextMeshProUGUI ERROR_TEXT;

    [Space(20f)]
    [Header("UPDATE_UI")]
    public GameObject UPDATE_UI;

    [Space(20f)]
    [Header("EDITOR")]
    public GameObject TEST_LOGIN_UI;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        version_text.text = "Version : " + Application.version;
        sliderParent = slider.transform.parent.gameObject;        
    }
    /// <summary>
    /// ���ΰ��� ���� �ε带 ��ġ��, ���۷α����� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadData_Coroutine()
    {
        yield return new WaitForSeconds(3.0f);

        asyncOperation = SceneManager.LoadSceneAsync("MainGame");
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            LoadingUpdate(asyncOperation.progress);
        }
        LoadingUpdate(1.0f); // 100% �Ϸ�Ǿ����� ���� �����ֱ� ����

        yield return new WaitForSeconds(1.0f);
        slider.gameObject.SetActive(false);
        Title_Object.gameObject.SetActive(true);
        Base_Manager.SOUND.Play(Sound.BGM, "Title");

        var bro = Backend.BMember.LoginWithTheBackendToken();

        #region Auto_Login

        if (bro.IsSuccess())
        {            
            Auto_Login_Success_Text_First.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);
            Auto_Login_Success_Text_First.gameObject.SetActive(false);
            Auto_Login_Success_Second.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1.5f);
            Auto_Login_Success_Second.gameObject.SetActive(false);

            Base_Manager.BACKEND.StartGoogleLogin();

        }
        else
        {
            TapToStart_Object.gameObject.SetActive(true);
        }
        #endregion
    }
    /// <summary>
    /// Ŭ���̾�Ʈ ������ ���� ������ ��ġ�ϴ��� �˻��մϴ�.
    /// </summary>
    /// <returns></returns>
    private bool Game_Vers_Update_Check()
    {

        // ���� ��Ģ x.y.z
        // x : Major ���� : ���������� ������� ������ �ִ� ��� (�����丵 �۾� ��)
        // y : Minor ���� : �ű� ����� �߰��� ���
        // z : Patch ���� : ���׸� ������ ���



        /*��, �ڳ� �ֿܼ��� �̷������ ���� ��� �۾��� �ݵ�� �� ����� �˼��� ����� �� ����Ǿ�� �մϴ�.        
        ����� �˼��� ����ؾ߸� �� ���� �ֽ� ������ ���������� ���ε�˴ϴ�. �� �˼��� ���
        ���� �ʾҴٴ� ���� ���Ͽ� �ֽ� ������ �ö� ���°� �ƴ϶�� ���� �ǹ��ϹǷ�, �������� 
        ������Ʈ ������ �ؼ��� �ȵ˴ϴ�. ���������, �ݵ�� ����� �˼��� ����� �� �ڳ� �ֿܼ� 
        ������ ����� �־�� �մϴ�.*/


        Version client = new Version(Application.version);

        Debug.Log("clientVersion: " + client);


        // �ڳ� �ֿܼ��� ������ ���� ������ ��ȸ
        var bro = Backend.Utils.GetLatestVersion();

        if (bro.IsSuccess() == false)
        {
            Utils.Get_LoadingCanvas_ErrorUI($"���� ������ ��ȸ�ϴµ� �����Ͽ����ϴ� : {bro}");

            return false;
        }

        var version = bro.GetReturnValuetoJSON()["version"].ToString();
        Version server = new Version(version);

        var result = server.CompareTo(client);

        if (result == 0)
        {
            // 0 �̸� �� ������ ��ġ�ϴ� �� �Դϴ�.
            // �ƹ� �۾� ���ϰ� ����           
            return true;
        }

        else if (result < 0)
        {
            // 0 �̸��� ��� server ������ client ���� ���� ��� �Դϴ�.
            // ����/���� ���� �˼��� �־��� ��� ���⿡ �ش� �� �� �ֽ��ϴ�.
            // ex)
            // �˼��� ��û�� Ŭ���̾�Ʈ ������ 3.0.0, 
            // ���̺꿡 ������� Ŭ���̾�Ʈ ������ 2.0.0,
            // �ڳ� �ֿܼ� ����� ������ 2.0.0 

            // �ƹ� �۾��� ���ϰ� ����
            return true;
        }
        // 0���� ũ�� server ������ Ŭ���̾�Ʈ ���� ������ �� �ֽ��ϴ�.
        else if (client == null)
        {
            // �� Ŭ���̾�Ʈ ���� ������ null�� ��쿡�� 0���� ū ���� ���ϵ� �� �ֽ��ϴ�.
            // �� ���� �ƹ� �۾��� ���ϰ� �����ϵ��� �ϰڽ��ϴ�.
            Utils.Get_LoadingCanvas_ErrorUI("Ŭ���̾�Ʈ ���� ������ �����ϴ�. �����ڿ��� �����ϼ��� : Client Version is null ; ");
            return false;
        }

        // ������� ���� ���� ������ server ����(�ڳ� �ֿܼ� ����� ����)�� 
        // Ŭ���̾�Ʈ���� ���� ��� �Դϴ�.

        // ������ ������ ������Ʈ�� �ϵ��� ������Ʈ UI�� ����ݴϴ�.        
        Utils.Get_LoadingCanvas_UpdateUI();

        return false;

    }
    /// <summary>
    /// �ΰ� ���̵��ξƿ��� �����ϰ�, ���ӹ��� üũ�� �����մϴ�.
    /// </summary>
    public void LoadingMain()
    {       
        bool check;

#if UNITY_EDITOR
        check = true;
#else
    check = Game_Vers_Update_Check();
#endif

        if (check)
        {
            StartCoroutine(LoadData_Coroutine());
        }

        else
        {          
            return;
        }
        
    }
    /// <summary>
    /// �� �޼��尡 ����Ǹ�, ���� ��ȯ�˴ϴ�.
    /// </summary>
    public void Main_Game_Start()
    {
        Base_Manager.Get_MainGame_Start = true;
        asyncOperation.allowSceneActivation = true;
    }
    /// <summary>
    /// �α��� â �ϴ� �ε� �ٸ� �����ŵ�ϴ�.
    /// </summary>
    /// <param name="progress"></param>
    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
    }

}
