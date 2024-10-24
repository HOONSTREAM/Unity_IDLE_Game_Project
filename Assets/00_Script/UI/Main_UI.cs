using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_UI : MonoBehaviour
{
    public static Main_UI Instance = null;
    [SerializeField]
    private TextMeshProUGUI _level_Text;
    [SerializeField]
    private TextMeshProUGUI _player_ability;
    [SerializeField]
    private Image Fade;
    [SerializeField]
    private float Fade_Duration;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Level_Text_Check();
    }

    public void FadeInOut(bool FadeInout, bool Sibling, Action action = null)
    {
        if (!Sibling)
        {
            Fade.transform.parent = this.transform;
            Fade.transform.SetSiblingIndex(0);
        }
        else
        {
            Fade.transform.parent = Base_Canvas.instance.transform;
            Fade.transform.SetAsLastSibling();
        }

        StartCoroutine(FadeInOut_Coroutine(FadeInout, action));
    }

    /// <summary>
    /// 코루틴이 끝나는 시점에 어떤 액션을 취할 것 인지도 인자로 넣어준다.
    /// </summary>
    /// <param name="FadeInout"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    IEnumerator FadeInOut_Coroutine(bool FadeInout, Action action = null)
    {
        if(FadeInout == false)
        {
            Fade.raycastTarget = true;
        }



        float current = 0.0f;
        float percent = 0.0f;
        float start = FadeInout ? 1.0f : 0.0f;
        float end = FadeInout ? 0.0f : 1.0f;

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / Fade_Duration;
            float LerpPos = Mathf.Lerp(start, end, percent);
            Fade.color = new Color (0,0,0, LerpPos); 

            yield return null;
        }

        if(action != null)
        {
            action?.Invoke();
        }

        Fade.raycastTarget = false;
    }

    public void Level_Text_Check()
    {
        _level_Text.text = "LV." + (Base_Manager.Player.Level + 1).ToString();
        _player_ability.text = StringMethod.ToCurrencyString(Base_Manager.Player.Player_ALL_Ability_ATK_HP());
    }

}
