using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class Player_Manager
{
    
    
    public double ATK = 5;
    public double HP = 50;

    public float Critical_Percentage = 20.0f; // 크리티컬 확률
    public double Critical_Damage = 140.0d; // 크리티컬 추가 데미지

    public double Total_ATK; // 플레이어가 소유중인 영웅의 합계 ATK
    public double Total_HP; // 플레이어가 소유중인 영웅의 합계 HP

   
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
    public double Get_ATK(Rarity rarity, Holder holder)
    {
        var Damage = ATK * ((int)rarity + 1);
        float Level_Damage = ((holder.Hero_Level + 1) * 10) / 1000.0f;
        var Final_Damage = Damage + (Damage * Level_Damage);

        return Final_Damage;
    }
    public double Get_HP(Rarity rarity, Holder holder)
    {
        var Now_HP = HP * ((int)rarity + 1);
        float Level_HP = ((holder.Hero_Level + 1) * 10) / 1000.0f;
        var Final_HP = HP + (Now_HP * Level_HP);

        return Final_HP;
    }
    /// <summary>
    /// 플레이어의 최종 전투력을 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public int Player_ALL_Ability_ATK_HP()
    {
        Total_ATK = 0;
        Total_HP = 0;

        double Total;

        var scriptableCharacters = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var characterScriptable in scriptableCharacters)
        {
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.M_Character_Name))
            {
                Total_ATK += Base_Manager.Player.Get_ATK(characterScriptable.Rarity, Base_Manager.Data.character_Holder[characterScriptable.name]);
                Total_HP += Base_Manager.Player.Get_HP(characterScriptable.Rarity, Base_Manager.Data.character_Holder[characterScriptable.name]);
            }       
        }

        Total = Total_ATK + Total_HP;

        return (int)Total;
    }
    
}
