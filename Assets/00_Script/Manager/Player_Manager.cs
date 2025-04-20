using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Player_Manager
{
    #region ADS
    // ADS 버프 적용 값
    private float ADS_Gold_Buff_Value = 0.0f;
    private float ADS_Item_Buff_Value = 0.0f;
    private float ADS_Atk_Buff_Value = 1.0f; // 곱셈으로 들어가므로.


    public void Init()
    {       
        for (int i = 0; i < Spawner.m_players.Count; i++) // 각 서브 히어로 공격력 및 체력 세팅
        {
            Spawner.m_players[i].Set_ATK_HP_Sub_Hero();
        }
    }

    /// <summary>
    /// ADS 버프를 적용하는 메서드
    /// </summary>
    public void Set_ADS_Buff(int buffType, bool isActive)
    {
        float buffValue = isActive ? 3.0f : 0.0f; // 버프가 활성화되면 300% 증가

        switch (buffType)
        {
            case 0: // 골드 드랍률 상승
                ADS_Gold_Buff_Value = buffValue;
                break;
            case 1: // 아이템 드랍률 상승
                ADS_Item_Buff_Value = buffValue * 100;
                break;
            case 2: // 영웅 공격력 상승
                float _atk_buff_value = isActive ? 3.0f : 1.0f;
                ADS_Atk_Buff_Value = _atk_buff_value;
                break;
        }
    }

    /// <summary>
    /// 재접속을 하였을 때, 광고버프를 적용합니다.
    /// </summary>
   

    #endregion

    #region EXP 처리
    public void EXP_UP()
    {
        Data_Manager.Main_Players_Data.EXP += Utils.Data.levelData.Get_EXP();
       
        if(Data_Manager.Main_Players_Data.EXP >= Utils.Data.levelData.Get_MAXEXP()) // 레벨업 조건 달성 시
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("레벨이 올라 더욱 강해졌습니다!");
            Base_Manager.SOUND.Play(Sound.BGS, "Level_Up");
            Data_Manager.Main_Players_Data.Player_Level++;
            Data_Manager.Main_Players_Data.EXP = 0;

            // 메인캐릭터 ATK,HP 세팅
            Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
            Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

            Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();

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
    #endregion

    #region 전투력 계산
    /// <summary>
    /// 실제 영웅에 ATK를 적용합니다.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_ATK(Rarity rarity, Holder holder)
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();
        var ADS_Buff_Value = ADS_Atk_Buff_Value;

        int cardLevel = holder.Hero_Level + 1;

        double rarityMultiplier = RarityBonusTable.RarityMultiplier.TryGetValue(rarity, out double value) ? value : 1.0;

        // 1. 카드 레벨에 절대적인 영향력을 주기 위한 공격력 기반 (레벨^2)
        double levelFactor = Mathf.Pow(cardLevel, 2); // 레벨이 오를수록 공격력 폭발적으로 증가

        // 2. 레벨 기반으로 공격력 직접 산출
        // 3. 레어리티 별 공격력 추가 상승
        double baseATK = levelFactor * 5.0 * rarityMultiplier; // 기본 배수는 게임 밸런스에 따라 조정

        Debug.Log($"{holder.Hero_Level}짜리 카드 {rarityMultiplier}가 추가로 곱해집니다.");
       
        // 4. 유저 레벨 기반 공격력 추가
        baseATK += Data_Manager.Main_Players_Data.ATK;
        Debug.Log($"{Data_Manager.Main_Players_Data.ATK}의 기본공격력");
        // 5. 광고 버프 적용
        baseATK *= ADS_Buff_Value;

        // 6. 각종 추가 버프 적용
        baseATK *= (1.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK) / 100));
        Debug.Log($"{1.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK) / 100)}의 각인밸류 가산중");
        // 7. 티어 보정
        var tier = Data_Manager.Main_Players_Data.Player_Tier;
        double tierMultiplier = TierBonusTable.GetBonusMultiplier(tier);
        Debug.Log($"{tierMultiplier}의 티어보너스 가산중");
        baseATK *= tierMultiplier;

        // 8. 보유 효과 적용
        baseATK *= (1.0d + holding_effect.GetValueOrDefault(Holding_Effect_Type.ATK, 0.0));
        Debug.Log($"{1.0d + holding_effect.GetValueOrDefault(Holding_Effect_Type.ATK, 0.0)}의 영웅 홀딩이펙트 가산중");
        baseATK *= (1.0d + holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.ATK, 0.0));
        Debug.Log($"{1.0d + holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.ATK, 0.0)}의 유물 홀딩이펙트 가산중");
        return baseATK;
    }

    /// <summary>
    /// 실제 영웅에 HP를 적용합니다.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_HP(Rarity rarity, Holder holder)
    {
        var holdingEffect = Check_Player_Holding_Effects();
        var relicEffect = Check_Relic_Holding_Effects();

        int cardLevel = holder.Hero_Level + 1;

        double rarityMultiplier = RarityBonusTable.RarityMultiplier.TryGetValue(rarity, out double value) ? value : 1.0;

        double levelFactor = Mathf.Pow(cardLevel, 2);
        double baseHP = levelFactor * 5.0 * rarityMultiplier;

        
        baseHP += Data_Manager.Main_Players_Data.HP;
        Debug.Log($"기본 체력: {Data_Manager.Main_Players_Data.HP}");

        baseHP *= 1.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.HP) / 100);
        Debug.Log($"각인 효과: {1.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.HP) / 100)}");

        var tier = Data_Manager.Main_Players_Data.Player_Tier;
        double tierMultiplier = TierBonusTable.GetBonusMultiplier(tier);
        baseHP *= tierMultiplier;
        Debug.Log($"티어 보너스: {tierMultiplier}");

        baseHP *= 1.0d + holdingEffect.GetValueOrDefault(Holding_Effect_Type.HP, 0.0);
        Debug.Log($"영웅 보유 효과: {1.0d + holdingEffect.GetValueOrDefault(Holding_Effect_Type.HP, 0.0)}");

        baseHP *= 1.0d + relicEffect.GetValueOrDefault(Holding_Effect_Type.HP, 0.0);
        Debug.Log($"유물 보유 효과: {1.0d + relicEffect.GetValueOrDefault(Holding_Effect_Type.HP, 0.0)}");

        return baseHP;
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
    #endregion

    #region 부가능력치 계산
    public float Calculate_Item_Drop_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.ITEM_DROP, 0.0)) * 100;
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.ITEM_DROP, 0.0)) * 100;

        var ADS_Buff_Value = ADS_Item_Buff_Value;

        var total_value = (0.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.ITEM) +
            (float)Value + (float)Relic_Value + ADS_Buff_Value);


        return total_value;
    }
    public float Calculate_Atk_Speed_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.ATK_SPEED, 0.0));
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.ATK_SPEED, 0.0));
      
        return (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK_SPEED) / 100) + (float)Value + (float)Relic_Value;
    }
    public float Calculate_Gold_Drop_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = ((holding_effect.GetValueOrDefault(Holding_Effect_Type.GOLD_DROP, 0.0)));
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.GOLD_DROP, 0.0));

        var ADS_Buff_Value = ADS_Gold_Buff_Value;
      
        return 0.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.MONEY) / 100) + (float)Value + (float)Relic_Value + ADS_Buff_Value;
    }
    public float Calculate_Critical_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.CRITICAL_PERCENTAGE, 0.0) * 100);
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.CRITICAL_PERCENTAGE, 0.0) * 100);

        return 20.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_PERCENTAGE) + (float)Value + (float)Relic_Value;
    }
    public float Calculate_Cri_Damage_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.CRITICAL_DAMAGE, 0.0) * 100);
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.CRITICAL_DAMAGE, 0.0) * 100);
       
        return 140.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_DAMAGE) +(float)Value + (float)Relic_Value;
    }
    #endregion

    #region 보유효과 계산

    /// <summary>
    /// 영웅 보유 상태를 점검하여 보유효과를 계산합니다.
    /// </summary>
    /// <returns></returns>
    private Dictionary<Holding_Effect_Type, double> Check_Player_Holding_Effects()
    {
        Dictionary<Holding_Effect_Type, double> holdingEffectValues = new Dictionary<Holding_Effect_Type, double>();

        var datas = Base_Manager.Data.character_Holder;

        foreach (var data in datas)
        {
            if (data.Value.Hero_Card_Amount > 0)
            {
                var effects = HeroEffectFactory.Get_Holding_Effects(data.Key);

                foreach (var effect in effects)
                {
                    Holding_Effect_Type type = effect.Get_Effect_Type();

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

    /// <summary>
    /// 유물 보유상태를 점검하여 보유효과를 계산합니다.
    /// </summary>
    /// <returns></returns>
    private Dictionary<Holding_Effect_Type, double> Check_Relic_Holding_Effects()
    {
        Dictionary<Holding_Effect_Type, double> holdingEffectValues = new Dictionary<Holding_Effect_Type, double>();

        var datas = Base_Manager.Data.Item_Holder;

        foreach (var data in datas)
        {
            if (data.Value.Hero_Card_Amount > 0)
            {
                var effects = RelicEffectFactory.Get_Holding_Effects_Relic(data.Key);

                foreach (var effect in effects)
                {
                    Holding_Effect_Type type = effect.Get_Effect_Type();

                    double effectValue = effect.ApplyEffect(Base_Manager.Data.Data_Item_Dictionary[data.Key]);

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

    public static class TierBonusTable
    {
        public static readonly Dictionary<Player_Tier, double> TierBonus = new Dictionary<Player_Tier, double>
    {
        { Player_Tier.Tier_Beginner, 1.0 },
        { Player_Tier.Tier_Bronze, 1.5 },     // +150%
        { Player_Tier.Tier_Silver, 2.0 },     // +250%
        { Player_Tier.Tier_Gold, 2.5 },       // +400%
        { Player_Tier.Tier_Diamond, 3.0 },    // +600%
        { Player_Tier.Tier_Master, 3.5 },     // +800%
        { Player_Tier.Tier_GrandMaster, 4.0 },// +950%
        { Player_Tier.Tier_Challenger, 5.0 } // +1200%
    };

        public static double GetBonusMultiplier(Player_Tier tier)
        {
            return TierBonus.ContainsKey(tier) ? TierBonus[tier] : 1.0;
        }
    }

    public static class RarityBonusTable
    {
        public static readonly Dictionary<Rarity, double> RarityMultiplier = new Dictionary<Rarity, double>()
        {
            { Rarity.Common, 1.0 },
            { Rarity.UnCommon, 2.0 },
            { Rarity.Rare, 3.5 },
            { Rarity.Epic, 20.5 },
            { Rarity.Legendary, 60.0 }
        };

    }

    #endregion
}

