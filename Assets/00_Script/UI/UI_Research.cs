using TMPro;
using UnityEngine;

public class UI_Research : UI_Base
{
    private const int RESEARCH_FIRST_MAX_LEVEL = 20;
    private const int RESEARCH_REQUEST_FIRST_ITEM_AMOUNT = 100;

    [SerializeField]
    private TextMeshProUGUI Research_All_Level_Text;
    [SerializeField]
    private TextMeshProUGUI Research_Level_ATK;
    [SerializeField]
    private TextMeshProUGUI Research_Level_HP;
    [SerializeField]
    private TextMeshProUGUI Research_Level_ATK_SPEED;
    [SerializeField]
    private TextMeshProUGUI Research_Level_GOLD;
    [SerializeField]
    private TextMeshProUGUI Research_Level_ITEM;
    [SerializeField]
    private TextMeshProUGUI Research_Level_CRI_DMG;
    [SerializeField]
    private TextMeshProUGUI Research_Level_CRI_PER;

    [SerializeField]
    private TextMeshProUGUI Research_ATK;
    [SerializeField]
    private TextMeshProUGUI Research_HP;
    [SerializeField]
    private TextMeshProUGUI Research_ATK_SPEED;
    [SerializeField]
    private TextMeshProUGUI Research_GOLD;
    [SerializeField]
    private TextMeshProUGUI Research_ITEM;
    [SerializeField]
    private TextMeshProUGUI Research_CRI_DMG;
    [SerializeField]
    private TextMeshProUGUI Research_CRI_PER;

    [SerializeField]
    private TextMeshProUGUI Button_ATK_Amount;
    [SerializeField]
    private TextMeshProUGUI Button_HP_Amount;
    [SerializeField]
    private TextMeshProUGUI Button_ATK_SPEED_Amount;
    [SerializeField]
    private TextMeshProUGUI Button_GOLD_Amount;
    [SerializeField]
    private TextMeshProUGUI Button_ITEM_Amount;
    [SerializeField]
    private TextMeshProUGUI Button_CRI_DMG_Amount;
    [SerializeField]
    private TextMeshProUGUI Button_CRI_PER_Amount;


    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        Research_All_Level_Text.text = $"종합 연구 레벨 : LV. {Base_Manager.Data.User_Main_Data_Research_Array[0].Research_ALL_LEVEL.ToString()}";
        Research_ATK.text = Base_Manager.Data.User_Main_Data_Research_Array[0].Research_Value.ToString();
        Research_HP.text = Base_Manager.Data.User_Main_Data_Research_Array[1].Research_Value.ToString();
        Research_ATK_SPEED.text = Base_Manager.Data.User_Main_Data_Research_Array[2].Research_Value.ToString();
        Research_GOLD.text = Base_Manager.Data.User_Main_Data_Research_Array[3].Research_Value.ToString();
        Research_ITEM.text = Base_Manager.Data.User_Main_Data_Research_Array[4].Research_Value.ToString();
        Research_CRI_DMG.text = Base_Manager.Data.User_Main_Data_Research_Array[5].Research_Value.ToString();
        Research_CRI_PER.text = Base_Manager.Data.User_Main_Data_Research_Array[6].Research_Value.ToString();

        Research_Level_ATK.text = $"{Base_Manager.Data.User_Main_Data_Research_Array[0].Research_Level} / " +
            $"{RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[0].Research_ALL_LEVEL + 1)}";

        Research_Level_HP.text =  $"{Base_Manager.Data.User_Main_Data_Research_Array[1].Research_Level} / " +
            $"{RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[1].Research_ALL_LEVEL + 1)}";

        Research_Level_ATK_SPEED.text =$"{Base_Manager.Data.User_Main_Data_Research_Array[2].Research_Level} / " +
            $"{RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[2].Research_ALL_LEVEL + 1)}";

        Research_Level_GOLD.text =  $"{Base_Manager.Data.User_Main_Data_Research_Array[3].Research_Level} / " + 
            $"{RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[3].Research_ALL_LEVEL + 1)}";

        Research_Level_ITEM.text =  $"{Base_Manager.Data.User_Main_Data_Research_Array[4].Research_Level} / " +
            $"{RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[4].Research_ALL_LEVEL + 1)}";

        Research_Level_CRI_DMG.text =  $"{Base_Manager.Data.User_Main_Data_Research_Array[5].Research_Level} / " +
            $"{RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[5].Research_ALL_LEVEL + 1)}";

        Research_Level_CRI_PER.text =  $"{Base_Manager.Data.User_Main_Data_Research_Array[6].Research_Level} / " +
            $"{RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[6].Research_ALL_LEVEL + 1)}";

        return base.Init();
    }
    public override void DisableOBJ()
    {
        _ = Base_Manager.BACKEND.WriteData();
        base.DisableOBJ();
    }

    public void test_Button(int Stat_Number)
    {
        if(Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level >= RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_ALL_LEVEL + 1))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("종합 연구 레벨이 부족합니다.");            
            return;
        }

        Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 1.28f;
        Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;

        Init();
        
    }

    public void Research_All_Level_Up_Button()
    {
        foreach(Research_Holder holder in Base_Manager.Data.User_Main_Data_Research_Array)
        {
            if(holder.Research_Level != RESEARCH_FIRST_MAX_LEVEL * holder.Research_ALL_LEVEL)
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("최고레벨 미달성 된 연구가 있습니다.");
                return;
            }
        }
    }
}
