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
        EXP += Get_EXP();
        ATK += Next_ATK();
        HP += Next_HP();

        if(EXP >= Get_MAX_EXP())
        {
            Level++;
            EXP = 0;
            Main_UI.Instance.Level_Text_Check();
        }
        
        for(int i = 0; i<Spawner.m_players.Count; i++)
        {
            Spawner.m_players[i].Set_ATK_HP();
        }
    }
    public float EXP_Percentage()
    {
        float exp = (float)Get_MAX_EXP();
        double myEXP = EXP;

        return (float)myEXP / exp;
    }
    public double Get_MAX_EXP()
    {
        return Utils.CalculateValue(Utils.Data.levelData.Base_MAX_EXP, Level, Utils.Data.levelData.MAX_EXP);
    }
    public double Get_EXP()
    {
        return Utils.CalculateValue(Utils.Data.levelData.Base_EXP, Level,Utils.Data.levelData.EXP);
    }
    public double Next_ATK()
    {
        return Utils.CalculateValue(Utils.Data.levelData.Base_ATK,Level,Utils.Data.levelData.ATK);
    }
    public double Next_HP()
    {
        return Utils.CalculateValue(Utils.Data.levelData.Base_HP, Level, Utils.Data.levelData.HP);
    }
    public float Next_EXP()
    {
        float exp = (float)Get_MAX_EXP();
        float myexp = (float)Get_EXP();

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
