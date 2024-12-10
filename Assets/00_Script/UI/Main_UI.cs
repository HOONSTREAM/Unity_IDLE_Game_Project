using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Main_UI : MonoBehaviour
{
    public static Main_UI Instance = null;

    #region Parameter
    [Space(20f)]
    [Header("Default")]   
    [SerializeField]
    private TextMeshProUGUI _level_Text; // 캐릭터의 레벨을 결정합니다.
    [SerializeField]
    private TextMeshProUGUI _player_ability; // 캐릭터의 최종 전투력을 결정합니다.
    [SerializeField]
    private TextMeshProUGUI _levelup_money_text; // 캐릭터의 레벨업에 필요한 돈을 결정합니다.
    [SerializeField]
    private TextMeshProUGUI _gold_text;
    [SerializeField]
    private TextMeshProUGUI _stage_Text;
    [SerializeField]
    private TextMeshProUGUI _stage_repeat_text;
    private Color StageColor = new Color(0.1824517f, 1.0f, 0.0f, 1.0f);
    [SerializeField]
    private TextMeshProUGUI _boss_stage_text;


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

    [Space(20f)]
    [Header("Dead_Frame")]
    [SerializeField]
    private GameObject Dead_Frame;
    [Space(20f)]
    [Header("Legenedary_Popup")]
    [SerializeField]
    private Animator Popup_animator;
    [SerializeField]
    private Image Popup_Image;
    [SerializeField]
    private TextMeshProUGUI Popup_Text;
    [Space(20f)]
    [Header("Item_Bottom_Popup")]
    [SerializeField]
    private Transform ItemContent;
    [Space(20f)]
    [Header("Hero_Main_Frame")]
    [SerializeField]
    private UI_Main_Hero_Parts[] main_hero_parts;
    public Image Main_Character_Skill_CoolTime;
    private Dictionary<Player, UI_Main_Hero_Parts> Main_Parts_Dict = new Dictionary<Player, UI_Main_Hero_Parts>();

    [Space(20f)]
    [Header("ADS_BUFF")]
    [SerializeField]
    private Image Fast_Mode_Lock_Image;
    [SerializeField]
    private Animator Fast_Mode_Anim;
    [SerializeField]
    private GameObject[] Buff_Lock;
    [SerializeField]
    private Image X2_Speed_Fill;
    [SerializeField]
    private TextMeshProUGUI X2_Time_Text;
    [Space(20f)]
    [Header("BUTTONS")]
    [SerializeField]
    private Transform[] Button_Images;


    private List<TextMeshProUGUI> Bottom_Popup_Text = new List<TextMeshProUGUI>();
    private List<Coroutine> Bottom_Popup_Coroutine = new List<Coroutine>();

    private bool isPopup = false;
    private Coroutine Legendary_Coroutine;

    #endregion

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
        
        ADS_Buff_Check();
        Base_Manager.is_Fast_Mode = PlayerPrefs.GetInt("FAST") == 1 ? true : false;
        Time.timeScale = Base_Manager.is_Fast_Mode ? 1.6f : 1.0f;

        Fast_Mode_Lock_Image.gameObject.SetActive(!Base_Manager.is_Fast_Mode);


        for(int i = 0 ; i<ItemContent.childCount; i++)
        {
            Bottom_Popup_Text.Add(ItemContent.GetChild(i).GetComponent<TextMeshProUGUI>());
            Bottom_Popup_Coroutine.Add(null);
        }

        Base_Manager.Stage.M_ReadyEvent += OnReady;
        Base_Manager.Stage.M_BossEvent += OnBoss;
        Base_Manager.Stage.M_ClearEvent += OnClear;
        Base_Manager.Stage.M_DeadEvent += OnDead;

        Base_Manager.Stage.State_Change(Stage_State.Ready);
    }

    private void Update()
    {
        if(Data_Manager.Main_Players_Data.buff_x2_speed > 0.0f)
        {
            X2_Speed_Fill.fillAmount = Data_Manager.Main_Players_Data.buff_x2_speed / 1800.0f;
            X2_Time_Text.text = Utils.GetTimer(Data_Manager.Main_Players_Data.buff_x2_speed);
        }
    }

    public void Layer_Check(int value)
    {
        if(value != -1)
        {
            StartCoroutine(Button_Image_Move_Coroutine(value));
        }
       

        for (int i = 0; i < Button_Images.Length; i++)
        {
            if(value != i && Button_Images[i].localScale.x >= 1.3f)
            {
                StartCoroutine(Button_Image_Move_Coroutine(i, true));
            }
        }
    }

    IEnumerator Button_Image_Move_Coroutine(int value, bool ScaleDown = false)
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = ScaleDown ? 50.0f : 30.8f;
        float end = ScaleDown ? 30.8f : 50.0f;
        float start_scale = ScaleDown ? 1.5f : 1.0f;
        float end_scale = ScaleDown ? 1.0f : 1.5f;

        while(percent < 1.0f)
        {
            current += Time.unscaledDeltaTime;
            percent = current / 0.2f;
            float yPos = Mathf.Lerp(start, end, percent);
            float scalePos = Mathf.Lerp(start_scale, end_scale, percent);
            Button_Images[value].transform.localPosition = new Vector2(0.0f, yPos);
            Button_Images[value].transform.localScale = new Vector3(scalePos, scalePos, 1.0f);
            yield return null;
        }
    }


    public void ADS_Buff_Check()
    {
        for(int i = 0; i<Data_Manager.Main_Players_Data.Buff_Timers.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.Buff_Timers[i] > 0.0f)
            {
                Buff_Lock[i].gameObject.SetActive(false);
            }

            else
            {
                Buff_Lock[i].gameObject.SetActive(true);
            }
        }

        if(Data_Manager.Main_Players_Data.buff_x2_speed > 0.0f)
        {
            X2_Speed_Fill.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            X2_Speed_Fill.transform.parent.gameObject.SetActive(false);
        }
    }
    public void Get_Fast_Mode()
    {        
        bool fast = !(Base_Manager.is_Fast_Mode);

        if(fast == true)
        {
            if (Data_Manager.Main_Players_Data.buff_x2_speed <= 0.0f)
            {
                Base_Manager.ADS.ShowRewardedAds(() =>
                {
                    Data_Manager.Main_Players_Data.buff_x2_speed = 1800.0f;
                    ADS_Buff_Check();
                    Fast_Mode_Lock_Image.gameObject.SetActive(!fast);
                    Time.timeScale = fast ? 1.6f : 1.0f;
                });
            }
        }


        Base_Manager.is_Fast_Mode = fast;
        PlayerPrefs.SetInt("FAST", fast == true ? 1 : 0);

        ADS_Buff_Check();
        Fast_Mode_Lock_Image.gameObject.SetActive(!fast);
        Time.timeScale = fast ? 1.6f : 1.0f;
       
  
    }

    /// <summary>
    /// 아이템을 획득하였을때, 팝업을 노출하는 메서드입니다.
    /// </summary>
    /// <param name="item"></param>
    public void Show_Get_Item_Popup(Item_Scriptable item)
    {

        bool isAllActive = true;

        for(int i = 0; i<Bottom_Popup_Text.Count; i++)
        {
            if (Bottom_Popup_Text[i].gameObject.activeSelf == false)
            {
                Bottom_Popup_Text[i].gameObject.SetActive(true);
                Bottom_Popup_Text[i].text = "아이템을 획득하였습니다. " + Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>";

                for(int j = 0; j< i; j++)
                {
                    RectTransform rect = Bottom_Popup_Text[j].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 50.0f);
                    
                }
                if (Bottom_Popup_Coroutine[i] != null)
                {
                    StopCoroutine(Bottom_Popup_Coroutine[i]);
                }
                Bottom_Popup_Coroutine[i] = StartCoroutine(Item_Bottom_Popup_FadeOut_Coroutine(Bottom_Popup_Text[i].GetComponent<RectTransform>()));
                isAllActive = false;
                break;
            }
        }

        if (isAllActive)
        {
            GameObject Base_Rect = null;
            float Ycount = 0.0f;


            for(int i = 0; i<Bottom_Popup_Text.Count; i++)
            {
                RectTransform rect = Bottom_Popup_Text[i].GetComponent<RectTransform>();
                if(rect.anchoredPosition.y > Ycount)
                {
                    Base_Rect = rect.gameObject;
                    Ycount = rect.anchoredPosition.y;
                }
            }

            for (int i = 0; i < Bottom_Popup_Text.Count; i++)
            {             
                if(Base_Rect == Bottom_Popup_Text[i].gameObject)
                {
                    Bottom_Popup_Text[i].gameObject.SetActive(false);
                    Bottom_Popup_Text[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 792.0f);

                    Bottom_Popup_Text[i].gameObject.SetActive(true);
                    Bottom_Popup_Text[i].text = "아이템을 획득하였습니다. " + Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>";
                    StartCoroutine(Item_Bottom_Popup_FadeOut_Coroutine(Bottom_Popup_Text[0].GetComponent<RectTransform>()));
                }
                else
                {
                    RectTransform rect = Bottom_Popup_Text[i].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(0.0f, rect.anchoredPosition.y + 50.0f);
                }
            }
              
          
        }

        if (item.rarity == Rarity.Legendary) // 아이템의 등급이 레전더리면, 상단에 노출합니다.
        {
            Get_Legendary_Popup(item);
        }

    }
    public void Set_Boss_State()
    {
        Stage_Manager.isDead = false;
        Base_Manager.Stage.State_Change(Stage_State.Boss);
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
    private void Slider_Object_Check(bool isboss)
    {
        if (Stage_Manager.isDead)
        {
            Monster_Slider_GameObject.SetActive(false);
            Boss_Slider_GameObject.gameObject.SetActive(false);

            Dead_Frame.gameObject.SetActive(true);

            return;
        }

        Dead_Frame.gameObject.SetActive(false);
        Monster_Slider_GameObject.gameObject.SetActive(!isboss);
        Boss_Slider_GameObject.gameObject.SetActive(isboss);

        Monster_Slider_Count();

        float value = isboss? 1.0f : 0.0f;

        Boss_Slider_Count(value, 1.0f);

    }
    private void OnReady()
    {
        FadeInOut(true);
        Main_Parts_Dict.Clear();

        for(int i =0; i< 6; i++)
        {
            main_hero_parts[i].Initialize();
        }

        int Value = 0; // 메인 UI 하단 영웅배치창에 영웅이 배치되면, 순서대로 배치될 수 있도록 인덱스 정의

        for (int i = 0; i < Base_Manager.Character.Set_Character.Length; i++)
        {
            var Data = Base_Manager.Character.Set_Character[i];

            if (Data != null)
            {
                Value++;
                main_hero_parts[i].Init_Data(Data.Data, false);
                main_hero_parts[i].transform.SetSiblingIndex(Value);
                Main_Parts_Dict.Add(Character_Spawner.players[i], main_hero_parts[i]);

            }
        }

    }

    /// <summary>
    /// 메인 UI에 있는, 하단 영웅배치칸의 캐릭터 데이터를 관리합니다.
    /// </summary>
    public void Set_Character_Data()
    {
        int indexValue = 0;

        for (int i = 0; i < Base_Manager.Character.Set_Character.Length; i++)
        {
            var Data = Base_Manager.Character.Set_Character[i];

            if (Data != null)
            {
                indexValue++;
                main_hero_parts[i].Init_Data(Data.Data, true);
                main_hero_parts[i].transform.SetSiblingIndex(indexValue);              
            }
        }
    }

    /// <summary>
    /// 각 영웅 별 HP와 MP를 검사합니다.
    /// </summary>
    public void Character_State_Check(Player player)
    {
        Main_Parts_Dict[player].State_Check(player);
    }
    private void OnBoss()
    {
        Level_Text_Check();
        Slider_Object_Check(true);
    }
    private void OnClear()
    {
        Slider_Object_Check(false);
        StartCoroutine(Clear_Delay());
    }
    private void OnDead()
    {
        Main_UI.Instance.Level_Text_Check();
        StartCoroutine(Dead_Delay());
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
    public void Level_Text_Check()
    {
        double Levelup_money_Value = Utils.Data.levelData.Get_LEVELUP_MONEY();

        _level_Text.text = "LV." + (Data_Manager.Main_Players_Data.Player_Level + 1).ToString();
        _player_ability.text = StringMethod.ToCurrencyString(Base_Manager.Player.Player_ALL_Ability_ATK_HP());

        _levelup_money_text.text = StringMethod.ToCurrencyString((Levelup_money_Value));
        _levelup_money_text.color = Utils.Check_Levelup_Gold(Levelup_money_Value) ? Color.green : Color.red;

        _gold_text.text = StringMethod.ToCurrencyString(Data_Manager.Main_Players_Data.Player_Money);

        _stage_repeat_text.text = Stage_Manager.isDead ? "반복중 ..." : "진행중 ...";
        _stage_repeat_text.color = Stage_Manager.isDead ? Color.yellow : StageColor;

        int stage_Value = Data_Manager.Main_Players_Data.Player_Stage + 1;

        
        _stage_Text.text = stage_Value.ToString() + "층";
        _boss_stage_text.text = stage_Value.ToString() + "층";

    }
    public void Get_Legendary_Popup(Item_Scriptable item)
    {
        if (isPopup)
        {
            Popup_animator.gameObject.SetActive(false);
        }
        isPopup = true;
        Popup_animator.gameObject.SetActive(true);
        Popup_Image.sprite = Utils.Get_Atlas(item.name);
        Popup_Text.text = Utils.String_Color_Rarity(Rarity.Legendary) + item.Item_Name + "</color>을(를) 획득하였습니다."; 

        if(Legendary_Coroutine != null)
        {
            StopCoroutine(Legendary_Coroutine);
        }

        Legendary_Coroutine = StartCoroutine(Legendary_Popup_Coroutine());
    }


    #region Coroutine
    IEnumerator Dead_Delay()
    {
        yield return StartCoroutine(Clear_Delay());

        Slider_Object_Check(false);

        for(int i = 0; i<Spawner.m_monsters.Count; i++)
        {
            if (Spawner.m_monsters[i].isBoss == true)
            {
                Destroy(Spawner.m_monsters[i].gameObject);
            }
            else
            {
                Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(Spawner.m_monsters[i].gameObject);
            }
        }

        Spawner.m_monsters.Clear();
       
    }   
    IEnumerator Clear_Delay()
    {
        yield return new WaitForSeconds(2.0f);
        FadeInOut(false);

        yield return new WaitForSeconds(1.0f);

        
        Base_Manager.Stage.State_Change(Stage_State.Ready);

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
    IEnumerator Legendary_Popup_Coroutine()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        isPopup = false;
        Popup_animator.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(2.0f);
        Popup_animator.gameObject.SetActive(false);
    }
    IEnumerator Item_Bottom_Popup_FadeOut_Coroutine(RectTransform rect)
    {
        yield return new WaitForSecondsRealtime(2.0f);
        rect.gameObject.SetActive(false);
        rect.anchoredPosition = new Vector2(0.0f, 792.0f);
    }
    #endregion
}
