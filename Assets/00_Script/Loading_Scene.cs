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

    private void Update()
    {
        if (asyncOperation!=null)
        {
            if (asyncOperation.progress >= 0.9f && Input.GetMouseButtonDown(0))
            {
                asyncOperation.allowSceneActivation = true;
            }
        }
      
    }

    IEnumerator LoadData_Coroutine()
    {
        asyncOperation = SceneManager.LoadSceneAsync("MainGame");
        asyncOperation.allowSceneActivation = false;

        while(asyncOperation.progress < 0.9f)
        {
            LoadingUpdate(asyncOperation.progress);
            yield return null;
        }
        LoadingUpdate(1.0f); // 100% 완료되었음을 직접 보여주기 위함       
        yield return new WaitForSeconds(1.0f);
        slider.gameObject.SetActive(false);
        TapToStart_Object.gameObject.SetActive(true);
    }

    public void LoadingMain()
    {
        StartCoroutine(LoadData_Coroutine());
    }

    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
    }

}
