using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Utils 
{

    public static readonly int TIER_SEASON = 0;

    public static readonly int GOLD_DUNGEON_MULTIPLE_HARD = 40;
    public static readonly int TIER_DUNGEON_FIRST_HARD = 25;
    public static readonly int DIA_DUNGEON_MULTIPLE_HARD = 50;


    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static int[] summon_level = { 10, 45, 110, 250, 300, 500, 750, 1000, 1240 };
    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();
    public static Level_Design Data = Resources.Load<Level_Design>("Scriptable/Level_Design");

    public static readonly string LEADERBOARD_UUID = "0196f55c-84dc-7040-80c1-6635f7a1d349";
    public static readonly double OFFLINE_TIME_CHECK = 300.0d;
    public static bool is_push_alarm_agree = false;

    public static bool is_Skill_Effect_Save_Mode = true;

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
        int totalMinutes = (int)timespan.TotalMinutes;
        string timer = string.Format("{0:000}:{1:00}", totalMinutes, timespan.Seconds);

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
            case Rarity.Chaos:
                return "<color=#FF5921>";

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

    /// <summary>
    /// 오프라인 된 시간을 서버시간으로 부터 받아와서 계산합니다.
    /// </summary>
    /// <returns></returns>
    public static double Offline_Timer_Check()
    {
        if(Data_Manager.Main_Players_Data.StartDate == null || Data_Manager.Main_Players_Data.EndDate == null)
        {
            return 0.0d;
        }

        DateTime startDate = Data_Manager.Main_Players_Data.StartDate;
       
        DateTime endDate = Data_Manager.Main_Players_Data.EndDate;
      
        TimeSpan timer = startDate - endDate;

        double Time_Count = timer.TotalSeconds;

        Time_Count = Mathf.Min((float)Time_Count, 72000f); 
             
        return Time_Count;
    }

    public static double BackGround_Timer_Check()
    {
        if (Data_Manager.Main_Players_Data.StartDate == null || Data_Manager.Main_Players_Data.EndDate == null)
        {
            return 0.0d;
        }

        DateTime startDate = Data_Manager.Main_Players_Data.StartDate;
       
        DateTime endDate = Data_Manager.Main_Players_Data.EndDate;


        TimeSpan timer = startDate - endDate;

        double Time_Count = timer.TotalSeconds;

        Time_Count = Mathf.Min((float)Time_Count,72000f);
       
        return Time_Count;
    }

    /// <summary>
    /// 접속 종료 후의 재접속 시간을 계산하여, 광고락시간을 차감시킵니다.
    /// </summary>
    public static void Calculate_ADS_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // 총 경과된 초 단위
      

        for (int i = 0; i < Data_Manager.Main_Players_Data.ADS_Timer.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.ADS_Timer[i] > 0.0f)
            {
                // 남은 시간에서 경과된 시간 차감
                float temp = Data_Manager.Main_Players_Data.ADS_Timer[i] -= (float)elapsedSeconds;
               
                if (Data_Manager.Main_Players_Data.ADS_Timer[i] < 0)
                {                   
                    Data_Manager.Main_Players_Data.ADS_Timer[i] = 0; // 음수가 되지 않도록 보정
                }
            }

        }
    }

    public static void Calculate_ADS_Buff_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // 총 경과된 초 단위
      
        for (int i = 0; i < Data_Manager.Main_Players_Data.Buff_Timers.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.Buff_Timers[i] > 0.0f)
            {
                // 남은 시간에서 경과된 시간 차감
                float temp = Data_Manager.Main_Players_Data.Buff_Timers[i] -= (float)elapsedSeconds;
               

                if (Data_Manager.Main_Players_Data.Buff_Timers[i] < 0)
                {                   
                    Data_Manager.Main_Players_Data.Buff_Timers[i] = 0; // 음수가 되지 않도록 보정
                }
            }

        }
    }

    public static void Calculate_ADS_X2_SPEED_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // 총 경과된 초 단위
      
        if (Data_Manager.Main_Players_Data.buff_x2_speed > 0.0f)
        {
            // 남은 시간에서 경과된 시간 차감
            float temp = Data_Manager.Main_Players_Data.buff_x2_speed -= (float)elapsedSeconds;


            if (Data_Manager.Main_Players_Data.buff_x2_speed < 0)
            {               
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

    /// <summary>
    /// 14일을 기준으로 시간을 계산합니다.
    /// </summary>
    /// <returns></returns>
    public static string GetNextResetTimer_14Days()
    {
        DateTime now = Utils.Get_Server_Time();
        DateTime launchDate = new DateTime(2025, 5, 15, 0, 0, 0); // 기준일

        if (now < launchDate)
            return "00 : 00 : 00"; // 아직 시작 전

        // 경과 일수 (정확히 소수점 포함해서 계산)
        double totalDaysPassed = (now - launchDate).TotalDays;

        // 다음 리셋 주기 (버림 아님!)
        int nextCycle = (int)Math.Floor(totalDaysPassed / 14) + 1;

        DateTime nextReset = launchDate.AddDays(nextCycle * 14);

        TimeSpan timeLeft = nextReset - now;

        int totalHours = (int)timeLeft.TotalHours;
        int minutes = timeLeft.Minutes;
        int seconds = timeLeft.Seconds;

        return $"{totalHours:D2} : {minutes:D2} : {seconds:D2}";
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

    /// <summary>
    /// 에러 UI를 노출시킵니다.
    /// </summary>
    /// <param name="text"></param>
    public static void Get_LoadingCanvas_ErrorUI(string text)
    {
        Loading_UI_Runner.Instance.ShowErrorUI(text);
    }

    /// <summary>
    /// 뒤끝 콘솔의 버전을 확인하여, 업데이트 UI를 노출시킵니다.
    /// </summary>
    public static void Get_LoadingCanvas_UpdateUI()
    {
        Loading_UI_Runner.Instance.ShowUpdateUI();  
    }

    /// <summary>
    /// 플레이어 티어를 계산하여 텍스트로 반환합니다.
    /// </summary>
    /// <returns></returns>
    public static string Set_Tier_Name()
    {
        string temp = default;

        switch (Data_Manager.Main_Players_Data.Player_Tier)
        {
            case Player_Tier.Tier_Beginner:
                return temp = "초보자";
            case Player_Tier.Tier_Bronze:
                return temp = "브론즈";
            case Player_Tier.Tier_Silver:
                return temp = "실버";
            case Player_Tier.Tier_Gold:
                return temp = "골드";
            case Player_Tier.Tier_Diamond:
                return temp = "다이아몬드";
            case Player_Tier.Tier_Master:
                return temp = "마스터";
            case Player_Tier.Tier_Master_1:
                return temp = "마스터 1성";
            case Player_Tier.Tier_Master_2:
                return temp = "마스터 2성";
            case Player_Tier.Tier_Master_3:
                return temp = "마스터 3성";
            case Player_Tier.Tier_Master_4:
                return temp = "마스터 4성";
            case Player_Tier.Tier_Master_5:
                return temp = "마스터 5성";
            case Player_Tier.Tier_GrandMaster:
                return temp = "그랜드마스터";
            case Player_Tier.Tier_Challenger:
                return temp = "챌린저";
            case Player_Tier.Tier_Challenger_1:
                return temp = "챌린저 1성";
            case Player_Tier.Tier_Challenger_2:
                return temp = "챌린저 2성";
            case Player_Tier.Tier_Challenger_3:
                return temp = "챌린저 3성";
            case Player_Tier.Tier_Challenger_4:
                return temp = "챌린저 4성";
            case Player_Tier.Tier_Challenger_5:
                return temp = "챌린저 5성";
            case Player_Tier.Tier_Challenger_6:
                return temp = "챌린저 6성";
            case Player_Tier.Tier_Challenger_7:
                return temp = "챌린저 7성";
            case Player_Tier.Tier_Challenger_8:
                return temp = "챌린저 8성";
            case Player_Tier.Tier_Challenger_9:
                return temp = "챌린저 9성";
            case Player_Tier.Tier_Challenger_10:
                return temp = "챌린저 마스터";

        }

        return temp;

    }
    /// <summary>
    /// 뒤끝 콘솔 리더보드에 있는 score를 인수로 받아서, 티어를 계산합니다.
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public static string Set_Rank_To_Tier(int score)
    {       
        if (score < 0 || score > (int)Player_Tier.Tier_Challenger_10)
            return "UNRANK";

        Player_Tier tier = (Player_Tier)score;


        switch (tier)
        {
            case Player_Tier.Tier_Beginner: return "초보자";
            case Player_Tier.Tier_Bronze: return "브론즈";
            case Player_Tier.Tier_Silver: return "실버";
            case Player_Tier.Tier_Gold: return "골드";
            case Player_Tier.Tier_Diamond: return "다이아몬드";
            case Player_Tier.Tier_Master: return "마스터";
            case Player_Tier.Tier_Master_1: return "마스터 1성";
            case Player_Tier.Tier_Master_2: return "마스터 2성";
            case Player_Tier.Tier_Master_3: return "마스터 3성";
            case Player_Tier.Tier_Master_4: return "마스터 4성";
            case Player_Tier.Tier_Master_5: return "마스터 5성";
            case Player_Tier.Tier_GrandMaster: return "그랜드마스터";
            case Player_Tier.Tier_Challenger: return "챌린저";
            case Player_Tier.Tier_Challenger_1: return "챌린저 1성";
            case Player_Tier.Tier_Challenger_2: return "챌린저 2성";
            case Player_Tier.Tier_Challenger_3: return "챌린저 3성";
            case Player_Tier.Tier_Challenger_4: return "챌린저 4성";
            case Player_Tier.Tier_Challenger_5: return "챌린저 5성";
            case Player_Tier.Tier_Challenger_6: return "챌린저 6성";
            case Player_Tier.Tier_Challenger_7: return "챌린저 7성";
            case Player_Tier.Tier_Challenger_8: return "챌린저 8성";
            case Player_Tier.Tier_Challenger_9: return "챌린저 9성";
            case Player_Tier.Tier_Challenger_10: return "챌린저 마스터";
            default: return "UNRANK";
        }

    }
    public static string Set_Next_Tier_Name()
    {
        string temp = default;


        switch (Data_Manager.Main_Players_Data.Player_Tier)
        {
            case Player_Tier.Tier_Beginner:
                return temp = "브론즈";
            case Player_Tier.Tier_Bronze:
                return temp = "실버";
            case Player_Tier.Tier_Silver:
                return temp = "골드";
            case Player_Tier.Tier_Gold:
                return temp = "다이아몬드";
            case Player_Tier.Tier_Diamond:
                return temp = "마스터";
            case Player_Tier.Tier_Master:
                return temp = "마스터 1성";
            case Player_Tier.Tier_Master_1:
                return temp = "마스터 2성";
            case Player_Tier.Tier_Master_2:
                return temp = "마스터 3성";
            case Player_Tier.Tier_Master_3:
                return temp = "마스터 4성";
            case Player_Tier.Tier_Master_4:
                return temp = "마스터 5성";
            case Player_Tier.Tier_Master_5:
                return temp = "그랜드마스터";
            case Player_Tier.Tier_GrandMaster:
                return temp = "챌린저";
            case Player_Tier.Tier_Challenger:
                return temp = "챌린저 1성";
            case Player_Tier.Tier_Challenger_1:
                return temp = "챌린저 2성";
            case Player_Tier.Tier_Challenger_2:
                return temp = "챌린저 3성";
            case Player_Tier.Tier_Challenger_3:
                return temp = "챌린저 4성";
            case Player_Tier.Tier_Challenger_4:
                return temp = "챌린저 5성";
            case Player_Tier.Tier_Challenger_5:
                return temp = "챌린저 6성";
            case Player_Tier.Tier_Challenger_6:
                return temp = "챌린저 7성";
            case Player_Tier.Tier_Challenger_7:
                return temp = "챌린저 8성";
            case Player_Tier.Tier_Challenger_8:
                return temp = "챌린저 9성";
            case Player_Tier.Tier_Challenger_9:
                return temp = "챌린저 마스터";
            case Player_Tier.Tier_Challenger_10:
                return temp = "챌린저 마스터";

        }

        return temp;

    }

    /// <summary>
    /// 스테이지 별로, 몬스터를 다르게 소환 시킵니다.
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static string GetStage_MonsterPrefab(int stage)
    {
        if (stage < 200)
            return "Monster_Slime";
        else 
            return "Monster_Skeleton";
       
        //else if (stage < 400)
        //    return "Monster_Troll";
        //else
        //    return "Monster_Dragon"; // 400층 이상
    }
    /// <summary>
    /// 스테이지 별로, 보스를 다르게 소환 시킵니다.
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static string GetStage_BossPrefab(int stage)
    {
        if (stage < 200)
            return "Boss_KingSlime";
        else
            return "Boss_LizardKing";
        //else if (stage < 400)
        //    return "Monster_Troll";
        //else
        //    return "Monster_Dragon"; // 400층 이상
    }


}
