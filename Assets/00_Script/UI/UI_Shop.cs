using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : UI_Base
{

    public static Action UI_Shop_First_Opened;
    public static Action UI_Shop_First_Closed;

    #region 확률정보 패널
    [Header("Gacha_Percent_Info")]
    [SerializeField]
    private GameObject Infomation_Panel;
    [SerializeField]
    private GameObject Information_Panel_Relic;
    [SerializeField]
    private GameObject Dia_Gacha_Infomation_Panel;
    [SerializeField]
    private TextMeshProUGUI[] Percentage_Text;
    [SerializeField]
    private TextMeshProUGUI Summon_Level_Text;
    [SerializeField]
    private TextMeshProUGUI[] Percentage_Text_Relic;
    [SerializeField]
    private TextMeshProUGUI Relic_Summon_Level_Text;
    #endregion

    [Space(20f)]
    [Header("Shop")]
    [SerializeField]
    private TextMeshProUGUI Real_Summon_Level_Text; // 실제 유저의 소환레벨
    [SerializeField]
    private TextMeshProUGUI Summon_Level_Count_Text;
    [SerializeField]
    private TextMeshProUGUI Pickup_Text; // 확정소환 횟수
    [SerializeField]
    private Image Summon_Level_Count_Slider;
    [SerializeField]
    private Image Pickup_Count_Slider;
    [SerializeField]
    private Button Hero_Summon_Button_11;
    [SerializeField]
    private Button Hero_Summon_Button_55;
    [SerializeField]
    private Button Exit_Button;
    [SerializeField]
    private TextMeshProUGUI ADS_Hero_Count;
    [SerializeField]
    private TextMeshProUGUI[] ADS_Timer_Text;
    [SerializeField]
    private GameObject[] ADS_Summon_Lock;

    [Space(20f)]
    [Header("Relic Shop")]
    [SerializeField]
    private TextMeshProUGUI Real_Relic_Summon_Level_Text; // 실제 유저의 소환레벨
    [SerializeField]
    private TextMeshProUGUI Relic_Summon_Level_Count_Text;
    [SerializeField]
    private TextMeshProUGUI Relic_Pickup_Text; // 확정소환 횟수
    [SerializeField]
    private Image Relic_Summon_Level_Count_Slider;
    [SerializeField]
    private Image Relic_Pickup_Count_Slider;
    [SerializeField]
    private Button Relic_Summon_Button_11;
    [SerializeField]
    private Button Relic_Summon_Button_55;
    [SerializeField]
    private TextMeshProUGUI ADS_Relic_Count;
    [SerializeField]
    private TextMeshProUGUI ADS_Relic_Timer_Text;
    [SerializeField]
    private GameObject TODAY_PACKAGE_SOLD_OUT;
    [SerializeField]
    private GameObject TODAY_PACKAGE_SOLD_OUT_STRONG;
    [SerializeField]
    private GameObject ADS_PACKAGE_OBJ;
    [SerializeField]
    private GameObject START_PACKAGE_OBJ;
    [SerializeField]
    private GameObject DIAMOND_PACKAGE_SOLD_OUT;
    [SerializeField]
    private GameObject DIAMOND_PURCHASE_NOT_OBJ; // 다이아몬드 티어 이하일 때, 구매 불가
    [SerializeField]
    private GameObject DIAMOND_GACHA_SOLD_OUT_OBJ;
    [SerializeField]
    private TextMeshProUGUI DIAMOND_GACHA_COUNT_TEXT;

    [SerializeField]
    private GameObject Tutorial_Panel;

    [Space(20f)]
    [Header("Money_Amount")]
    [SerializeField]
    private TextMeshProUGUI Dia_Amount;

    private int Information_Panel_Summon_Level;
    private int Information_Panel_Relic_Summon_Level;

    private const int GACHA_PRICE_11 = 500;
    private const int GACHA_PRICE_55 = 2500;

    private void Awake()
    {
        if (Utils.is_Tutorial)
        {
            UI_Shop_First_Opened -= Start_Tutorial_Hero_Gacha;
            UI_Shop_First_Opened += Start_Tutorial_Hero_Gacha;
            UI_Gacha.Pressed_Tutorial_Gacha_Close_Button -= Start_Tutorial_Hero_Set;
            UI_Gacha.Pressed_Tutorial_Gacha_Close_Button += Start_Tutorial_Hero_Set;
            UI_Gacha.Pressed_Tutorial_Gacha_Close_Button -= End_Tutorial;
            UI_Gacha.Pressed_Tutorial_Gacha_Close_Button += End_Tutorial;
        }
              
    }

    public override bool Init()
    {
        Dia_Amount.text = Data_Manager.Main_Players_Data.DiaMond.ToString();

        Set_Summon_Button();      
        Get_Init();
        Relic_Init();
      
        return base.Init();
    }

    private void Update()
    {
        for (int i = 0; i < Data_Manager.Main_Players_Data.ADS_Timer.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.ADS_Timer[i] > 0.0f)
            {
                ADS_Timer_Text[i].text = Utils.GetTimer(Data_Manager.Main_Players_Data.ADS_Timer[i]);
                ADS_Summon_Lock[i].gameObject.SetActive(true);             
            }

            else
            {
                ADS_Summon_Lock[i].gameObject.SetActive(false);              
            }
        }
    }

    public void Get_IAP_Product(string purchase_name)
    {
        Base_Manager.IAP.Purchase(purchase_name, () =>
        {
            Init();
        });        
    }

    #region Gacha_Button_Method
    public void GachaButton(int value, bool ADS = false)
    {
        StartCoroutine(GaCha_Start_Delay_Coroutine(value, ADS));             
    }

    /// <summary>
    /// 광고 종료 후 Unity의 그래픽 디바이스가 완전히 복구되기 전에 UI를 불러오면서 충돌이 발생할 수 있음.
    /// 코루틴을 활용하여 0.5초간 딜레이 실행.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="ADS"></param>
    /// <returns></returns>
    IEnumerator GaCha_Start_Delay_Coroutine(int value, bool ADS)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Base_Manager.SOUND.Play(Sound.BGS, "Gacha");
        if (ADS == true)
        {
            
            Base_Canvas.instance.Get_UI("GaCha");
            var UI = Utils.UI_Holder.Peek().gameObject.GetComponent<UI_Gacha>(); // Get_UI로 소환한 Gacha 오브젝트를 가져온다.
            UI.Get_Gacha_Hero(value, true);

            Init();
        }

        else
        {
            switch (value)
            {
                case 11:
                    if (Data_Manager.Main_Players_Data.DiaMond < GACHA_PRICE_11)
                    {
                        Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드가 부족합니다.");
                        yield break;
                    }
                    break;
                case 55:
                    if (Data_Manager.Main_Players_Data.DiaMond < GACHA_PRICE_55)
                    {
                        Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드가 부족합니다.");
                        yield break;
                    }
                    break;
            }

            Base_Canvas.instance.Get_UI("GaCha");
            var UI = Utils.UI_Holder.Peek().gameObject.GetComponent<UI_Gacha>(); // Get_UI로 소환한 Gacha 오브젝트를 가져온다.
            UI.Get_Gacha_Hero(value);

        }
    }
    public void GachaButton_ADS()
    {
        if (Data_Manager.Main_Players_Data.ADS_Hero_Summon_Count >= 3)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("금일 최대 횟수를 이용하였습니다. 자정이후 초기화됩니다.");
            return;
        }

        Data_Manager.Main_Players_Data.ADS_Hero_Summon_Count++;
        Data_Manager.Main_Players_Data.ADS_Timer[0] = 900.0f;
        Base_Manager.ADS.ShowRewardedAds(() => GachaButton(11, true));

        Init();
    }
    public void ADS_Free_Dia_Button()
    {
        if (Data_Manager.Main_Players_Data.ADS_FREE_DIA)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("금일 최대 횟수를 이용하였습니다. 자정이후 초기화됩니다.");
            return;
        }

        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            StartCoroutine(WaitForUIRewardAndGive("Dia", 800));
        });

        Data_Manager.Main_Players_Data.ADS_FREE_DIA = true;
        Base_Manager.BACKEND.Log_Get_Dia("ADS_FREE_DIA_SHOP");

        Init();
    }
    public void ADS_Free_Steel_Button()
    {
        if (Data_Manager.Main_Players_Data.ADS_FREE_STEEL)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("금일 최대 횟수를 이용하였습니다. 자정이후 초기화됩니다.");
            return;
        }

        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            StartCoroutine(WaitForUIRewardAndGive("Steel", 150));
        });

        Data_Manager.Main_Players_Data.ADS_FREE_STEEL = true;

        Init();
    }
    private IEnumerator WaitForUIRewardAndGive(string rewardType, int amount)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        // UI 열기 시도 (비동기 생성)
        Base_Canvas.instance.Get_UI("UI_Reward");

        float timeout = 2f;
        float elapsed = 0f;

        // UI_Holder에 Reward UI가 올라올 때까지 최대 2초 대기
        while (elapsed < timeout)
        {
            var top = Utils.UI_Holder.Peek();
            if (top != null && top.GetComponent<UI_Reward>() != null)
            {
                var reward = top.GetComponent<UI_Reward>();
                reward.GetRewardInit(rewardType, amount);
                yield break;
            }

            yield return null;
            elapsed += Time.unscaledDeltaTime;
        }

        // 실패 시 예외 메시지 출력
        Debug.LogError("UI_Reward 로딩 타임아웃");
        Base_Canvas.instance.Get_Toast_Popup().Initialize("보상 UI를 불러오지 못했습니다.");
    }
    public void Get_Free_Scroll_Comb_Button()
    {
        if (Data_Manager.Main_Players_Data.FREE_COMB_SCROLL == true)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("금일 최대 횟수를 이용하였습니다. 자정이후 초기화됩니다.");
            return;
        }

        Data_Manager.Main_Players_Data.FREE_COMB_SCROLL = true;
        Base_Manager.Data.Item_Holder["Scroll_Comb"].Hero_Card_Amount += 2000;

        Base_Canvas.instance.Get_TOP_Popup().Initialize("보상 획득 완료!");

        Init();
    }
    public void Get_Free_Dia_Button()
    {
        if (Data_Manager.Main_Players_Data.FREE_DIA == true)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("금일 최대 횟수를 이용하였습니다. 자정이후 초기화됩니다.");
            return;
        }

        Data_Manager.Main_Players_Data.FREE_DIA = true;
        Data_Manager.Main_Players_Data.DiaMond += 500;
        Base_Manager.BACKEND.Log_Get_Dia("FREE_DIA_SHOP");
        Base_Canvas.instance.Get_TOP_Popup().Initialize("보상 획득 완료!");

        Init();
    }
    public void GachaButton_Relic(int value, bool ADS = false)
    {
        StartCoroutine(Relic_Gacha_Delay_Coroutine(value,ADS));
    }
    IEnumerator Relic_Gacha_Delay_Coroutine(int value, bool ADS)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Base_Manager.SOUND.Play(Sound.BGS, "Gacha");

        if (ADS == true)
        {
            Base_Canvas.instance.Get_UI("GaCha_Relic");
            var UI = Utils.UI_Holder.Peek().gameObject.GetComponent<UI_Relic_Gacha>(); // Get_UI로 소환한 Gacha 오브젝트를 가져온다.
            UI.Get_Gacha_Relic(value, true);

            Init();
        }

        else
        {
            switch (value)
            {
                case 11:
                    if (Data_Manager.Main_Players_Data.DiaMond < GACHA_PRICE_11)
                    {
                        Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드가 부족합니다.");
                        yield break;
                    }
                    break;
                case 55:
                    if (Data_Manager.Main_Players_Data.DiaMond < GACHA_PRICE_55)
                    {
                        Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드가 부족합니다.");
                        yield break;
                    }

                    break;
            }


            Base_Canvas.instance.Get_UI("GaCha_Relic");
            var UI = Utils.UI_Holder.Peek().gameObject.GetComponent<UI_Relic_Gacha>(); // Get_UI로 소환한 Gacha 오브젝트를 가져온다.
            UI.Get_Gacha_Relic(value);

        }
    }
    public void GachaButton_Relic_ADS()
    {
        if(Data_Manager.Main_Players_Data.ADS_Relic_Summon_Count >= 3)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("금일 최대 횟수를 이용하였습니다. 자정이후 초기화됩니다.");
            return;
        }
        Data_Manager.Main_Players_Data.ADS_Relic_Summon_Count++;
        Data_Manager.Main_Players_Data.ADS_Timer[1] = 900.0f;
        Base_Manager.ADS.ShowRewardedAds(() => GachaButton_Relic(11, true));

        Init();
    }
    #endregion
    private void Set_Summon_Button()
    {
        Hero_Summon_Button_11.onClick.RemoveAllListeners();
        Hero_Summon_Button_55.onClick.RemoveAllListeners();
        Relic_Summon_Button_11.onClick.RemoveAllListeners();
        Relic_Summon_Button_55.onClick.RemoveAllListeners();

        Hero_Summon_Button_11.onClick.AddListener(() => GachaButton(11));
        Hero_Summon_Button_55.onClick.AddListener(() => GachaButton(55));
        Relic_Summon_Button_11.onClick.AddListener(() => GachaButton_Relic(11));
        Relic_Summon_Button_55.onClick.AddListener(() => GachaButton_Relic(55));
    }
    private void Get_Init()
    {
        TODAY_PACKAGE_SOLD_OUT.gameObject.SetActive(false);
        TODAY_PACKAGE_SOLD_OUT_STRONG.gameObject.SetActive(false);
        ADS_PACKAGE_OBJ.gameObject.SetActive(true);
        START_PACKAGE_OBJ.gameObject.SetActive(true);
        DIAMOND_PACKAGE_SOLD_OUT.gameObject.SetActive(false);
        DIAMOND_PURCHASE_NOT_OBJ.gameObject.SetActive(false);

        DIAMOND_GACHA_COUNT_TEXT.text = $"( {Data_Manager.Main_Players_Data.DIA_GACHA_COUNT} / 3 )";

        ADS_Hero_Count.text = "(" + Data_Manager.Main_Players_Data.ADS_Hero_Summon_Count.ToString() + "/3)";

        Real_Summon_Level_Text.text = "영웅 소환 레벨 Lv." + 
            (Utils.Calculate_Summon_Level(Data_Manager.Main_Players_Data.Hero_Summon_Count) + 1).ToString();

       

        int level = Utils.Calculate_Summon_Level(Data_Manager.Main_Players_Data.Hero_Summon_Count);
        if (level < 9)
        {
            int valueCount = Data_Manager.Main_Players_Data.Hero_Summon_Count;
            int MaximumValueCount = Utils.summon_level[level];
            Summon_Level_Count_Text.text = "(" + valueCount.ToString() + "/" + MaximumValueCount.ToString() + ")";
            Summon_Level_Count_Slider.fillAmount = (float)valueCount / (float)MaximumValueCount;
        }
        else if (level >= 9)
        {
            Summon_Level_Count_Text.text = "Max Level";
            Summon_Level_Count_Slider.fillAmount = 1.0f;
        }

        int valuePickUp = Data_Manager.Main_Players_Data.Hero_Pickup_Count;
        Pickup_Text.text = "(" + valuePickUp.ToString() + "/110)";
        Pickup_Count_Slider.fillAmount = (float)valuePickUp / 110.0f;

        if (Data_Manager.Main_Players_Data.isBuyADPackage)
        {
            ADS_PACKAGE_OBJ.gameObject.SetActive(false);
        }

        if (Data_Manager.Main_Players_Data.isBuyTodayPackage)
        {
            TODAY_PACKAGE_SOLD_OUT.gameObject.SetActive(true);
        }

        if (Data_Manager.Main_Players_Data.isBuySTRONGPackage)
        {
            TODAY_PACKAGE_SOLD_OUT_STRONG.gameObject.SetActive(true);
        }

        if (Data_Manager.Main_Players_Data.isBuySTARTPackage)
        {
            START_PACKAGE_OBJ.gameObject.SetActive(false);
        }

        if (Data_Manager.Main_Players_Data.isBuyDIAMONDPackage)
        {
            DIAMOND_PACKAGE_SOLD_OUT.gameObject.SetActive(true);
        }

        if (Data_Manager.Main_Players_Data.Player_Tier < Player_Tier.Tier_Diamond)
        {
            DIAMOND_PURCHASE_NOT_OBJ.gameObject.SetActive(true);
            DIAMOND_PACKAGE_SOLD_OUT.gameObject.SetActive(false);
        }

        if(Data_Manager.Main_Players_Data.DIA_GACHA_COUNT >= 3)
        {
            DIAMOND_GACHA_SOLD_OUT_OBJ.gameObject.SetActive(true);
        }

    }
    private void Relic_Init()
    {
        ADS_Relic_Count.text = "(" + Data_Manager.Main_Players_Data.ADS_Relic_Summon_Count.ToString() + "/3)";

        Real_Relic_Summon_Level_Text.text = "유물 소환 레벨 Lv." +
           (Utils.Calculate_Summon_Level(Data_Manager.Main_Players_Data.Relic_Summon_Count) + 1).ToString();



        int level = Utils.Calculate_Summon_Level(Data_Manager.Main_Players_Data.Relic_Summon_Count);
        if (level < 9)
        {
            int valueCount = Data_Manager.Main_Players_Data.Relic_Summon_Count;
            int MaximumValueCount = Utils.summon_level[level];
            Relic_Summon_Level_Count_Text.text = "(" + valueCount.ToString() + "/" + MaximumValueCount.ToString() + ")";
            Relic_Summon_Level_Count_Slider.fillAmount = (float)valueCount / (float)MaximumValueCount;
        }
        else if (level >= 9)
        {
            Relic_Summon_Level_Count_Text.text = "Max Level";
            Relic_Summon_Level_Count_Slider.fillAmount = 1.0f;
        }

        int valuePickUp = Data_Manager.Main_Players_Data.Relic_Pickup_Count;
        Relic_Pickup_Text.text = "(" + valuePickUp.ToString() + "/110)";
        Relic_Pickup_Count_Slider.fillAmount = (float)valuePickUp / 110.0f;
    }
    /// <summary>
    /// 소환 레벨에 따른 확률정보를 공시합니다.
    /// </summary>
    public void Get_Infomation_Panel()
    {
        Infomation_Panel.gameObject.SetActive(true);

        Percentage_Check(Utils.Calculate_Summon_Level(Data_Manager.Main_Players_Data.Hero_Summon_Count));
    }

    public void Get_Infomation_Panel_Relic()
    {
        Information_Panel_Relic.gameObject.SetActive(true);

        Percentage_Check_Relic(Utils.Calculate_Summon_Level(Data_Manager.Main_Players_Data.Relic_Summon_Count));
    }

    public void Get_Dia_Gacha_Information_Panel()
    {
        Dia_Gacha_Infomation_Panel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 화살표 방향에 따른 레벨에 따라, 소환 확률을 계산하여 나타내줍니다.
    /// </summary>
    /// <param name="value"></param>
    private void Percentage_Check(int value)
    {
        Information_Panel_Summon_Level = value;

        for (int i = 0; i < Percentage_Text.Length; i++)
        {
            Percentage_Text[i].text = string.Format("{0:0.00000}", CSV_Importer.Summon_Design[value][((Rarity)i).ToString()].ToString()) + "%";
        }

        Summon_Level_Text.text = "LEVEL." + (value + 1).ToString();

    }
    private void Percentage_Check_Relic(int value)
    {
        Information_Panel_Relic_Summon_Level = value;

        for (int i = 0; i < (Percentage_Text_Relic.Length); i++)
        {
            Percentage_Text_Relic[i].text = string.Format("{0:0.00000}", CSV_Importer.Summon_Design_Relic[value][((Rarity)i).ToString()].ToString()) + "%";
        }

        Relic_Summon_Level_Text.text = "LEVEL." + (value + 1).ToString();

    }
    public void  Level_Arrow_Button(int value)
    {
        Information_Panel_Summon_Level += value;

        
        if(Information_Panel_Summon_Level < 0)
        {
            Information_Panel_Summon_Level = 9;
        }
        else if(Information_Panel_Summon_Level > 9)
        {
            Information_Panel_Summon_Level = 0;
        }

        Percentage_Check(Information_Panel_Summon_Level);
    }
    public void Level_Arrow_Button_Relic(int value)
    {
        Information_Panel_Relic_Summon_Level += value;


        if (Information_Panel_Relic_Summon_Level < 0)
        {
            Information_Panel_Relic_Summon_Level = 9;
        }
        else if (Information_Panel_Relic_Summon_Level > 9)
        {
            Information_Panel_Relic_Summon_Level = 0;
        }

        Percentage_Check_Relic(Information_Panel_Relic_Summon_Level);
    }
    public void Disable_Information_Panel()
    {
        Infomation_Panel.gameObject.SetActive(false);
    }
    public void Disable_Information_Panel_Relic()
    {
        Information_Panel_Relic.gameObject.SetActive(false);
    }
    public void Disable_Dia_Gacha_Infomation_Panel()
    {
        Dia_Gacha_Infomation_Panel.gameObject.SetActive(false);
    }

    public void Start_Tutorial(Button original_Button)
    {
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

        if (Utils.is_Tutorial)
        {
            copy_Button.onClick = original_Button.onClick;
            copy_Button.onClick.AddListener(() =>
            {
                End_Tutorial();
                copy_Button.onClick.RemoveAllListeners();
            });
        }
        
    }

    private void End_Tutorial()
    {
        if (Tutorial_Panel == null) return;
        if (!Tutorial_Panel) return; // Unity 오브젝트가 Destroy 된 상태를 확인하는 방식

        // 자식 오브젝트 파괴 시 안전하게 처리
        for (int i = Tutorial_Panel.transform.childCount - 1; i >= 0; i--)
        {
            var child = Tutorial_Panel.transform.GetChild(i);
            if (child != null && child.gameObject != null)
            {
                Destroy(child.gameObject);
            }
        }

        Tutorial_Panel.SetActive(false);
    }

    private void Start_Tutorial_Hero_Gacha()
    {
        if (Utils.is_Tutorial)
        {
            Start_Tutorial(Hero_Summon_Button_11);
        }
        
    }
    private void Start_Tutorial_Hero_Set()
    {
        if (Utils.is_Tutorial)
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("영웅을 뽑았으니, 영웅을 배치해봅니다. 상점에서 나가볼까요?");
            Tutorial_Panel.gameObject.SetActive(false);
            Start_Tutorial(Exit_Button);
        }
       
    }
    public override void DisableOBJ()
    {
        if (Utils.is_Tutorial)
        {
            UI_Shop_First_Closed?.Invoke();
        }

        Main_UI.Instance.Layer_Check(-1);
        _ = Base_Manager.BACKEND.WriteData();
        base.DisableOBJ();
    }
}
