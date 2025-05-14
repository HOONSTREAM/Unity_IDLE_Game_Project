using System.Collections;
using UnityEngine;

public class Loading_UI_Runner : MonoBehaviour
{
    private static Loading_UI_Runner _instance;
    public static Loading_UI_Runner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Loading_UI_Runner");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<Loading_UI_Runner>();
            }
            return _instance;
        }
    }

    public void ShowErrorUI(string text)
    {
        StartCoroutine(DelayedShowError(text));
    }

    private IEnumerator DelayedShowError(string text)
    {
        yield return null; // ¶Ç´Â yield return new WaitForEndOfFrame();

        var canvas = GameObject.Find("Loading_CANVAS");
        if (canvas == null) yield break;

        var loadingScene = canvas.GetComponent<Loading_Scene>();
        if (loadingScene == null) yield break;

        var errorUI = loadingScene.ERROR_UI?.gameObject;
        var errorText = loadingScene.ERROR_TEXT;

        if (errorUI != null)
        {
            errorUI.SetActive(true);
            errorUI.transform.SetSiblingIndex(4);
        }

        if (errorText != null)
        {
            errorText.text = text;
        }
    }

    public void ShowUpdateUI()
    {
        StartCoroutine(DelayedShowUpdate());
    }

    private IEnumerator DelayedShowUpdate()
    {
        yield return null;

        var canvas = GameObject.Find("Loading_CANVAS");
        if (canvas == null) yield break;

        var loadingScene = canvas.GetComponent<Loading_Scene>();
        if (loadingScene == null) yield break;

        var updateUI = loadingScene.UPDATE_UI?.gameObject;
        if (updateUI != null)
        {
            updateUI.SetActive(true);
            updateUI.transform.SetSiblingIndex(4);
        }
    }
}
