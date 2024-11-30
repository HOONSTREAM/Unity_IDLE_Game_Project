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
        ATK,
        GOLD,
        CRITICAL
    }

    [SerializeField]
    private TextMeshProUGUI Buff_Level_Text, Buff_Level_up_Count;
    [SerializeField]
    private Button[] Buttons;
    [SerializeField]
    private GameObject[] Button_Lock, Buff_Lock, Skill_Cool_Time_Frame;
    [SerializeField]
    private TextMeshProUGUI[] Timer_Text;
    [SerializeField]
    private Image[] Buttons_Fill;
    [SerializeField]
    private Image Level_Fill;

    private void Update()
    {
        for (int i = 0; i < Base_Manager.Data.Buff_Timers.Length; i++)
        {
            if (Base_Manager.Data.Buff_Timers[i] >= 0.0f)
            {               
                Buttons_Fill[i].fillAmount = 1 - (Base_Manager.Data.Buff_Timers[i] / 1800.0f);
                TimeSpan timespan = TimeSpan.FromSeconds(Base_Manager.Data.Buff_Timers[i]);
                string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
                Timer_Text[i].text = timer;
            }

        }
    }

    /// <summary>
    /// UI_Base에 있는 Init을 재정의 하였으므로, Instantiate 가 되면, 해당 Init이 실행됨.
    /// </summary>
    /// <returns></returns>
    public override bool Init()
    {
       
        for (int i = 0; i < Base_Manager.Data.Buff_Timers.Length; i++)
        {

            int button_index = i;
            Buttons[button_index].onClick.AddListener(() => Get_ADS_Buff((ADS_Buff_State)button_index));

            if (Base_Manager.Data.Buff_Timers[i] > 0.0f)
            {
                Set_Buff(i, true);
            }

        }

        return base.Init();

    }

    public void Get_ADS_Buff(ADS_Buff_State state)
    {
        bool Get_Buff = true;
        int state_Value = (int)state;

        Base_Manager.Data.Buff_Level_Count++;

        Base_Manager.Data.Buff_Timers[state_Value] = 1800.0f;
        Set_Buff(state_Value, Get_Buff);

        Main_UI.Instance.ADS_Buff_Check();
    }

    private void Set_Buff(int Value, bool Get_bool)
    {
        Button_Lock[Value].gameObject.SetActive(Get_bool);
        Buff_Lock[Value].gameObject.SetActive(!Get_bool);
        Skill_Cool_Time_Frame[Value].gameObject.SetActive(Get_bool);
    }


}
