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
        Research_ATK.text = StringMethod.ToCurrencyString(Base_Manager.Data.User_Main_Data_Research_Array[0].Research_Value);
        Research_HP.text = StringMethod.ToCurrencyString(Base_Manager.Data.User_Main_Data_Research_Array[1].Research_Value);
        Research_ATK_SPEED.text = $"{Base_Manager.Data.User_Main_Data_Research_Array[2].Research_Value.ToString("F2")}%";
        Research_GOLD.text = $"{Base_Manager.Data.User_Main_Data_Research_Array[3].Research_Value.ToString("F2")}%";
        Research_ITEM.text = $"{Base_Manager.Data.User_Main_Data_Research_Array[4].Research_Value.ToString("F2")}%";
        Research_CRI_DMG.text = $"{Base_Manager.Data.User_Main_Data_Research_Array[5].Research_Value.ToString("F2")}%";
        Research_CRI_PER.text = $"{Base_Manager.Data.User_Main_Data_Research_Array[6].Research_Value.ToString("F2")}%";

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


        Button_ATK_Amount.text = $"{RESEARCH_REQUEST_FIRST_ITEM_AMOUNT}";
        Button_ATK_Amount.color = Utils.Item_Count("Slime_Potion", 
            (RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)) ? Color.green : Color.red;

        Button_HP_Amount.text = $"{RESEARCH_REQUEST_FIRST_ITEM_AMOUNT}";
        Button_HP_Amount.color = Utils.Item_Count("Spider_Potion",
            (RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)) ? Color.green : Color.red;

        Button_ATK_SPEED_Amount.text = $"{RESEARCH_REQUEST_FIRST_ITEM_AMOUNT}";
        Button_ATK_SPEED_Amount.color = Utils.Item_Count("Orc_Potion",
            (RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)) ? Color.green : Color.red;

        Button_GOLD_Amount.text = $"{RESEARCH_REQUEST_FIRST_ITEM_AMOUNT}";
        Button_GOLD_Amount.color = Utils.Item_Count("Skeleton_Potion",
            (RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)) ? Color.green : Color.red;

        Button_ITEM_Amount.text = $"{RESEARCH_REQUEST_FIRST_ITEM_AMOUNT}";
        Button_ITEM_Amount.color = Utils.Item_Count("Plant_Potion",
            (RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)) ? Color.green : Color.red;

        Button_CRI_DMG_Amount.text = $"{RESEARCH_REQUEST_FIRST_ITEM_AMOUNT}";
        Button_CRI_DMG_Amount.color = Utils.Item_Count("Mushroom_Potion",
            (RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)) ? Color.green : Color.red;

        Button_CRI_PER_Amount.text = $"{RESEARCH_REQUEST_FIRST_ITEM_AMOUNT}";
        Button_CRI_PER_Amount.color = Utils.Item_Count("Turtle_Potion",
            (RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)) ? Color.green : Color.red;
        return base.Init();
    }
    public override void DisableOBJ()
    {
        _ = Base_Manager.BACKEND.WriteData();
        base.DisableOBJ();
    }

    public void Research_Level_Up_Button(int Stat_Number)
    {
        if(Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level >= RESEARCH_FIRST_MAX_LEVEL * (Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_ALL_LEVEL + 1))
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("최고 연구 레벨 입니다.");            
            return;
        }

        switch (Stat_Number)
        {
            case 0:
                if (Base_Manager.Data.Item_Holder["Slime_Potion"].Hero_Card_Amount < 
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)                   
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 재료가 부족합니다.");
                    return;
                }
                else // 연구 등급상승
                {
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 20000000f;
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;
                    Base_Manager.Data.Item_Holder["Slime_Potion"].Hero_Card_Amount -= 
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT;
                    Base_Manager.Player.MarkResearchDirty();
                }
                    break;
            case 1:
                if (Base_Manager.Data.Item_Holder["Spider_Potion"].Hero_Card_Amount <
                    RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 재료가 부족합니다.");
                    return;
                }
                else // 연구 등급상승
                {
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 20000000f;
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;
                    Base_Manager.Data.Item_Holder["Spider_Potion"].Hero_Card_Amount -=
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT;
                    Base_Manager.Player.MarkResearchDirty();
                }
                break;
            case 2:
                if (Base_Manager.Data.Item_Holder["Orc_Potion"].Hero_Card_Amount <
                    RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 재료가 부족합니다.");
                    return;
                }
                else // 연구 등급상승
                {
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 0.15f;
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;
                    Base_Manager.Data.Item_Holder["Orc_Potion"].Hero_Card_Amount -=
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT;
                    Base_Manager.Player.MarkResearchDirty();
                }
                break;
            case 3:
                if (Base_Manager.Data.Item_Holder["Skeleton_Potion"].Hero_Card_Amount <
                    RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 재료가 부족합니다.");
                    return;
                }
                else // 연구 등급상승
                {
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 1.28f;
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;
                    Base_Manager.Data.Item_Holder["Skeleton_Potion"].Hero_Card_Amount -=
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT;
                    Base_Manager.Player.MarkResearchDirty();
                }
                break;
            case 4:
                if (Base_Manager.Data.Item_Holder["Plant_Potion"].Hero_Card_Amount <
                    RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 재료가 부족합니다.");
                    return;
                }
                else // 연구 등급상승
                {
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 0.2f;
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;
                    Base_Manager.Data.Item_Holder["Plant_Potion"].Hero_Card_Amount -=
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT;
                    Base_Manager.Player.MarkResearchDirty();
                }
                break;
            case 5:
                if (Base_Manager.Data.Item_Holder["Mushroom_Potion"].Hero_Card_Amount <
                    RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 재료가 부족합니다.");
                    return;
                }
                else // 연구 등급상승
                {
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 0.25f;
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;
                    Base_Manager.Data.Item_Holder["Mushroom_Potion"].Hero_Card_Amount -=
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT;
                    Base_Manager.Player.MarkResearchDirty();
                }
                break;
            case 6:
                if (Base_Manager.Data.Item_Holder["Turtle_Potion"].Hero_Card_Amount <
                    RESEARCH_REQUEST_FIRST_ITEM_AMOUNT)
                {
                    Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 재료가 부족합니다.");
                    return;
                }
                else // 연구 등급상승
                {
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Value += 0.05f;
                    Base_Manager.Data.User_Main_Data_Research_Array[Stat_Number].Research_Level++;
                    Base_Manager.Data.Item_Holder["Turtle_Potion"].Hero_Card_Amount -=
                        RESEARCH_REQUEST_FIRST_ITEM_AMOUNT;
                    Base_Manager.Player.MarkResearchDirty();
                }
                break;
        }

        

        Init();
        
    }

    public void Research_All_Level_Up_Button()
    {
        foreach(Research_Holder holder in Base_Manager.Data.User_Main_Data_Research_Array)
        {
            if(holder.Research_Level < RESEARCH_FIRST_MAX_LEVEL * (holder.Research_ALL_LEVEL + 1))
            {
                Base_Canvas.instance.Get_Toast_Popup().Initialize("최고레벨 미달성 된 연구가 있습니다.");
                return;
            }
        }

        if (Base_Manager.Data.Item_Holder["Research_Levelup_Book"].Hero_Card_Amount < 1)
        {
            Base_Canvas.instance.Get_Toast_Popup().Initialize("연구 비급서가 부족합니다.");           
            return;
        }
        // 종합 연구레벨 상승
        foreach (Research_Holder holder in Base_Manager.Data.User_Main_Data_Research_Array)
        {
            holder.Research_ALL_LEVEL++;
        }

        Base_Canvas.instance.Get_Toast_Popup().Initialize("종합 연구레벨이 상승하였습니다!");
        Base_Manager.Data.Item_Holder["Research_Levelup_Book"].Hero_Card_Amount--;
        Base_Manager.SOUND.Play(Sound.BGS, "Clear");
        Init();
    }
}
