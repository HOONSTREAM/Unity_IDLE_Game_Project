using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : UI_Base
{
    #region 확률정보 패널
    [Header("Gacha_Percent_Info")]
    [SerializeField]
    private GameObject Infomation_Panel;
    [SerializeField]
    private TextMeshProUGUI[] Percentage_Text;
    [SerializeField]
    private TextMeshProUGUI Summon_Level_Text;
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
    private Button Hero_Summon_Button_1;
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
    private Button Relic_Summon_Button_1;
    [SerializeField]
    private TextMeshProUGUI ADS_Relic_Count;
    [SerializeField]
    private TextMeshProUGUI ADS_Relic_Timer_Text;
  


    [Space(20f)]
    [Header("Money_Amount")]
    [SerializeField]
    private TextMeshProUGUI Dia_Amount;

    private int Information_Panel_Summon_Level;

    private const int GACHA_PRICE_11 = 500;
   

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

    #region Gacha_Button_Method
    public void GachaButton(int value, bool ADS = false)
    {
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
                        return;
                    }                   
                    break;
                case 1:
                    if (Data_Manager.Main_Players_Data.DiaMond < (GACHA_PRICE_11/10))
                    {
                        Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드가 부족합니다.");
                        return;
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
    public void GachaButton_Relic(int value, bool ADS = false)
    {
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
                        return;
                    }
                    break;
                case 1:
                    if (Data_Manager.Main_Players_Data.DiaMond < (GACHA_PRICE_11/10))
                    {
                        Base_Canvas.instance.Get_Toast_Popup().Initialize("다이아몬드가 부족합니다.");
                        return;
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
        Hero_Summon_Button_1.onClick.RemoveAllListeners();
        Relic_Summon_Button_11.onClick.RemoveAllListeners();
        Relic_Summon_Button_1.onClick.RemoveAllListeners();

        Hero_Summon_Button_11.onClick.AddListener(() => GachaButton(11));
        Hero_Summon_Button_1.onClick.AddListener(() => GachaButton(1));
        Relic_Summon_Button_11.onClick.AddListener(() => GachaButton_Relic(11));
        Relic_Summon_Button_1.onClick.AddListener(() => GachaButton_Relic(1));
    }
    private void Get_Init()
    {
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
    public void Disable_Information_Panel()
    {
        Infomation_Panel.gameObject.SetActive(false);
    }  
    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }
}
