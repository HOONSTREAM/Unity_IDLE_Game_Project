using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using BackEnd;

public class Main_UI : MonoBehaviour
{
    public static Main_UI Instance = null;

    #region Parameter
    [Space(20f)]
    [Header("Default")]
    [SerializeField]
    private TextMeshProUGUI Tier_Text;
    [SerializeField]
    private Image Tier_Image;
    [SerializeField]
    private TextMeshProUGUI _level_Text; // 캐릭터의 레벨을 결정합니다.
    [SerializeField]
    private TextMeshProUGUI Main_Char_HP_Text;
    [SerializeField]
    private TextMeshProUGUI _player_ability; // 캐릭터의 최종 전투력을 결정합니다.
    [SerializeField]
    private TextMeshProUGUI _levelup_money_text; // 캐릭터의 레벨업에 필요한 돈을 결정합니다.
    [SerializeField]
    private TextMeshProUGUI _gold_text;
    [SerializeField]
    private TextMeshProUGUI _dia_text;
    [SerializeField]
    private TextMeshProUGUI _stage_Text;
    [SerializeField]
    private TextMeshProUGUI _stage_repeat_text;
    private Color StageColor = new Color(0.1824517f, 1.0f, 0.0f, 1.0f);
    [SerializeField]
    private TextMeshProUGUI _boss_stage_text;
    [SerializeField]
    private TextMeshProUGUI _char_nick_name;
    [SerializeField]
    private Button Mode_Change_Button;

    private Character Cleric_Component;

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
    [Header("Dungeon_Slider")]
    [SerializeField]
    private GameObject Dungeon_Slider_GameObject;
    [SerializeField]
    private TextMeshProUGUI M_Dungeon_HP_Text;
    [SerializeField]
    private Image Dungeon_Slider;
    [SerializeField]
    private TextMeshProUGUI Dungeon_Stage_Text;
    [SerializeField]
    private GameObject[] Dungeon_Addtional_Sliders;
    [SerializeField]
    private TextMeshProUGUI MonsterCountText;
    [SerializeField]
    private Image Gold_Dungeon_Slider_Fill;
    [SerializeField]
    private TextMeshProUGUI Gold_Dungeon_Hp_Text;
    [SerializeField]
    private Image Tier_Dungeon_Slider_Fill;
    [SerializeField]
    private TextMeshProUGUI Tier_Dungeon_Hp_Text;
    [SerializeField]
    private TextMeshProUGUI DPS_Dungeon_Hp_Text;

    private Coroutine Dungeon_Coroutine = null;

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

    [Space(20f)]
    [Header("TUTORIAL")]
    [SerializeField]
    private GameObject Hand_Icon_Daily_Quest;
    [SerializeField]
    private GameObject Tutorial_Panel;


    private List<TextMeshProUGUI> Bottom_Popup_Text = new List<TextMeshProUGUI>();
    private List<Coroutine> Bottom_Popup_Coroutine = new List<Coroutine>();

    private bool isPopup = false;
    private Coroutine Legendary_Coroutine;


    private float clearPoolTimer = 0f;
    private bool Can_Boss_Try = false;
    double Accmulate_DMG = default;

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
        Show_Tutorial_First_Game_Start();
       
        Cleric_Component = GameObject.Find("Cleric").gameObject.GetComponent<Character>();

        Main_UI_PlayerInfo_Text_Check();
        Monster_Slider_Count();       
        ADS_Buff_Check();
     
        for (int i = 0 ; i<ItemContent.childCount; i++)
        {
            Bottom_Popup_Text.Add(ItemContent.GetChild(i).GetComponent<TextMeshProUGUI>());
            Bottom_Popup_Coroutine.Add(null);
        }

        Base_Manager.Stage.M_ReadyEvent += OnReady;
        Base_Manager.Stage.M_BossEvent += OnBoss;
        Base_Manager.Stage.M_ClearEvent += OnClear;
        Base_Manager.Stage.M_DeadEvent += OnDead;
        Base_Manager.Stage.M_DungeonEvent += OnDungeon;
        Base_Manager.Stage.M_DungeonClearEvent += OnDungeonClear;        
        Base_Manager.Stage.M_DungeonDeadEvent += OnDungeonDead;

        Base_Manager.Stage.State_Change(Stage_State.Ready);

        Set_User_Nick_Name();

        if (PlayerPrefs.GetInt("EFFECT") == 1)
        {
            Utils.is_Skill_Effect_Save_Mode = true;
        }

        if (PlayerPrefs.GetInt("EFFECT") == 0)
        {
            Utils.is_Skill_Effect_Save_Mode = false;
        }

    }
    private void Update()
    {
        Check_ADS_Fast_Mode();

        if (Stage_Manager.isDead)
        {
            clearPoolTimer += Time.unscaledDeltaTime;
            if (clearPoolTimer >= 30f)
            {
                Base_Manager.Pool.Clear_Pool();
                clearPoolTimer = 0f; // 다음 30초를 위해 초기화
            }
        }
        else
        {
            clearPoolTimer = 0f; // isDead가 아니면 타이머 초기화
        }
    }
  
    private void Check_ADS_Fast_Mode()
    {
        if (Data_Manager.Main_Players_Data.buff_x2_speed > 0.0f)
        {
            X2_Speed_Fill.gameObject.SetActive(true);
            X2_Speed_Fill.fillAmount = Data_Manager.Main_Players_Data.buff_x2_speed / 1800.0f;
            X2_Time_Text.text = Utils.GetTimer(Data_Manager.Main_Players_Data.buff_x2_speed);
            Fast_Mode_Lock_Image.gameObject.SetActive(!Data_Manager.Main_Players_Data.isFastMode);
            Time.timeScale = Data_Manager.Main_Players_Data.isFastMode ? 1.6f : 1.0f;
        }

        else
        {
            Data_Manager.Main_Players_Data.isFastMode = false;
            X2_Speed_Fill.gameObject.SetActive(false);
            X2_Time_Text.text = default;
            Fast_Mode_Lock_Image.gameObject.SetActive(!Data_Manager.Main_Players_Data.isFastMode);
            Time.timeScale = Data_Manager.Main_Players_Data.isFastMode ? 1.5f : 1.0f;
        }
    }
    private void Set_User_Nick_Name()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        string temp = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        _char_nick_name.text = temp;
    }
    public void Layer_Check(int value)
    {
        if(value != -1)
        {
            StartCoroutine(Button_Image_Move_Coroutine(value));
        }
       

        for (int i = 0; i < Button_Images.Length; i++)
        {
            if(value != i && Button_Images[i].localScale.x >= 1.2f)
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
     
    }

    /// <summary>
    /// 광고를 시청하고, 게임 두배속 효과를 획득합니다.
    /// </summary>
    public void Get_Fast_Mode()
    {
        bool fast = Data_Manager.Main_Players_Data.isFastMode;

        if(fast == false)
        {
            if (Data_Manager.Main_Players_Data.buff_x2_speed <= 0.0f)
            {
                Base_Manager.ADS.ShowRewardedAds(() =>
                {
                    if(Data_Manager.Main_Players_Data.isBuyADPackage == false)
                    {
                        Data_Manager.Main_Players_Data.buff_x2_speed = 1800.0f;
                        Data_Manager.Main_Players_Data.isFastMode = true;
                    }

                    else
                    {
                        Data_Manager.Main_Players_Data.buff_x2_speed = 43200.0f;
                        Data_Manager.Main_Players_Data.isFastMode = true;
                    }
                                                                                          
                });
            }

            Time.timeScale = Data_Manager.Main_Players_Data.isFastMode ? 1.6f : 1.0f;
        }

        else
        {

        }
         
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
                Bottom_Popup_Text[i].text = Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>" + "획득";

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
                    Bottom_Popup_Text[i].text = Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>" + "획득";
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
        if (!Can_Boss_Try)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("최소 10초 후에 도전 가능 합니다.");
            return;
        }

        Can_Boss_Try = false;
        Stage_Manager.isDead = false;
        Base_Manager.Stage.State_Change(Stage_State.Boss);
    }

    IEnumerator Set_Boss_Coroutine_Timer()
    {
        yield return new WaitForSecondsRealtime(10.0f);
        Can_Boss_Try = true;
    }

    /// <summary>
    /// 즉시 방치형모드로 전환합니다.
    /// </summary>
    public void Set_Mode_Change_Idle_Mode()
    {

        Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        Stage_Manager.isDead = true;
        Base_Manager.Stage.State_Change(Stage_State.Ready);
        Slider_Object_Check(Slider_Type.Default);
        Mode_Change_Button.gameObject.SetActive(false);
        

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

    public void Dungeon_Monster_Slider_Count()
    {
        MonsterCountText.text = "(" + Stage_Manager.DungeonCount.ToString() + "/30)";

        if(Stage_Manager.DungeonCount <= 0)
        {
            Base_Manager.Stage.State_Change(Stage_State.Dungeon_Clear,Stage_Manager.Dungeon_Enter_Type);
            Base_Manager.BACKEND.Log_Clear_Dungeon(Stage_Manager.Dungeon_Enter_Type);
        }
    }


    public void Boss_Slider_Count(double hp = default, double MaxHp = default, double dmg = default)
    {
        float value = (float)hp / (float)MaxHp;       
        Accmulate_DMG += dmg;

        if (value <= 0.0f)
        {
            value = 0.0f;
        }


        if(Stage_Manager.isDungeon == false)
        {
            Boss_Slider.fillAmount = value;
            M_Boss_HP_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
        }
        else
        {
            switch (Stage_Manager.Dungeon_Enter_Type)
            {
                case 1:
                    Gold_Dungeon_Slider_Fill.fillAmount = value;
                    Gold_Dungeon_Hp_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
                    break;
                case 2:
                    Tier_Dungeon_Slider_Fill.fillAmount = value;
                    Tier_Dungeon_Hp_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
                    break;
                case 3:                   
                    DPS_Dungeon_Hp_Text.text = StringMethod.ToCurrencyString(Accmulate_DMG);
                    break;

            }
            
        }
    }
       
    private void Slider_Object_Check(Slider_Type type)
    {
        Monster_Slider_GameObject.gameObject.SetActive(false);
        Boss_Slider_GameObject.gameObject.SetActive(false);
        Dungeon_Slider_GameObject.gameObject.SetActive(false);

        if (Stage_Manager.isDead) // 데드프레임 생성 및 방치모드 버튼 해제
        {
            Dead_Frame.gameObject.SetActive(true);
            StartCoroutine(Set_Boss_Coroutine_Timer());
            Mode_Change_Button.gameObject.SetActive(false);
            Base_Canvas.instance.Get_UI("UI_Dead");
            return;
        }

        switch (type)
        {
            case Slider_Type.Default:
                Monster_Slider_GameObject.gameObject.SetActive(true);
                Monster_Slider_Count();
                break;
            case Slider_Type.Boss:
                Boss_Slider_GameObject.gameObject.SetActive(true);
                break;
            case Slider_Type.Dungeon:
                Dungeon_Slider_GameObject.gameObject.SetActive(true);             
                Dungeon_Coroutine = StartCoroutine(Dungeon_Slider_Coroutine());

                break;
        }

        Dead_Frame.gameObject.SetActive(false);

        float value = type == Slider_Type.Default ? 0.0f : 1.0f;
        Boss_Slider_Count(value, 1.0f);

    }
    private void OnReady()
    {
        FadeInOut(true);
        Monster_Slider_Count();

        if (!Stage_Manager.isDead)
        {
            Mode_Change_Button.gameObject.SetActive(true);
        }
        
        Parts_Initialize();        
    }

    public void Parts_Initialize()
    {
        Main_Parts_Dict.Clear();

        for (int i = 0; i < Base_Manager.Character.Set_Character.Length; i++)        
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

                if (Character_Spawner.players[i] != null) // 유효성검사
                {
                    Main_Parts_Dict.Add(Character_Spawner.players[i], main_hero_parts[i]);
                }
                else
                {
                    Debug.LogError($"{Character_Spawner.players[i]} 가 null 입니다.");
                }

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
                main_hero_parts[i].Init_Data(Data.Data, true); // 진영교체
                main_hero_parts[i].transform.SetSiblingIndex(indexValue);              
            }
        }
    }

    /// <summary>
    /// 각 영웅 별 HP와 MP를 검사합니다.
    /// </summary>
    public void Character_State_Check(Player player)
    {
        if (!Main_Parts_Dict.ContainsKey(player))
        {
            Debug.LogWarning($"플레이어 {player.name}에 대한 UI 요소가 존재하지 않습니다.");
            return;
        }

        var part = Main_Parts_Dict[player];

        // HP 또는 MP 변화가 있는 경우에만 UI 갱신
        if (part.LastKnownHP != player.HP || part.LastKnownMP != player.MP)
        {
            part.State_Check(player); // 내부에서 HP, MP UI 반영
            part.LastKnownHP = player.HP;
            part.LastKnownMP = player.MP;
        }
    }
    private void OnBoss()
    {
        Main_UI_PlayerInfo_Text_Check();
        Mode_Change_Button.gameObject.SetActive(false);
        Slider_Object_Check(Slider_Type.Boss);
    }
    private void OnClear()
    {
        Slider_Object_Check(Slider_Type.Default);        
        StartCoroutine(Clear_Delay());
    }
    private void OnDungeon(int Value)
    {
        Mode_Change_Button.gameObject.SetActive(false);

        for (int i = 0; i< Dungeon_Addtional_Sliders.Length; i++)
        {
            Dungeon_Addtional_Sliders[i].gameObject.SetActive(false);
        }

        Dungeon_Addtional_Sliders[Value].gameObject.SetActive(true);

        if(Value == 0)
        {
            int Stage_Value = (Stage_Manager.Dungeon_Level + 1); // 0층에서 시작하므로 +1을 해줍니다.
            Dungeon_Stage_Text.text = $"보물던전 {Stage_Value} 층";
        }

        else if (Value == 1)
        {
            int Stage_Value = (Stage_Manager.Dungeon_Level + 1); // 0층에서 시작하므로 +1을 해줍니다.
            Dungeon_Stage_Text.text = $"골드던전 {Stage_Value} 층";
        }

        else if (Value == 2)
        {
            Dungeon_Stage_Text.text = $"승급던전 도전 중";
        }

        else if (Value == 3)
        {
            Dungeon_Stage_Text.text = $"시험의탑 도전 중";
        }

        FadeInOut(true, true);
        Parts_Initialize();
        Dungeon_Addtional_Sliders[Value].gameObject.SetActive(true);

        Slider_Object_Check(Slider_Type.Dungeon);     
    }
    private void OnDungeonClear(int Value)
    {       
        Mode_Change_Button.gameObject.SetActive(false);

        if (Dungeon_Coroutine != null)
        {
            StopCoroutine(Dungeon_Coroutine);
            Dungeon_Coroutine = null;
        }
        int clear_Level = Stage_Manager.Dungeon_Level;

        if (Value == 0 || Value == 1)
        {
            
            if (Stage_Manager.Dungeon_Level == Data_Manager.Main_Players_Data.Dungeon_Clear_Level[Value]) // 클리어레벨이 최종클리어레벨과 동일할 때에만 레벨증가
            {
                Data_Manager.Main_Players_Data.Dungeon_Clear_Level[Value]++;

                if (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[Value] >= 100)
                {
                    Data_Manager.Main_Players_Data.Dungeon_Clear_Level[Value] = 100;
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("던전 최고 레벨에 도달하였습니다.");
                }
            }


            if (Data_Manager.Main_Players_Data.Daily_Enter_Key[Value] > 0)
            {
                Data_Manager.Main_Players_Data.Daily_Enter_Key[Value]--;
            }
            else
            {
                Data_Manager.Main_Players_Data.User_Key_Assets[Value]--;
            }

        }

        switch (Value)
        {
            case 0:

                Data_Manager.Main_Players_Data.DiaMond += ((clear_Level + 1) * Stage_Manager.MULTIPLE_REWARD_DIAMOND_DUNGEON);
                Base_Manager.BACKEND.Log_Get_Dia("Dia_Dungeon");
                _ = Base_Manager.BACKEND.WriteData();
                break;

            case 1:

                int levelCount = (clear_Level + 1);
                var value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, levelCount, Utils.Data.stageData.DROP_MONEY) * Stage_Manager.MULTIPLE_REWARD_GOLD_DUNGEON; ;

                Data_Manager.Main_Players_Data.Player_Money += value;
                _ = Base_Manager.BACKEND.WriteData();
                break;

            case 2:
                               
                Player_Tier tier = Data_Manager.Main_Players_Data.Player_Tier;
                Player_Tier next_tier = tier + 1;
                Data_Manager.Main_Players_Data.Player_Tier = next_tier;
                _ = Base_Manager.BACKEND.WriteData();
                break;

            case 3:

                Debug.Log($"{StringMethod.ToCurrencyString(Accmulate_DMG)}가 저장될 예정입니다.");

                if(Accmulate_DMG >= Data_Manager.Main_Players_Data.USER_DPS) 
                {
                    Data_Manager.Main_Players_Data.USER_DPS = Accmulate_DMG;
                }
               
                Accmulate_DMG = 0;
                _ = Base_Manager.BACKEND.WriteData();

                break;
                
        }

        Main_UI_PlayerInfo_Text_Check();


        Base_Canvas.instance.Get_TOP_Popup().Initialize("던전 클리어에 성공하였습니다!");
        Base_Manager.SOUND.Play(Sound.BGS, "Victory");
        
        OnClear();
    }
    private void OnDead()
    {
        Base_Canvas.instance.Get_TOP_Popup().Initialize("전투에서 패배하였습니다.");
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
        StartCoroutine(Dead_Delay());
    }

    private void OnDungeonDead()
    {
        Mode_Change_Button.gameObject.SetActive(false);

        if (Dungeon_Coroutine != null)
        {
            StopCoroutine(Dungeon_Coroutine);
            Dungeon_Coroutine = null;
        }

        OnDead();      
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
    /// 플레이어 레벨, 전투력, 골드, 스테이지 상태 의 텍스트를 업데이트 합니다.
    /// </summary>
    public void Main_UI_PlayerInfo_Text_Check()
    {
        double Levelup_money_Value = Utils.Data.levelData.Get_LEVELUP_MONEY();

        Tier_Text.text = Utils.Set_Tier_Name();
        Tier_Image.sprite = Utils.Get_Atlas(Data_Manager.Main_Players_Data.Player_Tier.ToString());

        _level_Text.text = "LV." + (Data_Manager.Main_Players_Data.Player_Level + 1).ToString();

        if(Cleric_Component.HP <= 0)
        {
            Cleric_Component.HP = 0;
        }

        Main_Char_HP_Text.text = StringMethod.ToCurrencyString(Cleric_Component.HP);
        _player_ability.text = StringMethod.ToCurrencyString(Base_Manager.Player.Player_ALL_Ability_ATK_HP());

        _levelup_money_text.text = StringMethod.ToCurrencyString((Levelup_money_Value));
        _levelup_money_text.color = Utils.Check_Levelup_Gold(Levelup_money_Value) ? Color.green : Color.red;

        _gold_text.text = StringMethod.ToCurrencyString(Data_Manager.Main_Players_Data.Player_Money);
        _dia_text.text = Data_Manager.Main_Players_Data.DiaMond.ToString();

        _stage_repeat_text.text = Stage_Manager.isDead ? "반복중 ..." : "진행중 ...";
        _stage_repeat_text.color = Stage_Manager.isDead ? Color.yellow : StageColor;

        int stage_Value = Data_Manager.Main_Players_Data.Player_Stage;

        
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

    private void Show_Tutorial_First_Game_Start()
    {
        if (Data_Manager.Main_Players_Data.Player_Money == 0.0d && Data_Manager.Main_Players_Data.Player_Level == 0)
        {
            Debug.Log("처음 시작하는 유저");
            StartCoroutine(Tutorial_Coroutine());
        }
    }
      

    #region Coroutine
    IEnumerator Dead_Delay()
    {
        yield return StartCoroutine(Clear_Delay());

        Slider_Object_Check(Slider_Type.Default);

        for(int i = 0; i<Spawner.m_monsters.Count; i++)
        {
            if (Spawner.m_monsters[i].isBoss == true)
            {
                Base_Manager.Pool.m_pool_Dictionary[Utils.GetStage_BossPrefab(Data_Manager.Main_Players_Data.Player_Stage)].Return(Spawner.m_monsters[i].gameObject);
            }
            else
            {
                Base_Manager.Pool.m_pool_Dictionary[Utils.GetStage_MonsterPrefab(Data_Manager.Main_Players_Data.Player_Stage)].Return(Spawner.m_monsters[i].gameObject);
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
    IEnumerator Dungeon_Slider_Coroutine(int Value = default)
    {
        float time = 30.0f;

        while(time >= 0.0f)
        {
            time -= Time.unscaledDeltaTime;
            Dungeon_Slider.fillAmount = time / 30.0f;
            M_Dungeon_HP_Text.text = string.Format("{0:0.00}초",time);
            yield return null;
        }

        if(Stage_Manager.Dungeon_Enter_Type == 3)
        {
            Base_Manager.Stage.State_Change(Stage_State.Dungeon_Clear, Stage_Manager.Dungeon_Enter_Type);
        }

        else
        {
            Base_Manager.Stage.State_Change(Stage_State.Dungeon_Dead);
        }
        
    }
    IEnumerator Tutorial_Coroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("영웅 파티 키우기 세계에 오신것을 환영합니다!");
        yield return new WaitForSecondsRealtime(3.0f);
        Base_Canvas.instance.Get_TOP_Popup().Initialize("일일퀘스트를 따라, 여정을 시작하세요!");
        Hand_Icon_Daily_Quest.gameObject.SetActive(true);      
    }
    #endregion
}
