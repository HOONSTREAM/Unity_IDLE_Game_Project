using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class Player_Manager
{
    
    public double Total_ATK; // 플레이어가 소유중인 영웅의 합계 ATK
    public double Total_HP; // 플레이어가 소유중인 영웅의 합계 HP

   
    public void EXP_UP()
    {
        Data_Manager.Main_Players_Data.EXP += Utils.Data.levelData.Get_EXP();
       
        if(Data_Manager.Main_Players_Data.EXP >= Utils.Data.levelData.Get_MAXEXP()) // 레벨업 조건 달성 시
        {
            Data_Manager.Main_Players_Data.Player_Level++;
            Data_Manager.Main_Players_Data.EXP = 0;

            // 메인캐릭터 ATK,HP 세팅
            Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_ATK();
            Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_HP();

            Main_UI.Instance.Level_Text_Check();

        }
        
        for(int i = 0; i<Spawner.m_players.Count; i++) // 각 서브 히어로 공격력 및 체력 세팅
        {
            Spawner.m_players[i].Set_ATK_HP_Sub_Hero();
        }

        Data_Manager.Main_Players_Data.EXP_Upgrade_Count++;
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
        var Damage = Data_Manager.Main_Players_Data.ATK * ((int)rarity + 2);
        float Level_Damage = ((holder.Hero_Level + 1) * 10) / 100.0f;
        var Final_Damage = Damage + (Damage * Level_Damage);

        return Final_Damage;
    }
    public double Get_HP(Rarity rarity, Holder holder)
    {
        var Now_HP = Data_Manager.Main_Players_Data.HP * ((int)rarity + 2);
        float Level_HP = ((holder.Hero_Level + 1) * 10) / 100.0f;
        var Final_HP = Data_Manager.Main_Players_Data.HP + (Now_HP * Level_HP);

        return Final_HP;
    }

    /// <summary>
    /// 플레이어의 총 합계 공격력을 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public double Calculate_Player_ATK()
    {
        double Total_ATK = 0;


        double Total;

        var scriptableCharacters = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var characterScriptable in scriptableCharacters)
        {
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.M_Character_Name))
            {
                Total_ATK += Base_Manager.Player.Get_ATK(characterScriptable.Rarity, Base_Manager.Data.character_Holder[characterScriptable.name]);
            }
        }

        Total = Total_ATK;

        return Total;

    }
    /// <summary>
    /// 플레이어의 총 합계 체력을 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public double Calculate_Player_HP()
    {
        double Total_HP = 0;


        double Total;

        var scriptableCharacters = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var characterScriptable in scriptableCharacters)
        {
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.M_Character_Name))
            {
                Total_HP += Base_Manager.Player.Get_HP(characterScriptable.Rarity, Base_Manager.Data.character_Holder[characterScriptable.name]);
            }
        }

        Total = Total_HP;

        return Total;

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
    
    public float Calculate_Item_Drop_Percentage()
    {
        return 0.0f;
    }
    public float Calculate_Atk_Speed_Percentage()
    {
        return 1.0f;
    }
    public float Calculate_Gold_Drop_Percentage()
    {
        return 0.0f;
    }
    public float Calculate_Critical_Percentage()
    {
        return 20.0f;
    }
    public float Calculate_Cri_Damage_Percentage()
    {
        return 140.0f;
    }
}
