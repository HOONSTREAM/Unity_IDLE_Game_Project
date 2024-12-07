using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Manager
{
    
    
    public double ATK = 5;
    public double HP = 50;

    public float Critical_Percentage = 20.0f; // 크리티컬 확률
    public double Critical_Damage = 140.0d; // 크리티컬 추가 데미지

   
    public void EXP_UP()
    {
        Data_Manager.Main_Players_Data.EXP += Utils.Data.levelData.Get_EXP();
        ATK += Utils.Data.levelData.Get_ATK();
        HP += Utils.Data.levelData.Get_HP();

        if(Data_Manager.Main_Players_Data.EXP >= Utils.Data.levelData.Get_MAXEXP())
        {
            Data_Manager.Main_Players_Data.Player_Level++;
            Data_Manager.Main_Players_Data.EXP = 0;
            Main_UI.Instance.Level_Text_Check();
        }
        
        for(int i = 0; i<Spawner.m_players.Count; i++)
        {
            Spawner.m_players[i].Set_ATK_HP();
        }
    }
    public float EXP_Percentage()
    {
        float exp = (float)Utils.Data.levelData.Get_MAXEXP();
        double myEXP = Data_Manager.Main_Players_Data.EXP;

        return (float)myEXP / exp;
    }

    public float Next_EXP()
    {
        float exp = (float)Utils.Data.levelData.Get_MAXEXP();
        float myexp = (float)Utils.Data.levelData.Get_EXP();

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
