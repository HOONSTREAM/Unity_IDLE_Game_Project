using System.Collections;
using System.Collections.Generic;
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
    private Button AD_Package_Button;
    [SerializeField]
    private Button Setting_Button;
    [SerializeField]
    private Button Daily_Quest_Button;

    [HideInInspector]
    public Item_ToolTip item_tooltip = null;
    [HideInInspector]
    public UI_Base UI;
    public static bool isSavingMode = false;

    /// <summary>
    /// 오프라인 보상 시작 시간을 지정합니다. (Second 단위)
    /// </summary>
    private const double START_OFFLINE_TIME = 1.0d; 

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
        Base_Manager.SOUND.Play(Sound.BGM, "Main");

        //빌드 시 ArgumentNullException: Value cannot be null; 발생.DateTime.parse 에러인듯.
        if (Utils.Offline_Timer_Check() >= START_OFFLINE_TIME)
        {
            Get_UI("OFFLINE_REWARD");
            Base_Manager.SOUND.Play(Sound.BGS, "OFFLINE");
        }


        Hero_Button.onClick.AddListener(() => Get_UI("@Heros", true, false, true, 1));
        Relic_Button.onClick.AddListener(() => Get_UI("UI_RELIC", false, false, true, 2));
        Inventory_Button.onClick.AddListener(() => Get_UI("UI_INVENTORY"));
        Saving_Mode_Button.onClick.AddListener(() => {
            Get_UI("Saving_Mode");
            isSavingMode = true;
        });
        ADS_Buff_Button.onClick.AddListener(() => { Get_UI("ADS_Buff"); });
        Shop_Button.onClick.AddListener(() => Get_UI("Shop", false, true, true, 5));
        Dungeon_Button.onClick.AddListener(() => Get_UI("UI_Dungeon", false, false, true, 3));
        Smelt_Button.onClick.AddListener(() => Get_UI("UI_Smelt", false, false, true, 4));
        Status_Button.onClick.AddListener(() => Get_UI("@Status", false, false, true, 0));
        AD_Package_Button.onClick.AddListener(() => Get_UI("AD_REMOVE_PACKAGE", false, false, true));
        Setting_Button.onClick.AddListener(() => Get_UI("UI_Setting", false, false, true));
        Daily_Quest_Button.onClick.AddListener(() => Get_UI("UI_Daliy_Quest", false, false, true));

    }
    private void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Utils.UI_Holder.Count > 0)
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

    public UI_Toast_Popup Get_Toast_Popup()
    {
        return Instantiate(Resources.Load<UI_Toast_Popup>("UI/Popup"), transform); //transform은 해당위치에 생성하라는 인자
    }

    public UI_TOP_POPUP Get_TOP_Popup()
    {
        return Instantiate(Resources.Load<UI_TOP_POPUP>("UI/TOP_POPUP"), transform); //transform은 해당위치에 생성하라는 인자
    }
}
