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
            Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
            Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

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

    /// <summary>
    /// 실제 영웅에 ATK를 적용합니다.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_ATK(Rarity rarity, Holder holder)
    {
        var Damage = Data_Manager.Main_Players_Data.ATK * ((int)rarity + 1);
        float Level_Damage = ((holder.Hero_Level + 1) * ((int)rarity * 3)) / 10.0f;

        var Final_Damage = (Damage + (Damage * Level_Damage)) * (1.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK) / 100));

        return Final_Damage * Check_Player_Holding_Effect();
    }

    private double Check_Player_Holding_Effect()
    {
        double Holding_Effect_Value = default;

        var Datas = Base_Manager.Data.Data_Character_Dictionary;

        foreach (var data in Datas)
        {
            if (Base_Manager.Data.Data_Character_Dictionary.ContainsKey(data.Value.Data.name))
            {               
                Debug.Log($"{data.Value.Data.name}의 보유효과를 적용합니다.");
                Holding_Effect_Value += (Utils.Data.Holding_Effect_Data.Get_ALL_ATK_Holding_Effect(data.Value.Data) / 100);
            }
        }

        Debug.Log($"{(1.0f) + Holding_Effect_Value} 의 보유효과 퍼센트 합계");

        return (1.0f) + Holding_Effect_Value;

    }

    /// <summary>
    /// 실제 영웅에 HP를 적용합니다.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_HP(Rarity rarity, Holder holder)
    {
        var Now_HP = Data_Manager.Main_Players_Data.HP * ((int)rarity + 1);
        float Level_HP = ((holder.Hero_Level + 1) * ((int)rarity * 3)) / 10.0f;
        var Final_HP = Data_Manager.Main_Players_Data.HP + (Now_HP * Level_HP);

        return Final_HP * (1.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.HP));
    }

    /// <summary>
    /// 플레이어의 총 합계 공격력을 텍스트로 리턴합니다. 실제 적용되는 능력치가 아닙니다.
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
    /// 플레이어의 총 합계 체력을 텍스트로 리턴합니다. 실제 적용되는 능력치가 아닙니다.
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
        double value = Calculate_Player_ATK() + Calculate_Player_HP();

        return (int)value;
    }

    public float Calculate_Item_Drop_Percentage()
    {
        return 0.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.ITEM);
    }
    public float Calculate_Atk_Speed_Percentage()
    {
        return (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK_SPEED) / 100);
    }
    public float Calculate_Gold_Drop_Percentage()
    {
        return 0.0f + ( Base_Manager.Data.Get_smelt_value(Smelt_Status.MONEY) / 100 );
    }
    public float Calculate_Critical_Percentage()
    {
        return 20.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_PERCENTAGE);
    }
    public float Calculate_Cri_Damage_Percentage()
    {
        return 140.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_DAMAGE);
    }
}

