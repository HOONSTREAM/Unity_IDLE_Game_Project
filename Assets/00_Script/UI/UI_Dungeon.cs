using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Dungeon : UI_Base
{

    [SerializeField]
    private TextMeshProUGUI[] KeyTexts;
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Enter_Request_Key;
    [SerializeField]
    private TextMeshProUGUI[] Clear_Assets;
    [SerializeField]
    private TextMeshProUGUI[] Dungeon_Levels;
    
    public override bool Init()
    {
        Main_UI.Instance.FadeInOut(true, true, null);

        for(int i = 0; i< KeyTexts.Length; i++)
        {
            KeyTexts[i].text = "(" + Data_Manager.Main_Players_Data.Daily_Enter_Key[i].ToString() + "/3)";
            Dungeon_Enter_Request_Key[i].color = (Data_Manager.Main_Players_Data.Daily_Enter_Key[i] + Data_Manager.Main_Players_Data.User_Key_Assets[i]) <= 0 ? Color.red : Color.green;
            Dungeon_Levels[i].text = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[i] + 1).ToString();
        }

        int levelCount = (Data_Manager.Main_Players_Data.Dungeon_Clear_Level[1] + 1) * 5;
        var value = Utils.CalculateValue(Utils.Data.stageData.Base_DROP_MONEY, levelCount, Utils.Data.stageData.DROP_MONEY);

        // 레벨디자인 필요
        Clear_Assets[0].text = 1000.ToString();
        Clear_Assets[1].text = StringMethod.ToCurrencyString(value);
        return base.Init();
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }


}
