using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_UI : MonoBehaviour
{


    public static Main_UI Instance = null;

    [Space(20f)]
    [Header("Default")]   
    [SerializeField]
    private TextMeshProUGUI _level_Text;
    [SerializeField]
    private TextMeshProUGUI _player_ability;

    [Space(20f)]
    [Header("Fade")]
    [SerializeField]
    private Image Fade;
    [SerializeField]
    private float Fade_Duration;

    [Space(20f)]
    [Header("Monster_Slider")]   
    [SerializeField]
    private Image Monster_Slider;
    [SerializeField]
    private GameObject Monster_Slider_GameObject;
    [SerializeField]
    private TextMeshProUGUI M_Monster_Value_Text;

    [Space(20f)]
    [Header("Boss_Slider")] 
    [SerializeField]
    private GameObject Boss_Slider_GameObject;
    [SerializeField]
    private TextMeshProUGUI M_Boss_HP_Text;
    [SerializeField]
    private Image Boss_Slider;
    [SerializeField]
    private TextMeshProUGUI Boss_Stage_Text;

    
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
        Monster_Slider_Count();
        Base_Manager.Stage.M_ReadyEvent += () => FadeInOut(true);
        Base_Manager.Stage.M_BossEvent += OnBoss;
    }

    public void Monster_Slider_Count()
    {
        float value = (float)Stage_Manager.Count / (float)Stage_Manager.MaxCount;
        if(value >= 1.0f)
        {
            value = 1.0f;
            if(Stage_Manager.M_State != Stage_State.Boss)
            {
                Base_Manager.Stage.State_Change(Stage_State.Boss);
            }

        }
        Monster_Slider.fillAmount = value;
        M_Monster_Value_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }

    public void Boss_Slider_Count(double hp, double MaxHp)
    {
        float value = (float)hp / (float)MaxHp;
        if (value <= 0.0f)
        {
            value = 0.0f;

        }
        Boss_Slider.fillAmount = value;
        M_Boss_HP_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }

    private void OnBoss()
    {
        Boss_Slider_GameObject.gameObject.SetActive(true);
        Monster_Slider_GameObject.gameObject.SetActive(false);
    }
    public void FadeInOut(bool FadeInout, bool Sibling = false, Action action = null)
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
