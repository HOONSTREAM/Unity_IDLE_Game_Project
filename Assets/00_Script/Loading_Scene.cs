using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Loading_Scene : MonoBehaviour
{
    public static Loading_Scene instance = null;

    private GameObject sliderParent;
    public Slider slider;
    public TextMeshProUGUI version_text;
    private AsyncOperation asyncOperation;
    public GameObject TapToStart_Object;

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

            if (asyncOperation != null)
            {
                if (asyncOperation.progress >= 0.9f)
                {                    
                    Base_Manager.Get_MainGame_Start = true;
                    yield return null;
                }
            }
   
        }
        LoadingUpdate(1.0f); // 100% 완료되었음을 직접 보여주기 위함
                             
        yield return new WaitForSeconds(1.0f);
        slider.gameObject.SetActive(false);
        TapToStart_Object.gameObject.SetActive(true);
    }

    public void LoadingMain()
    {
        GameObject.Find("LOGO").gameObject.GetComponent<Logo_FadeOut>().StartFadeOut();
        StartCoroutine(LoadData_Coroutine());
    }

    public void Main_Game_Start_Custom_Account_Test()
    {
        asyncOperation.allowSceneActivation = true;
    }

    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
    }

}
