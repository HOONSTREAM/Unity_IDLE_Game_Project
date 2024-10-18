using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Manager
{
    public int Level;
    public double EXP;
    public double ATK = 5;
    public double HP = 50;

    public float Critical_Percentage = 20.0f; // 크리티컬 확률
    public double Critical_Damage = 140.0d; // 크리티컬 추가 데미지

   
    public void EXP_UP()
    {
        EXP += float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());

        ATK += Next_ATK();
        HP += Next_HP();

        if(EXP >= float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString()))
        {
            Level++;
            Main_UI.Instance.Level_Text_Check();
        }
        
        for(int i = 0; i<Spawner.m_players.Count; i++)
        {
            Spawner.m_players[i].Set_ATK_HP();
        }
    }

    public float EXP_Percentage()
    {
        float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
        double myEXP = EXP;

        if (Level >= 1)
        {
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
            myEXP -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }

        return (float)myEXP / exp;
    }

    public double Next_ATK()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
    }

    public double Next_HP()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
    }

    public float Next_EXP()
    {
        float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
        float myexp = float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());

        if (Level >= 1)
        {
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
          
        }

        return (myexp / exp) * 100.0f;
    }

    public double Get_ATK(Rarity rarity)
    {
        return ATK * ((int)rarity + 1);
    }

    public double Get_HP(Rarity rarity)
    {
        return HP * ((int)rarity + 1);
    }

    /// <summary>
    /// 플레이어의 최종 전투력을 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public double Player_ALL_Ability_ATK_HP()
    {
        return ATK + HP;
    }
    
}
