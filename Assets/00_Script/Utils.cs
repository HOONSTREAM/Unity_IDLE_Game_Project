using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.U2D;

public class Utils 
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static int[] summon_level = { 10, 45, 110, 250, 300, 500, 750, 1000, 1240 };
    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();
    public static Level_Design Data = Resources.Load<Level_Design>("Scriptable/Level_Design");

    public static readonly string LEADERBOARD_UUID = "0195f6e0-a8cd-7d69-b421-10a1dc221fcf";
    public static void CloseAllPopupUI()
    {
        while(UI_Holder.Count > 0)
        {
            ClosePopupUI(); 
        }
    }
    public static void ClosePopupUI()
    {
        if(UI_Holder.Count == 0) { return; }

        UI_Base popup = UI_Holder.Peek(); // 스택에 마지막으로 들어온 값을 반환
        popup.DisableOBJ();
    }
    public static Sprite Get_Atlas(string temp)
    {
        return atlas.GetSprite(temp);
    }
    public static string GetTimer(float Time)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(Time);
        string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);

        return timer;
    }
    public static string String_Color_Rarity(Rarity rarity)
    {

        switch (rarity)
        {
            case Rarity.Common:
                return "<color=#FFFFFF>";
                
            case Rarity.UnCommon:
                return "<color=#00FF00>";

            case Rarity.Rare:
                return "<color=#00D8FF>";

            case Rarity.Epic:
                return "<color=#B750C3>";


            case Rarity.Legendary:
                return "<color=#FF9000>";

        }



        return "<color=#FFFFFF>";
    }


    /// <summary>
    /// 레벨디자인에 이용할 지수증가공식입니다.
    /// </summary>
    /// <param name="baseValue"></param>
    /// <param name="level"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double CalculateValue(float baseValue, int level, float value)
    {
        return baseValue * Mathf.Pow((level+1), value);
    }
    
    /// <summary>
    /// 레벨업에 필요한 골드를 충족했는지 검사합니다.
    /// </summary>
    /// <returns></returns>
    public static bool Check_Levelup_Gold(double value)
    {
        if (Data_Manager.Main_Players_Data.Player_Money >= value) return true;

        else return false;

    }

    public static int Calculate_Summon_Level(int count)
    {
        if (count >= summon_level[8])
        {
            return 9;
        }

        for (int i = 0; i < summon_level.Length; i++)
        {
            if (count < summon_level[i])
            {
                return i;
            }
        }
        return -1;
        
    }

    public static float[] Gacha_Percentage()
    {
        float[] Summon_Percentage = new float[5];

        for(int i = 0; i<Summon_Percentage.Length; i++)
        {
            Summon_Percentage[i] =
                float.Parse(CSV_Importer.Summon_Design[Calculate_Summon_Level(Data_Manager.Main_Players_Data.Hero_Summon_Count)][((Rarity)i).ToString()].ToString());
        }

        return Summon_Percentage;
    }

    public static float[] Gacha_Percentage_Relic()
    {
        float[] Summon_Percentage = new float[5];

        for (int i = 0; i < Summon_Percentage.Length; i++)
        {
            Summon_Percentage[i] =
                float.Parse(CSV_Importer.Summon_Design[Calculate_Summon_Level(Data_Manager.Main_Players_Data.Relic_Summon_Count)][((Rarity)i).ToString()].ToString());
        }

        return Summon_Percentage;
    }

    ///
    public static double Offline_Timer_Check()
    {
        if(Data_Manager.Main_Players_Data.StartDate == null || Data_Manager.Main_Players_Data.EndDate == null)
        {
            return 0.0d;
        }

        //TODO: 로컬시간작업 X , 외부통신하여 시간작업필요. 유저 조작 가능성 높음

        DateTime startDate = Data_Manager.Main_Players_Data.StartDate;
        Debug.Log($"게임시작 시간 : {startDate}");
        DateTime endDate = Data_Manager.Main_Players_Data.EndDate;
        Debug.Log($"게임 종료 시간 : {endDate}");

        TimeSpan timer = startDate - endDate;

        double Time_Count = timer.TotalSeconds;

        return Time_Count;
    }

    public static double BackGround_Timer_Check()
    {
        if (Data_Manager.Main_Players_Data.StartDate == null || Data_Manager.Main_Players_Data.EndDate == null)
        {
            return 0.0d;
        }

        //TODO: 로컬시간작업 X , 외부통신하여 시간작업필요. 유저 조작 가능성 높음

        DateTime startDate = Data_Manager.Main_Players_Data.StartDate;
        Debug.Log($"게임시작 시간 : {startDate}");
        DateTime endDate = Data_Manager.Main_Players_Data.EndDate;
        Debug.Log($"게임 종료 시간 : {endDate}");

        TimeSpan timer = startDate - endDate;

        double Time_Count = timer.TotalSeconds;

        return Time_Count;
    }

    /// <summary>
    /// 접속 종료 후의 재접속 시간을 계산하여, 광고락시간을 차감시킵니다.
    /// </summary>
    public static void Calculate_ADS_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // 총 경과된 초 단위
        Debug.Log($"{elapsedSeconds}초가 지나서 그만큼 뺍니다.");

        for (int i = 0; i < Data_Manager.Main_Players_Data.ADS_Timer.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.ADS_Timer[i] > 0.0f)
            {
                // 남은 시간에서 경과된 시간 차감
                float temp = Data_Manager.Main_Players_Data.ADS_Timer[i] -= (float)elapsedSeconds;
                Debug.Log($"{temp}가 계산되었습니다. {Data_Manager.Main_Players_Data.ADS_Timer[i]}에서,{elapsedSeconds}를 뺐습니다.");

                if (Data_Manager.Main_Players_Data.ADS_Timer[i] < 0)
                {
                    Debug.Log("타이머가 0이합니다. 0으로 수정합니다.");
                    Data_Manager.Main_Players_Data.ADS_Timer[i] = 0; // 음수가 되지 않도록 보정
                }
            }

        }
    }

    public static void Calculate_ADS_Buff_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // 총 경과된 초 단위
        Debug.Log($"{elapsedSeconds}초가 지나서 그만큼 뺍니다.");

        for (int i = 0; i < Data_Manager.Main_Players_Data.Buff_Timers.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.Buff_Timers[i] > 0.0f)
            {
                // 남은 시간에서 경과된 시간 차감
                float temp = Data_Manager.Main_Players_Data.Buff_Timers[i] -= (float)elapsedSeconds;
               

                if (Data_Manager.Main_Players_Data.Buff_Timers[i] < 0)
                {
                    Debug.Log("타이머가 0이합니다. 0으로 수정합니다.");
                    Data_Manager.Main_Players_Data.Buff_Timers[i] = 0; // 음수가 되지 않도록 보정
                }
            }

        }
    }

    public static void Calculate_ADS_X2_SPEED_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // 총 경과된 초 단위
        Debug.Log($"{elapsedSeconds}초가 지나서 그만큼 뺍니다.");

        if (Data_Manager.Main_Players_Data.buff_x2_speed > 0.0f)
        {
            // 남은 시간에서 경과된 시간 차감
            float temp = Data_Manager.Main_Players_Data.buff_x2_speed -= (float)elapsedSeconds;


            if (Data_Manager.Main_Players_Data.buff_x2_speed < 0)
            {
                Debug.Log("타이머가 0이합니다. 0으로 수정합니다.");
                Data_Manager.Main_Players_Data.buff_x2_speed = 0; // 음수가 되지 않도록 보정
            }
        }
    }

    public static string NextDayTimer()
    {       
        DateTime nowDate = Get_Server_Time();

        DateTime NextDate = nowDate.AddDays(1); // AddDays = 지정된 날짜로부터 일수를 1일 올려주는
        NextDate = new DateTime(NextDate.Year, NextDate.Month, NextDate.Day, 0, 0, 0);
        TimeSpan timer = NextDate - nowDate;
        return timer.Hours + " : " + timer.Minutes + " : " + timer.Seconds;
    }

    public static bool Item_Count(string itemName, int value)
    {
        if (Base_Manager.Data.Item_Holder[itemName].Hero_Card_Amount >= value)
        {
            return true;
        }
            
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 뒤끝서버시간을 참조하여, 서버시간을 받아옵니다.
    /// </summary>
    /// <returns></returns>
    public static DateTime Get_Server_Time()
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();

        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        DateTime parsedDate = DateTime.Parse(time);

        return parsedDate;
    }

}
