using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LAUNCH_EVENT : UI_Base
{
    private void Start()
    {
        if (Data_Manager.Main_Players_Data.isBuyLAUNCH_EVENT)
        {
            return;
        }
    }

    public void Get_Reward()
    {
        if (Data_Manager.Main_Players_Data.isBuyLAUNCH_EVENT)
        {
            return;
        }

        Data_Manager.Main_Players_Data.isBuyLAUNCH_EVENT = true;

        Base_Canvas.instance.Get_UI("UI_Reward");
        Utils.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit("Dia", 20000); // 보상지급
        Destroy(this.gameObject);
        Base_Canvas.instance.Destroy_Launch_Event_Button();
        Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();
    }
   
}
