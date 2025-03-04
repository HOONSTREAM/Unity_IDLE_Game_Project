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
        ITEM
    }
   
    [SerializeField]
    private Button[] Buttons;
    [SerializeField]
    private GameObject[] Button_Lock, Buff_Lock, Skill_Cool_Time_Frame;
    [SerializeField]
    private TextMeshProUGUI[] Timer_Text;
    [SerializeField]
    private Image[] Buttons_Fill;
    

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
    /// UI_Base�� �ִ� Init�� ������ �Ͽ����Ƿ�, Instantiate �� �Ǹ�, �ش� Init�� �����.
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

        // ���� �ο�

        switch (Value)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }


}
