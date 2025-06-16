using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;
    public Transform Coin;
    [SerializeField]
    private Transform LAYER;
    [SerializeField] 
    private Transform BACK_LAYER;
    [SerializeField]
    private Button Hero_Button;
    [SerializeField]
    private Button Inventory_Button;
    [SerializeField]
    private Button Saving_Mode_Button;
    [SerializeField]
    private Button ADS_Buff_Button;
    [SerializeField]
    private Button Shop_Button;
    [SerializeField]
    private Button Relic_Button;
    [SerializeField]
    private Button Dungeon_Button;
    [SerializeField]
    private Button Status_Button;
    [SerializeField]
    private Button Smelt_Button;
    [SerializeField]
    private Button LAUNCH_EVENT_Button;
    [SerializeField]
    private Button Setting_Button;
    [SerializeField]
    private Button Daily_Quest_Button;
    [SerializeField]
    private Button Attendance_Button;
    [SerializeField]
    private Button Combination_Button;
    [SerializeField]
    private Button Post_Box_Button;
    [SerializeField]
    private Button Chat_Button;
    [SerializeField]
    private Button Heros_Dictionary_Button;
    [SerializeField]
    private Button Rank_Button;
    [SerializeField]
    private Button Dead_Frame_Button;
    [SerializeField]
    private Button Select_Stage_Button;
    [SerializeField]
    private GameObject Tutorial_Levelup_Button_Panel;

    [HideInInspector]
    public Item_ToolTip item_tooltip = null;
    [HideInInspector]
    public Hero_ToolTip hero_tooltip = null;
    [HideInInspector]
    public Relic_ToolTip relic_tooltip = null;
    [HideInInspector]
    public Skill_ToolTip skill_tooltip = null;
    [HideInInspector]
    public Status_ToolTip Status_tooltip = null;
    [HideInInspector]
    public UI_Base UI;
    public static bool isSavingMode = false;

    [SerializeField]
    private GameObject Tutorial_Panel;
    [SerializeField]
    private GameObject All_Block_Panel;

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        UI_Daily_Quest.OnDailyQuestUIOpened -= Tutorial_Daily_Quest;
        UI_Daily_Quest.OnDailyQuestUIOpened += Tutorial_Daily_Quest;
        Daily_Quest_Parts.Pressed_Daily_Quest_Parts_Reward -= Tutorial_Daily_Close_And_Goto_Shop;
        Daily_Quest_Parts.Pressed_Daily_Quest_Parts_Reward += Tutorial_Daily_Close_And_Goto_Shop;
        UI_Shop.UI_Shop_First_Closed -= Start_Hero_Set_Tutorial;
        UI_Shop.UI_Shop_First_Closed += Start_Hero_Set_Tutorial;
        UI_Heros.Pressed_Hero_Exit -= Start_Press_DeadFrame_Tutorial;
        UI_Heros.Pressed_Hero_Exit += Start_Press_DeadFrame_Tutorial;
        Main_UI.Dead_Frame_Pressed_Tutorial -= Start_Tutorial_Levelup_Button;
        Main_UI.Dead_Frame_Pressed_Tutorial += Start_Tutorial_Levelup_Button;
        LevelUp_Button.Pressed_Levelup_Button_Tutorial -= Start_Levelup_Button_tutorial;
        LevelUp_Button.Pressed_Levelup_Button_Tutorial += Start_Levelup_Button_tutorial;

        if (Data_Manager.Main_Players_Data.isBuyLAUNCH_EVENT)
        {
            LAUNCH_EVENT_Button.gameObject.SetActive(false);
        }

        if (Utils.Offline_Timer_Check() >= Utils.OFFLINE_TIME_CHECK)
        {
            Get_UI("OFFLINE_REWARD");
            Base_Manager.SOUND.Play(Sound.BGS, "OFFLINE");
        }

        Base_Manager.SOUND.Play(Sound.BGM, "Village");

        Hero_Button.onClick.AddListener(() => Get_UI("@Heros", true, false, true, 1));
        Relic_Button.onClick.AddListener(() => Get_UI("UI_RELIC", false, false, true, 2));
        Inventory_Button.onClick.AddListener(() => Get_UI("UI_INVENTORY"));
        Saving_Mode_Button.onClick.AddListener(() => {
            Get_UI("Saving_Mode");
            isSavingMode = true;
        });
        ADS_Buff_Button.onClick.AddListener(() => { Get_UI("ADS_Buff"); });

        Shop_Button.onClick.AddListener(() => {

            Get_UI("Shop", false, true, true, 5);
            UI_Shop.UI_Shop_First_Opened?.Invoke();

            }); 

        Dungeon_Button.onClick.AddListener(() =>
        {
            if (!Stage_Manager.isDungeon) 
            {
                Get_UI("UI_Dungeon", false, false, true, 3);
            }
        });
        Smelt_Button.onClick.AddListener(() => Get_UI("UI_Smelt", false, false, true, 4));
        Status_Button.onClick.AddListener(() => Get_UI("@Status", false, false, true, 0));       
        LAUNCH_EVENT_Button.onClick.AddListener(() => Get_UI("LAUNCH_EVENT", false, false, true));
        Heros_Dictionary_Button.onClick.AddListener(() => Get_UI("Heros_Dictionary", false, false, true));
        Setting_Button.onClick.AddListener(() => Get_UI("UI_Setting", false, false, true));
        Daily_Quest_Button.onClick.AddListener(() => Get_UI("UI_Daliy_Quest", false, false, true));
        Attendance_Button.onClick.AddListener(() => Get_UI("UI_Attendance", false, false, true));
        Combination_Button.onClick.AddListener(() => Get_UI("UI_Combination", false, false, true));
        Post_Box_Button.onClick.AddListener(() => Get_UI("UI_PostBox", false, false, true));

        Chat_Button.onClick.AddListener(() =>
        {
            if (Data_Manager.Main_Players_Data.Player_Stage >= 50)
            {
                Get_UI("@Chat", false, false, true);
            }
            else
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("스테이지 50층 이상부터 해금됩니다.");
            }
        });

        Select_Stage_Button.onClick.AddListener(() => Get_UI("UI_SELECT_STAGE", false, false, true));

        Rank_Button.onClick.AddListener(() =>
        {
            DateTime serverTime = Utils.Get_Server_Time(); 

            if (serverTime.Hour >= 0 && serverTime.Hour < 1)
            {
                Base_Canvas.instance.Get_TOP_Popup().Initialize("00시~01시 사이에는, 랭크 업데이트 중 입니다.");
                return; 
            }

            Get_UI("UI_Rank", false, false, true);
        });
       
    }
    private void Update()
    {
        Get_Escape_Panel();        
    }

    private void Start_Tutorial_Levelup_Button()
    {
        All_Block_Panel.gameObject.SetActive(true);
        StartCoroutine(Start_Tutorial_Level_Button_Coroutine());
    }
    IEnumerator Start_Tutorial_Level_Button_Coroutine()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("몬스터와 보스를 처치하고 획득한 골드로, 강해질 수 있습니다.");
        yield return new WaitForSecondsRealtime(2.0f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("성장방법은, 레벨업, 영웅강화, 각인 등이 있습니다.");
        yield return new WaitForSecondsRealtime(2.0f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("레벨업을 해서, 더욱 강해져봅시다!");
        All_Block_Panel.gameObject.SetActive(false);
        Tutorial_Levelup_Button_Panel.gameObject.SetActive(true);              
    }
    private void Start_Levelup_Button_tutorial()
    {
        Base_Canvas.instance.Get_TOP_Popup().Initialize("기본 튜토리얼을 모두 마쳤습니다!");
        Tutorial_Levelup_Button_Panel.gameObject.SetActive(false);    
        Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        Utils.is_Tutorial = false;
    } 
    private void Start_Hero_Set_Tutorial()
    {       
        Base_Canvas.instance.Get_TOP_Popup().Initialize("이제 영웅을 배치하여, 전투에 임해봅니다.");
        Start_Tutorial(Hero_Button);
    }
    private void Start_Press_DeadFrame_Tutorial()
    {
        StartCoroutine(DeadFrame_Tutorial_Coroutine());
    }
    IEnumerator DeadFrame_Tutorial_Coroutine()
    {
        All_Block_Panel.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        Main_UI.Instance.Set_Mode_Change_Idle_Mode();
        Base_Canvas.instance.Get_TOP_Popup().Initialize("이런, 전투에서 패배해서 BOSS 버튼이 열렸습니다.");
        yield return new WaitForSecondsRealtime(2.5f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("전투에서 패배하게되면, BOSS 버튼이 열리고, 방치모드에 들어갑니다.");
        yield return new WaitForSecondsRealtime(2.5f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("방치모드란, 데미지를 받지않고 무한 성장이 가능한 모드입니다.");
        yield return new WaitForSecondsRealtime(2.5f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("영웅을 배치했으니, BOSS 버튼을 눌러서, 다음 층으로 진행할 수 있습니다.");
        yield return new WaitForSecondsRealtime(1.8f);
        All_Block_Panel.gameObject.SetActive(false);
        Start_Tutorial(Dead_Frame_Button);

    }
    public void Start_First_Tutorial()
    {
        StartCoroutine(Delay_Start_Tutorial_Coroutine());
    }
    private void Get_Escape_Panel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Utils.UI_Holder.Count > 0)
            {
                Utils.ClosePopupUI();
            }

            else
            {
                Get_UI("Back_Button_Popup");
            }

        }
    }
    public Transform Holder_Layer(int value)
    {
        return LAYER.GetChild(value);
    }
    public void All_Layer_Destroy()
    {
        for (int i = 0; i < GameObject.Find("Layer1").gameObject.transform.childCount; i++)
        {
            Destroy(GameObject.Find("Layer1").gameObject.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameObject.Find("Layer2").gameObject.transform.childCount; i++)
        {
            Destroy(GameObject.Find("Layer2").gameObject.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameObject.Find("Layer3").gameObject.transform.childCount; i++)
        {
            Destroy(GameObject.Find("Layer3").gameObject.transform.GetChild(i).gameObject);
        }

        return;
    }
    public void Get_UI(string temp, bool Fade = false, bool Back = false, bool Close = false, int value = -1)
    {
        if (Utils.UI_Holder.Count > 0)
        {
            var topUI = Utils.UI_Holder.Peek();
            if (topUI != null && topUI.name == temp)
            {
                Utils.CloseAllPopupUI();
                Main_UI.Instance.Layer_Check(-1);
                return;
            }
        }

        if (Close)
        {
            Utils.CloseAllPopupUI();
        }
        if (Fade)
        {
            Main_UI.Instance.FadeInOut(false, true, () => GetPopupUI(temp, Back));
            Main_UI.Instance.Layer_Check(value);
            return;
        }
        Main_UI.Instance.Layer_Check(value);       
        GetPopupUI(temp, Back);
    }
    private void GetPopupUI(string temp, bool Back = false)
    {
        if (UI != null) UI = null;

        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), Back == true ? BACK_LAYER : transform);
        UI = go;
        UI.name = temp;

        Utils.UI_Holder.Push(go);
    }
    public Item_ToolTip Get_Item_Tooltip()
    {
        if (item_tooltip == null) // 기존 툴팁이 없을 때만 새로 생성
        {
            item_tooltip = Instantiate(Resources.Load<Item_ToolTip>("UI/Item_ToolTip"), transform);
        }

        return item_tooltip;
    }

    public Hero_ToolTip Get_Hero_Tooltip()
    {
        if (hero_tooltip == null) // 기존 툴팁이 없을 때만 새로 생성
        {
            hero_tooltip = Instantiate(Resources.Load<Hero_ToolTip>("UI/Hero_ToolTip"), transform);
        }

        return hero_tooltip;
    }

    public Relic_ToolTip Get_Relic_Tooltip()
    {
        if (relic_tooltip == null) // 기존 툴팁이 없을 때만 새로 생성
        {
            relic_tooltip = Instantiate(Resources.Load<Relic_ToolTip>("UI/Relic_ToolTip"), transform);
        }

        return relic_tooltip;
    }

    public Skill_ToolTip Get_Skill_Tooltip()
    {
        if (skill_tooltip == null) // 기존 툴팁이 없을 때만 새로 생성
        {
            skill_tooltip = Instantiate(Resources.Load<Skill_ToolTip>("UI/Skill_ToolTip"), transform);
        }

        return skill_tooltip;
    }
    public Status_ToolTip Get_Status_Item_Tooltip()
    {
        if (Status_tooltip == null) // 기존 툴팁이 없을 때만 새로 생성
        {           
            Status_tooltip = Instantiate(Resources.Load<Status_ToolTip>("UI/Status_ToolTip"), Base_Canvas.instance.transform);                  
        }

        return Status_tooltip;
    }
    public UI_Toast_Popup Get_Toast_Popup()
    {
        return Instantiate(Resources.Load<UI_Toast_Popup>("UI/Popup"), transform); //transform은 해당위치에 생성하라는 인자
    }
    public UI_TOP_POPUP Get_TOP_Popup()
    {
        return Instantiate(Resources.Load<UI_TOP_POPUP>("UI/TOP_POPUP"), transform); //transform은 해당위치에 생성하라는 인자
    }
    public MainGame_Error_UI Get_MainGame_Error_UI()
    {
        return Instantiate(Resources.Load<MainGame_Error_UI>("UI/MainGame_Error_UI"), transform); //transform은 해당위치에 생성하라는 인자
    }
    public void Destroy_Launch_Event_Button()
    {
        LAUNCH_EVENT_Button.gameObject.SetActive(false);
    }
    public void Start_Tutorial(Button original_Button)
    {
        if (!Utils.is_Tutorial)
        {
            Tutorial_Panel.gameObject.SetActive(false);
            return;
        }

        Tutorial_Panel.gameObject.SetActive(true);

        GameObject copy = Instantiate(original_Button.gameObject);
        RectTransform copyRect = copy.GetComponent<RectTransform>();
        RectTransform original_Rect = original_Button.GetComponent<RectTransform>();

        copy.transform.SetParent(Tutorial_Panel.transform);

        copyRect.anchorMin = new Vector2(0.5f, 0.5f);
        copyRect.anchorMax = new Vector2(0.5f, 0.5f);
        copyRect.pivot = new Vector2(0.5f, 0.5f);
        copyRect.sizeDelta = original_Rect.sizeDelta;

        copyRect.position = original_Rect.position;
        copyRect.localScale = Vector3.one;

        Button copy_Button = copy.GetComponent<Button>();
        copy_Button.onClick = original_Button.onClick;
        copy_Button.onClick.AddListener(() => End_Tutorial());
    }

    private void End_Tutorial()
    {
        for(int i = 0; i<Tutorial_Panel.transform.childCount; i++)
        {
            Destroy(Tutorial_Panel.transform.GetChild(i).gameObject);

        }

        Tutorial_Panel.gameObject.SetActive(false);
    }

    IEnumerator Delay_Start_Tutorial_Coroutine()
    {
        All_Block_Panel.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2.0f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("영웅 파티 키우기 세계에 오신 것을 환영합니다!");
        Main_UI.Instance.Get_Fast_Mode_Free_Start_User();
        Base_Canvas.instance.Get_Toast_Popup().Initialize("광고버프체험! 2배속이 30분 지급됩니다.");
        yield return new WaitForSecondsRealtime(2.0f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("일일 퀘스트를 통하여, 기본 조작을 익혀봅니다.");
        yield return new WaitForSecondsRealtime(1.0f);
        All_Block_Panel.gameObject.SetActive(false);
        Start_Tutorial(Daily_Quest_Button);
    }

    private void Tutorial_Daily_Quest()
    {
        if (Utils.is_Tutorial)
        {
            if (Data_Manager.Main_Players_Data.DiaMond < 500) // 다이아가 500개 미만이면, 튜토리얼 진행중으로 간주
            {
                UI_Daily_Quest.is_Tutorial_Attendance = true;
                Base_Canvas.instance.Get_TOP_Popup().Initialize("출석 보상을 수령해봅니다.");
            }
            else
            {
                Base_Canvas.instance.Get_TOP_Popup().Initialize("일일 퀘스트 창을 닫고, 상점으로 향해볼까요?");
                UI_Daily_Quest.is_Tutorial_Attendance = false;
            }
        }       
    }

    /// <summary>
    /// 일일퀘스트 창 튜토리얼을 마친 뒤에 발생하는 이벤트 메서드 체이닝 (상점 뽑기)
    /// </summary>
   private void Tutorial_Daily_Close_And_Goto_Shop()
    {
        if (Utils.is_Tutorial)
        {
            Start_Tutorial(Shop_Button);
        }
       
    }

}
