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
    /// 메인게임 씬의 로드를 마치고, 구글로그인을 진행합니다.
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
        LoadingUpdate(1.0f); // 100% 완료되었음을 직접 보여주기 위함

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
    /// 클라이언트 버전과 서버 버전이 일치하는지 검사합니다.
    /// </summary>
    /// <returns></returns>
    private bool Game_Vers_Update_Check()
    {

        // 버전 규칙 x.y.z
        // x : Major 버전 : 내부적으로 대대적인 수정이 있는 경우 (리팩토링 작업 등)
        // y : Minor 버전 : 신규 기능이 추가된 경우
        // z : Patch 버전 : 버그를 수정한 경우



        /*단, 뒤끝 콘솔에서 이루어지는 버전 등록 작업은 반드시 각 스토어 검수를 통과한 후 진행되어야 합니다.        
        스토어 검수를 통과해야만 각 스토어에 최신 버전이 성공적으로 업로드됩니다. 즉 검수를 통과
        하지 않았다는 것은 마켓에 최신 버전이 올라간 상태가 아니라는 것을 의미하므로, 유저에게 
        업데이트 유도를 해서는 안됩니다. 결론적으로, 반드시 스토어 검수를 통과한 후 뒤끝 콘솔에 
        버전을 등록해 주어야 합니다.*/


        Version client = new Version(Application.version);

        Debug.Log("clientVersion: " + client);


        // 뒤끝 콘솔에서 설정한 버전 정보를 조회
        var bro = Backend.Utils.GetLatestVersion();

        if (bro.IsSuccess() == false)
        {
            Utils.Get_LoadingCanvas_ErrorUI($"버전 정보를 조회하는데 실패하였습니다 : {bro}");

            return false;
        }

        var version = bro.GetReturnValuetoJSON()["version"].ToString();
        Version server = new Version(version);

        var result = server.CompareTo(client);

        if (result == 0)
        {
            // 0 이면 두 버전이 일치하는 것 입니다.
            // 아무 작업 안하고 리턴           
            return true;
        }

        else if (result < 0)
        {
            // 0 미만인 경우 server 버전이 client 보다 작은 경우 입니다.
            // 애플/구글 스토어에 검수를 넣었을 경우 여기에 해당 할 수 있습니다.
            // ex)
            // 검수를 신청한 클라이언트 버전은 3.0.0, 
            // 라이브에 운용중인 클라이언트 버전은 2.0.0,
            // 뒤끝 콘솔에 등록한 버전은 2.0.0 

            // 아무 작업을 안하고 리턴
            return true;
        }
        // 0보다 크면 server 버전이 클라이언트 이후 버전일 수 있습니다.
        else if (client == null)
        {
            // 단 클라이언트 버전 정보가 null인 경우에도 0보다 큰 값이 리턴될 수 있습니다.
            // 이 때는 아무 작업을 안하고 리턴하도록 하겠습니다.
            Utils.Get_LoadingCanvas_ErrorUI("클라이언트 버전 정보가 없습니다. 개발자에게 문의하세요 : Client Version is null ; ");
            return false;
        }

        // 여기까지 리턴 없이 왔으면 server 버전(뒤끝 콘솔에 등록한 버전)이 
        // 클라이언트보다 높은 경우 입니다.

        // 유저가 스토어에서 업데이트를 하도록 업데이트 UI를 띄워줍니다.        
        Utils.Get_LoadingCanvas_UpdateUI();

        return false;

    }
    /// <summary>
    /// 로고 페이드인아웃을 진행하고, 게임버전 체크를 진행합니다.
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
    /// 이 메서드가 실행되면, 씬이 전환됩니다.
    /// </summary>
    public void Main_Game_Start()
    {
        Base_Manager.Get_MainGame_Start = true;
        asyncOperation.allowSceneActivation = true;
    }
    /// <summary>
    /// 로그인 창 하단 로딩 바를 진행시킵니다.
    /// </summary>
    /// <param name="progress"></param>
    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
    }

}
