using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ADS_Buff : UI_Base
{
    public enum ADS_Buff_State
    {
        GOLD,
        ITEM,
        ATK
    }
   
    [SerializeField]
    private Button[] Buttons;
    [SerializeField]
    private GameObject[] Button_Lock, Buff_Lock, Skill_Cool_Time_Frame;
    [SerializeField]
    private TextMeshProUGUI[] Timer_Text;
    [SerializeField]
    private Image[] Buttons_Fill;
    

    /// <summary>
    /// 버프타이머를 업데이트 구문으로 검사하고, 초가 지나면 버프를 해제합니다.
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < Data_Manager.Main_Players_Data.Buff_Timers.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.Buff_Timers[i] >= 0.0f)
            {               
                Buttons_Fill[i].fillAmount = 1 - (Data_Manager.Main_Players_Data.Buff_Timers[i] / 1800.0f);

                Timer_Text[i].text = Utils.GetTimer(Data_Manager.Main_Players_Data.Buff_Timers[i]);
            }

            else
            {
                Set_Buff(i, false);
            }

        }
    }

    /// <summary>
    /// UI_Base에 있는 Init을 재정의 하였으므로, Instantiate 가 되면, 해당 Init이 실행됨.
    /// </summary>
    /// <returns></returns>
    public override bool Init()
    {
       
        for (int i = 0; i < Data_Manager.Main_Players_Data.Buff_Timers.Length; i++)
        {

            int button_index = i;
            Buttons[button_index].onClick.AddListener(() => Get_ADS_Buff((ADS_Buff_State)button_index));

            if (Data_Manager.Main_Players_Data.Buff_Timers[i] > 0.0f)
            {
                Set_Buff(i, true);
            }

        }

        return base.Init();

    }

    public void Get_ADS_Buff(ADS_Buff_State state)
    {
        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            bool Get_Buff = true;
            int state_Value = (int)state;
          
            Data_Manager.Main_Players_Data.Buff_Timers[state_Value] = 1800.0f;
            Set_Buff(state_Value, Get_Buff);

            Main_UI.Instance.ADS_Buff_Check();
        });
        
    }

    private void Set_Buff(int Value, bool Get_bool)
    {
        Button_Lock[Value].gameObject.SetActive(Get_bool);
        Buff_Lock[Value].gameObject.SetActive(!Get_bool);
        Skill_Cool_Time_Frame[Value].gameObject.SetActive(Get_bool);

        // 버프 부여

        if (Get_bool == true)
        {
            switch (Value)
            {
                case 0: // 골드 드랍률 상승
                    Base_Manager.Player.Set_ADS_Buff(0, true);
                    break;
                case 1: // 아이템 드랍률 상승
                    Base_Manager.Player.Set_ADS_Buff(1, true);
                    break;
                case 2: // 영웅 공격력 상승
                    Base_Manager.Player.Set_ADS_Buff(2, true);
                    break;
            }
        }
        else
        {
            switch (Value)
            {
                case 0:
                    Base_Manager.Player.Set_ADS_Buff(0, false);
                    break;
                case 1:
                    Base_Manager.Player.Set_ADS_Buff(1, false);
                    break;
                case 2:
                    Base_Manager.Player.Set_ADS_Buff(2, false);
                    break;
            }
        }

    }


}
