using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Utils 
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static int[] summon_level = { 10, 45, 110, 250, 300, 500, 750, 1000, 1240 };
    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();
    public static Level_Design Data = Resources.Load<Level_Design>("Scriptable/Level_Design");

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

    ///
    public static double Offline_Timer_Check()
    {
        if(Data_Manager.Main_Players_Data.StartDate == "" || Data_Manager.Main_Players_Data.EndDate == "")
        {
            return 0.0d;
        }

        //TODO: ���ýð��۾� X , �ܺ�����Ͽ� �ð��۾��ʿ�. ���� ���� ���ɼ� ����

        DateTime startDate = DateTime.Parse(Data_Manager.Main_Players_Data.StartDate);
        Debug.Log($"���ӽ��� �ð� : {startDate}");
        DateTime endDate = DateTime.Parse(Data_Manager.Main_Players_Data.EndDate);
        Debug.Log($"���� ���� �ð� : {endDate}");

        TimeSpan timer = startDate - endDate;
        double Time_Count = timer.TotalSeconds;

        return Time_Count;
    }

    public static string NextDayTimer()
    {
        DateTime nowDate = DateTime.Now;
        DateTime NextDate = nowDate.AddDays(1); // AddDays = ������ ��¥�κ��� �ϼ��� 1�� �÷��ִ�
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

}
