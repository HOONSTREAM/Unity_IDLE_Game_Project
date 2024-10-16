using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager
{
    public int Level;
    public double EXP;
    public double ATK = 10;
    public double HP = 50;

   
    public void EXP_UP()
    {
        EXP += float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());

        if(EXP >= float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString()))
        {
            Level++;
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

    
}
