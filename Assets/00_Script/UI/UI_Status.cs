using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_Base
{

    public Status_Type status_type;
    [SerializeField]
    private RectTransform Bar; // 인벤토리의 메뉴 (전체아이템, 장비, 소비, 기타)   
    [SerializeField]
    private RectTransform Top_Content;
    [SerializeField]
    private Button[] Status_Bottom_Buttons;
    [SerializeField]
    private GameObject[] Panel_Objs;
    [SerializeField]
    private UI_Status_Parts Status_Parts_Weapon;
    [SerializeField]
    private UI_Status_Parts Status_Parts_ACC;

    [SerializeField]
    private Image Tier_Image;
    [SerializeField]
    private TextMeshProUGUI Player_Level_Text,User_NickName,Tier_Name,Ability, ATK, HP;
    [SerializeField]
    private TextMeshProUGUI GoldDrop, ItemDrop, Atk_Speed, Critical, Cri_Damage;
    [SerializeField]
    private TextMeshProUGUI STR, DEX, VIT;
    

    public override bool Init()
    {
        
        Ability.text = StringMethod.ToCurrencyString(Base_Manager.Player.Player_ALL_Ability_ATK_HP());
        Player_Level_Text.text = "LV." + (Data_Manager.Main_Players_Data.Player_Level + 1).ToString();
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        string temp = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        User_NickName.text = temp;    
        ATK.text = StringMethod.ToCurrencyString(Base_Manager.Player.Calculate_Player_ATK());       
        HP.text = StringMethod.ToCurrencyString(Base_Manager.Player.Calculate_Player_HP());
        GoldDrop.text = $"{100 + Base_Manager.Player.Calculate_Gold_Drop_Percentage() * 100}%";
        ItemDrop.text = $"{100 + (Base_Manager.Player.Calculate_Item_Drop_Percentage())}%";     
        Atk_Speed.text = $"{100 + Base_Manager.Player.Calculate_Atk_Speed_Percentage() * 100}%";

        Calculate_Status_Stat_Text();


        Critical.text = string.Format("{0:0.0}%", Base_Manager.Player.Calculate_Critical_Percentage());
        Cri_Damage.text = string.Format("{0:0.0}%", Base_Manager.Player.Calculate_Cri_Damage_Percentage());
        Tier_Name.text = Utils.Set_Tier_Name();
        Tier_Image.sprite = Utils.Get_Atlas(Data_Manager.Main_Players_Data.Player_Tier.ToString());
        for (int i = 0; i< Status_Bottom_Buttons.Length; i++)
        {
            int index = i;

            Status_Bottom_Buttons[index].onClick.RemoveAllListeners();
            Status_Bottom_Buttons[index].onClick.AddListener(() => Status_Menu_Check((Status_Type)index));
        }

        if(Data_Manager.Main_Players_Data.Player_Max_Stage >= Utils.ENHANCEMENT_DUNGEON_FIRST_HARD)
        {
            Status_Item_Init();
        }
        
        return base.Init();
    }

    /// <summary>
    /// 스테이터스 창에 나타나는 성장장비의 총 스텟 합계를 계산하여 스테이터스 창에 나타냅니다.
    /// </summary>
    private void Calculate_Status_Stat_Text()
    {

        double STR_Temp = 0;
        double DEX_Temp = 0;
        double VIT_Temp = 0;

        foreach (var kvp in Base_Manager.Data.Status_Item_Holder)
        {
            string itemKey = kvp.Key;
            var holderData = kvp.Value;

            // 해당 이름의 ScriptableObject 불러오기
            var scriptable = Base_Manager.Data.Status_Item_Dictionary[itemKey];

            if (holderData.Item_Amount > 0)
            {
                if (scriptable != null)
                {
                    STR_Temp += scriptable.Base_STR;
                    DEX_Temp += scriptable.Base_DEX;
                    VIT_Temp += scriptable.Base_VIT;
                }

                STR_Temp += holderData.Additional_STR;
                DEX_Temp += holderData.Additional_DEX;
                VIT_Temp += holderData.Additional_VIT;
            }

        }

        STR.text = STR_Temp.ToString();
        DEX.text = DEX_Temp.ToString();
        VIT.text = VIT_Temp.ToString();

    }

    public void Status_Item_Init()
    {
        var itemHolder = Base_Manager.Data.Status_Item_Holder;

        // 모든 아이템이 Item_Amount == 0인지 확인
        bool allZero = itemHolder.Values.All(item => item.Item_Amount <= 0);

        if (allZero)
        {
            // 기본 장비 지급
            if (itemHolder.ContainsKey("Weapon_1"))
                itemHolder["Weapon_1"].Item_Amount = 1;

            if (itemHolder.ContainsKey("Ring_1"))
                itemHolder["Ring_1"].Item_Amount = 1;

            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("2000층에 도달하여,성장장비가 지급됩니다.");
        }

        var sort_Dictionary = Base_Manager.Data.Status_Item_Dictionary.OrderByDescending(x => x.Value.rarity);

        foreach (var item in sort_Dictionary)
        {
            if (Base_Manager.Data.Status_Item_Holder.ContainsKey(item.Value.name))
            {
                if(Base_Manager.Data.Status_Item_Holder[item.Key].Item_Amount > 0 && item.Value.Position == "무기")
                {
                    Status_Parts_Weapon.Init(Utils.User_Status_Weapon_Item_Level(), Base_Manager.Data.Status_Item_Holder[item.Key]);
                }
                
                if(Base_Manager.Data.Status_Item_Holder[item.Key].Item_Amount > 0 && item.Value.Position == "악세사리")
                {
                    Status_Parts_ACC.Init(Utils.User_Status_ACC_Item_Level(), Base_Manager.Data.Status_Item_Holder[item.Key]);
                }               
            }
        }
     
    }
    /// <summary>
    /// 유저가 원하는 메뉴를 누르면 바가 움직이는 기능을 구현합니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Bar_Movement_Coroutine(Vector2 endPos) //float endXPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = Bar.anchoredPosition;
        Vector2 end = new Vector2(endPos.x, Top_Content.anchoredPosition.y);


        float startX = Bar.sizeDelta.x;
        //float endX = endXPos;

        while (percent < 1)
        {

            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            // float LerpPosX = Mathf.Lerp(startX, endX, percent);

            Bar.anchoredPosition = LerpPos;

            //Bar.sizeDelta = new Vector2(LerpPosX, Bar.sizeDelta.y);

            yield return null;
        }
    }

    public void Status_Menu_Check(Status_Type state)
    {
        for(int i = 0; i<Panel_Objs.Length; i++)
        {
            Panel_Objs[i].gameObject.SetActive(false);
        }

        Panel_Objs[(int)state].gameObject.SetActive(true);

        status_type = state;
        StartCoroutine(Bar_Movement_Coroutine(Status_Bottom_Buttons[(int)state].GetComponent<RectTransform>().anchoredPosition));       
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1); // 버튼을 다시 원래 크기로 되돌립니다.        
        base.DisableOBJ();

    }
}
