using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using BackEnd;

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
            Base_Manager.SOUND.Play(Sound.BGM, "Loading_Scene");

        var bro = Backend.BMember.LoginWithTheBackendToken();

        #region Auto_Login
        if (bro.IsSuccess())
        {
            //TODO :자동토큰로그인 구현부

            Debug.Log("로그인 성공");

            Auto_Login_Success_Text_First.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);
            Auto_Login_Success_Text_First.gameObject.SetActive(false);
            Auto_Login_Success_Second.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1.5f);
            Auto_Login_Success_Second.gameObject.SetActive(false);

            Base_Manager.BACKEND.Custom_Login_Policy_Agree();

        }
        else
        {
            //taptostart는 토큰만료되었을 때, 수동로그인 진행 시 구현, 그리고 출시직전 커스텀은 삭제하고 구글 페더레이션으로 교체
            TapToStart_Object.gameObject.SetActive(true);
        }
        #endregion

        
        
    }

    public void LoadingMain()
    {
        GameObject.Find("LOGO").gameObject.GetComponent<Logo_FadeOut>().StartFadeOut();
        StartCoroutine(LoadData_Coroutine());
    }

    public void Main_Game_Start_Custom_Account_Test()
    {
        Base_Manager.Get_MainGame_Start = true;
        asyncOperation.allowSceneActivation = true;
    }

    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
    }

}
