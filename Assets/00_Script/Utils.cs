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

        UI_Base popup = UI_Holder.Peek(); // ���ÿ� ���������� ���� ���� ��ȯ
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
    /// ���������ο� �̿��� �������������Դϴ�.
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
    /// �������� �ʿ��� ��带 �����ߴ��� �˻��մϴ�.
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
    /// �������� �� �ð��� �����ð����� ���� �޾ƿͼ� ����մϴ�.
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
    /// ���� ���� ���� ������ �ð��� ����Ͽ�, ������ð��� ������ŵ�ϴ�.
    /// </summary>
    public static void Calculate_ADS_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // �� ����� �� ����
      

        for (int i = 0; i < Data_Manager.Main_Players_Data.ADS_Timer.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.ADS_Timer[i] > 0.0f)
            {
                // ���� �ð����� ����� �ð� ����
                float temp = Data_Manager.Main_Players_Data.ADS_Timer[i] -= (float)elapsedSeconds;
               
                if (Data_Manager.Main_Players_Data.ADS_Timer[i] < 0)
                {                   
                    Data_Manager.Main_Players_Data.ADS_Timer[i] = 0; // ������ ���� �ʵ��� ����
                }
            }

        }
    }

    public static void Calculate_ADS_Buff_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // �� ����� �� ����
      
        for (int i = 0; i < Data_Manager.Main_Players_Data.Buff_Timers.Length; i++)
        {
            if (Data_Manager.Main_Players_Data.Buff_Timers[i] > 0.0f)
            {
                // ���� �ð����� ����� �ð� ����
                float temp = Data_Manager.Main_Players_Data.Buff_Timers[i] -= (float)elapsedSeconds;
               

                if (Data_Manager.Main_Players_Data.Buff_Timers[i] < 0)
                {                   
                    Data_Manager.Main_Players_Data.Buff_Timers[i] = 0; // ������ ���� �ʵ��� ����
                }
            }

        }
    }

    public static void Calculate_ADS_X2_SPEED_Timer()
    {
        TimeSpan span = TimeSpan.FromSeconds(Utils.Offline_Timer_Check());
        double elapsedSeconds = span.TotalSeconds; // �� ����� �� ����
      
        if (Data_Manager.Main_Players_Data.buff_x2_speed > 0.0f)
        {
            // ���� �ð����� ����� �ð� ����
            float temp = Data_Manager.Main_Players_Data.buff_x2_speed -= (float)elapsedSeconds;


            if (Data_Manager.Main_Players_Data.buff_x2_speed < 0)
            {               
                Data_Manager.Main_Players_Data.buff_x2_speed = 0; // ������ ���� �ʵ��� ����
            }
        }
    }

    public static string NextDayTimer()
    {       
        DateTime nowDate = Get_Server_Time();

        DateTime NextDate = nowDate.AddDays(1); // AddDays = ������ ��¥�κ��� �ϼ��� 1�� �÷��ִ�
        NextDate = new DateTime(NextDate.Year, NextDate.Month, NextDate.Day, 0, 0, 0);
        TimeSpan timer = NextDate - nowDate;
        return timer.Hours + " : " + timer.Minutes + " : " + timer.Seconds;
    }

    /// <summary>
    /// 14���� �������� �ð��� ����մϴ�.
    /// </summary>
    /// <returns></returns>
    public static string GetNextResetTimer_14Days()
    {
        DateTime now = Utils.Get_Server_Time();
        DateTime launchDate = new DateTime(2025, 5, 15, 0, 0, 0); // ������

        if (now < launchDate)
            return "00 : 00 : 00"; // ���� ���� ��

        // ��� �ϼ� (��Ȯ�� �Ҽ��� �����ؼ� ���)
        double totalDaysPassed = (now - launchDate).TotalDays;

        // ���� ���� �ֱ� (���� �ƴ�!)
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
    /// �ڳ������ð��� �����Ͽ�, �����ð��� �޾ƿɴϴ�.
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
    /// ���� UI�� �����ŵ�ϴ�.
    /// </summary>
    /// <param name="text"></param>
    public static void Get_LoadingCanvas_ErrorUI(string text)
    {
        Loading_UI_Runner.Instance.ShowErrorUI(text);
    }

    /// <summary>
    /// �ڳ� �ܼ��� ������ Ȯ���Ͽ�, ������Ʈ UI�� �����ŵ�ϴ�.
    /// </summary>
    public static void Get_LoadingCanvas_UpdateUI()
    {
        Loading_UI_Runner.Instance.ShowUpdateUI();  
    }

    /// <summary>
    /// �÷��̾� Ƽ� ����Ͽ� �ؽ�Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public static string Set_Tier_Name()
    {
        string temp = default;

        switch (Data_Manager.Main_Players_Data.Player_Tier)
        {
            case Player_Tier.Tier_Beginner:
                return temp = "�ʺ���";
            case Player_Tier.Tier_Bronze:
                return temp = "�����";
            case Player_Tier.Tier_Silver:
                return temp = "�ǹ�";
            case Player_Tier.Tier_Gold:
                return temp = "���";
            case Player_Tier.Tier_Diamond:
                return temp = "���̾Ƹ��";
            case Player_Tier.Tier_Master:
                return temp = "������";
            case Player_Tier.Tier_Master_1:
                return temp = "������ 1��";
            case Player_Tier.Tier_Master_2:
                return temp = "������ 2��";
            case Player_Tier.Tier_Master_3:
                return temp = "������ 3��";
            case Player_Tier.Tier_Master_4:
                return temp = "������ 4��";
            case Player_Tier.Tier_Master_5:
                return temp = "������ 5��";
            case Player_Tier.Tier_GrandMaster:
                return temp = "�׷��帶����";
            case Player_Tier.Tier_Challenger:
                return temp = "ç����";
            case Player_Tier.Tier_Challenger_1:
                return temp = "ç���� 1��";
            case Player_Tier.Tier_Challenger_2:
                return temp = "ç���� 2��";
            case Player_Tier.Tier_Challenger_3:
                return temp = "ç���� 3��";
            case Player_Tier.Tier_Challenger_4:
                return temp = "ç���� 4��";
            case Player_Tier.Tier_Challenger_5:
                return temp = "ç���� 5��";
            case Player_Tier.Tier_Challenger_6:
                return temp = "ç���� 6��";
            case Player_Tier.Tier_Challenger_7:
                return temp = "ç���� 7��";
            case Player_Tier.Tier_Challenger_8:
                return temp = "ç���� 8��";
            case Player_Tier.Tier_Challenger_9:
                return temp = "ç���� 9��";
            case Player_Tier.Tier_Challenger_10:
                return temp = "ç���� ������";

        }

        return temp;

    }
    /// <summary>
    /// �ڳ� �ܼ� �������忡 �ִ� score�� �μ��� �޾Ƽ�, Ƽ� ����մϴ�.
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
            case Player_Tier.Tier_Beginner: return "�ʺ���";
            case Player_Tier.Tier_Bronze: return "�����";
            case Player_Tier.Tier_Silver: return "�ǹ�";
            case Player_Tier.Tier_Gold: return "���";
            case Player_Tier.Tier_Diamond: return "���̾Ƹ��";
            case Player_Tier.Tier_Master: return "������";
            case Player_Tier.Tier_Master_1: return "������ 1��";
            case Player_Tier.Tier_Master_2: return "������ 2��";
            case Player_Tier.Tier_Master_3: return "������ 3��";
            case Player_Tier.Tier_Master_4: return "������ 4��";
            case Player_Tier.Tier_Master_5: return "������ 5��";
            case Player_Tier.Tier_GrandMaster: return "�׷��帶����";
            case Player_Tier.Tier_Challenger: return "ç����";
            case Player_Tier.Tier_Challenger_1: return "ç���� 1��";
            case Player_Tier.Tier_Challenger_2: return "ç���� 2��";
            case Player_Tier.Tier_Challenger_3: return "ç���� 3��";
            case Player_Tier.Tier_Challenger_4: return "ç���� 4��";
            case Player_Tier.Tier_Challenger_5: return "ç���� 5��";
            case Player_Tier.Tier_Challenger_6: return "ç���� 6��";
            case Player_Tier.Tier_Challenger_7: return "ç���� 7��";
            case Player_Tier.Tier_Challenger_8: return "ç���� 8��";
            case Player_Tier.Tier_Challenger_9: return "ç���� 9��";
            case Player_Tier.Tier_Challenger_10: return "ç���� ������";
            default: return "UNRANK";
        }

    }
    public static string Set_Next_Tier_Name()
    {
        string temp = default;


        switch (Data_Manager.Main_Players_Data.Player_Tier)
        {
            case Player_Tier.Tier_Beginner:
                return temp = "�����";
            case Player_Tier.Tier_Bronze:
                return temp = "�ǹ�";
            case Player_Tier.Tier_Silver:
                return temp = "���";
            case Player_Tier.Tier_Gold:
                return temp = "���̾Ƹ��";
            case Player_Tier.Tier_Diamond:
                return temp = "������";
            case Player_Tier.Tier_Master:
                return temp = "������ 1��";
            case Player_Tier.Tier_Master_1:
                return temp = "������ 2��";
            case Player_Tier.Tier_Master_2:
                return temp = "������ 3��";
            case Player_Tier.Tier_Master_3:
                return temp = "������ 4��";
            case Player_Tier.Tier_Master_4:
                return temp = "������ 5��";
            case Player_Tier.Tier_Master_5:
                return temp = "�׷��帶����";
            case Player_Tier.Tier_GrandMaster:
                return temp = "ç����";
            case Player_Tier.Tier_Challenger:
                return temp = "ç���� 1��";
            case Player_Tier.Tier_Challenger_1:
                return temp = "ç���� 2��";
            case Player_Tier.Tier_Challenger_2:
                return temp = "ç���� 3��";
            case Player_Tier.Tier_Challenger_3:
                return temp = "ç���� 4��";
            case Player_Tier.Tier_Challenger_4:
                return temp = "ç���� 5��";
            case Player_Tier.Tier_Challenger_5:
                return temp = "ç���� 6��";
            case Player_Tier.Tier_Challenger_6:
                return temp = "ç���� 7��";
            case Player_Tier.Tier_Challenger_7:
                return temp = "ç���� 8��";
            case Player_Tier.Tier_Challenger_8:
                return temp = "ç���� 9��";
            case Player_Tier.Tier_Challenger_9:
                return temp = "ç���� ������";
            case Player_Tier.Tier_Challenger_10:
                return temp = "ç���� ������";

        }

        return temp;

    }

    /// <summary>
    /// �������� ����, ���͸� �ٸ��� ��ȯ ��ŵ�ϴ�.
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
        //    return "Monster_Dragon"; // 400�� �̻�
    }
    /// <summary>
    /// �������� ����, ������ �ٸ��� ��ȯ ��ŵ�ϴ�.
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
        //    return "Monster_Dragon"; // 400�� �̻�
    }


}
