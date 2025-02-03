using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class Player_Manager
{
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
        var holding_effect = Check_Player_Holding_Effects();

        var Damage = Data_Manager.Main_Players_Data.ATK * ((int)rarity + 1);
        float Level_Damage = ((holder.Hero_Level + 1) * ((int)rarity * 3)) / 10.0f;

        var Final_Damage = (Damage + (Damage * Level_Damage)) * (1.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK) / 100));

        return Final_Damage * (1.0d + holding_effect.GetValueOrDefault(Hero_Holding_Effect_Type.ATK, 0.0));
    }

    /// <summary>
    /// 실제 영웅에 HP를 적용합니다.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_HP(Rarity rarity, Holder holder)
    {
        var holding_effect = Check_Player_Holding_Effects();

        var Now_HP = Data_Manager.Main_Players_Data.HP * ((int)rarity + 1);
        float Level_HP = ((holder.Hero_Level + 1) * ((int)rarity * 3)) / 10.0f;
        var Final_HP = (Data_Manager.Main_Players_Data.HP + (Now_HP * Level_HP)) * (1.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.HP) / 100);

        return Final_HP * (1.0d + holding_effect.GetValueOrDefault(Hero_Holding_Effect_Type.HP, 0.0));
    }

    /// <summary>
    /// 플레이어의 총 합계 공격력을 텍스트로 리턴합니다. 실제 적용되는 능력치가 아닙니다.
    /// </summary>
    /// <returns></returns>
    public double Calculate_Player_ATK()
    {
        double Total_ATK = 0;

        var datas = Base_Manager.Data.character_Holder;

        foreach (var data in datas)
        {
            if (data.Value.Hero_Card_Amount > 0)
            {              
                Total_ATK += Base_Manager.Player.Get_ATK(Base_Manager.Data.Data_Character_Dictionary[data.Key].Data.Rarity, data.Value);
            }
        }

        return Total_ATK;

    }
    /// <summary>
    /// 플레이어의 총 합계 체력을 텍스트로 리턴합니다. 실제 적용되는 능력치가 아닙니다.
    /// </summary>
    /// <returns></returns>
    public double Calculate_Player_HP()
    {
        double Total_HP = 0;

        var datas = Base_Manager.Data.character_Holder;

        foreach (var data in datas)
        {
            if (data.Value.Hero_Card_Amount > 0)
            {             
                Total_HP += Base_Manager.Player.Get_HP(Base_Manager.Data.Data_Character_Dictionary[data.Key].Data.Rarity, data.Value);
            }
        }

        return Total_HP;

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
        var holding_effect = Check_Player_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Hero_Holding_Effect_Type.ITEM_DROP, 0.0));
        

        return 0.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.ITEM) + (float)Value;
    }
    public float Calculate_Atk_Speed_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Hero_Holding_Effect_Type.ATK_SPEED, 0.0));
        Debug.Log($"공속 상승 밸류 : {Value}");

        return (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK_SPEED) / 100) + (float)Value;
    }
    public float Calculate_Gold_Drop_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();

        var Value = ((holding_effect.GetValueOrDefault(Hero_Holding_Effect_Type.GOLD_DROP, 0.0)));
        Debug.Log($"골드드랍률 상승 밸류 : {Value}");

        return 0.0f + ( Base_Manager.Data.Get_smelt_value(Smelt_Status.MONEY) / 100 ) + (float)Value;
    }
    public float Calculate_Critical_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Hero_Holding_Effect_Type.CRITICAL_PERCENTAGE, 0.0) * 100);
        Debug.Log($"크리티컬 확률 상승 밸류 : {Value}");

        return 20.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_PERCENTAGE) + (float)Value;
    }
    public float Calculate_Cri_Damage_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Hero_Holding_Effect_Type.CRITICAL_DAMAGE, 0.0) * 100);
        Debug.Log($"크리티컬 데미지 상승 밸류 : {Value}");

        return 140.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_DAMAGE) +(float)Value;
    }


    private Dictionary<Hero_Holding_Effect_Type, double> Check_Player_Holding_Effects()
    {
        Dictionary<Hero_Holding_Effect_Type, double> holdingEffectValues = new Dictionary<Hero_Holding_Effect_Type, double>();

        var datas = Base_Manager.Data.character_Holder;

        foreach (var data in datas)
        {
            if (data.Value.Hero_Card_Amount > 0)
            {
                var effects = HeroEffectFactory.Get_Holding_Effects(data.Key);

                foreach (var effect in effects)
                {
                    Hero_Holding_Effect_Type type = effect.Get_Effect_Type();

                    double effectValue = effect.ApplyEffect(Base_Manager.Data.Data_Character_Dictionary[data.Key].Data);

                    if (!holdingEffectValues.ContainsKey(type))
                    {
                        holdingEffectValues[type] = 0.0;
                    }

                    holdingEffectValues[type] += effectValue;

                    
                }
            }
        }

       

        return holdingEffectValues;
    }


}

