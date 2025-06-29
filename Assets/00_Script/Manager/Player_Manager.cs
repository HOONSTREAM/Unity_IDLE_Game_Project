using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Player_Manager
{
    #region ADS
    // ADS ���� ���� ��
    private float ADS_Gold_Buff_Value = 0.0f;
    private float ADS_Item_Buff_Value = 0.0f;
    private float ADS_Atk_Buff_Value = 1.0f; // �������� ���Ƿ�.

    public bool isAutoLeveling = false; // �ڵ� ������ �� ����

    #region ����ȿ�� ĳ��

    private Dictionary<Holding_Effect_Type, double> cachedPlayerHoldingEffects;
    private Dictionary<Holding_Effect_Type, double> cachedRelicHoldingEffects;
    private bool isEffectDirty_PlayerEffect = true;
    private bool isEffectDirty_RelicEffect = true;

    /// <summary>
    /// �÷��̾� (���� ����ȿ��)�� ����Ǿ����� �˸��� ��Ƽ�÷��� ������ ȣ���ϴ� �޼���
    /// </summary>
    public void MarkPlayerEffectDirty() => isEffectDirty_PlayerEffect = true;
    /// <summary>
    /// �÷��̾� (���� ����ȿ��)�� ����Ǿ����� �˸��� ��Ƽ�÷��� ������ ȣ���ϴ� �޼���
    /// </summary>
    public void MarkRelicEffectDirty() => isEffectDirty_RelicEffect = true;

    /// <summary>
    /// ������ �����ʾ����� ĳ�õ� ����ȿ���� ��ȯ�ϰ�, �׷��������� ���� ����մϴ�.
    /// </summary>
    /// <returns></returns>
    public Dictionary<Holding_Effect_Type, double> Check_Player_Holding_Effects()
    {
        if (!isEffectDirty_PlayerEffect && cachedPlayerHoldingEffects != null)
            return cachedPlayerHoldingEffects;

        cachedPlayerHoldingEffects = CalCulate_Player_Holding_Effects();
        isEffectDirty_PlayerEffect = false;
        return cachedPlayerHoldingEffects;
    }

    public Dictionary<Holding_Effect_Type, double> Check_Relic_Holding_Effects()
    {
        if (!isEffectDirty_RelicEffect && cachedRelicHoldingEffects != null)
            return cachedRelicHoldingEffects;

        cachedRelicHoldingEffects = CalCulate_Relic_Holding_Effects();
        isEffectDirty_RelicEffect = false;
        return cachedRelicHoldingEffects;
    }
    /// <summary>
    /// ���� ���� ���¸� �����Ͽ� ����ȿ���� ����մϴ�.
    /// </summary>
    /// <returns></returns>
    private Dictionary<Holding_Effect_Type, double> CalCulate_Player_Holding_Effects()
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
    /// ���� �������¸� �����Ͽ� ����ȿ���� ����մϴ�.
    /// </summary>
    /// <returns></returns>
    private Dictionary<Holding_Effect_Type, double> CalCulate_Relic_Holding_Effects()
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
    #endregion

    public void Init()
    {       
        for (int i = 0; i < Spawner.m_players.Count; i++) // �� ���� ����� ���ݷ� �� ü�� ����
        {
            Spawner.m_players[i].Set_ATK_HP_Sub_Hero();
        }
    }

    /// <summary>
    /// ADS ������ �����ϴ� �޼���
    /// </summary>
    public void Set_ADS_Buff(int buffType, bool isActive)
    {
        float buffValue = isActive ? 3.0f : 0.0f; // ������ Ȱ��ȭ�Ǹ� 300% ����

        switch (buffType)
        {
            case 0: // ��� ����� ���
                ADS_Gold_Buff_Value = buffValue;
                break;
            case 1: // ������ ����� ���
                ADS_Item_Buff_Value = buffValue * 100;
                break;
            case 2: // ���� ���ݷ� ���
                float _atk_buff_value = isActive ? 3.0f : 1.0f;
                ADS_Atk_Buff_Value = _atk_buff_value;
                break;
        }
    }

    #endregion

    #region ������ ���
    /// <summary>
    /// ���� ������ ATK�� �����մϴ�.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_ATK(Rarity rarity, Holder holder, string Hero_name, bool NextUnit = false)
    {
        
        var holdingEffect = Check_Player_Holding_Effects();
        var holdingEffectRelic = Check_Relic_Holding_Effects();
        var adsBuffValue = ADS_Atk_Buff_Value;
        var playerData = Data_Manager.Main_Players_Data;

        int cardLevel = holder.Hero_Level + 1;

        if (NextUnit) // �������� ���ݷ��� �̸� �˻���.
        {
            cardLevel = holder.Hero_Level + 2;
        }

        double rarityMultiplier = RarityBonusTable.RarityMultiplier.TryGetValue(rarity, out double value) ? value : 1.0;
        
        double baseATK = Base_Manager.Data.Data_Character_Dictionary[Hero_name].Data.Base_ATK;

        // ī�� ���� ���� (���� �� ���)
        double levelFactor = cardLevel * cardLevel;

        baseATK += playerData.ATK;

        // �⺻ ���ݷ� ��� (���� + ���Ƽ)
        baseATK *= levelFactor * ((int)Base_Manager.Data.Data_Character_Dictionary[Hero_name].Data.Rarity + 1) * (rarityMultiplier);
        
        foreach (var kvp in Base_Manager.Data.Status_Item_Holder)
        {
            string itemKey = kvp.Key;
            var holderData = kvp.Value;

            // �ش� �̸��� ScriptableObject �ҷ�����
            var scriptable = Base_Manager.Data.Status_Item_Dictionary[itemKey];
           
            if (holderData.Item_Amount > 0)
            {
                if (scriptable != null)
                {
                   
                    baseATK += scriptable.Base_ATK;
                    baseATK += scriptable.Base_STR * 1000000.0;
                  
                }
              
                baseATK += holderData.Additional_ATK;
               
                baseATK += holderData.Additional_STR * 1000000.0;
            }
        }

        
        // ���� ����
        baseATK *= adsBuffValue;

        // ���� ���ʽ�
        baseATK *= 1.0 + (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK) * 0.01);

        // ������ ȿ�� ����
        if (Base_Manager.Item.Set_Item_Check("ATK"))
        {
            var atkLevel = Base_Manager.Data.Item_Holder["ATK"].Hero_Level;
            float effectValue = float.Parse(CSV_Importer.RELIC_ATK_Design[atkLevel]["effect_percent"].ToString());
            baseATK *= effectValue;
        }

        if (Base_Manager.Item.Set_Item_Check("STAFF"))
        {
            baseATK *= 1.5d;
        }

        if (Base_Manager.Item.Set_Item_Check("GOLD_PER_ATK"))
        {
            int goldLevel = (Base_Manager.Data.Item_Holder["GOLD_PER_ATK"].Hero_Level + 1);

            // CSV���� ȿ�� �ۼ�Ʈ �� �������� (�̹� ������ ����Ǿ� �����Ƿ� *0.01 ���� ����)
            float effectValue = float.Parse(CSV_Importer.RELIC_GOLD_PER_ATK_Design[goldLevel]["effect_percent"].ToString());

            // ��� 1õ�� ������ effectValue ��ŭ ����
            double atkBonus = (playerData.Player_Money / 10000000.0) * effectValue;

            // �ִ� �������� ���� ���� * 1.0 (������ �ִ� 100% ����)
            double maxBonus = goldLevel * 1.0;

            // ���ʽ� ����
            if (atkBonus > maxBonus)
            {
                atkBonus = maxBonus;
            }
           
            // ���ݷ¿� ���ʽ� ����
            baseATK *= (1.0d + atkBonus);
           
        }

        // Ƽ�� ���ʽ�
        double tierMultiplier = TierBonusTable.GetBonusMultiplier(playerData.Player_Tier);
        baseATK *= tierMultiplier;

        // ���� ȿ�� ����
        baseATK *= 1.0 + holdingEffect.GetValueOrDefault(Holding_Effect_Type.ATK, 0.0);
        baseATK *= 1.0 + holdingEffectRelic.GetValueOrDefault(Holding_Effect_Type.ATK, 0.0);

       

        return baseATK;
    }

    /// <summary>
    /// ���� ������ HP�� �����մϴ�.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_HP(Rarity rarity, Holder holder, string Hero_name, bool NextUnit = false)
    {
        var holdingEffect = Check_Player_Holding_Effects();
        var relicEffect = Check_Relic_Holding_Effects();
        var playerData = Data_Manager.Main_Players_Data;

        int cardLevel = holder.Hero_Level + 1;

        if (NextUnit) // �������� ü���� �̸� �˻���.
        {
            cardLevel = holder.Hero_Level + 2;
        }

        double rarityMultiplier = RarityBonusTable.RarityMultiplier.TryGetValue(rarity, out double rarityValue) ? rarityValue : 1.0;
        double baseHP = Base_Manager.Data.Data_Character_Dictionary[Hero_name].Data.Base_HP;


        // ���� ��� HP ��� (���� ���� ���)
        double levelFactor = cardLevel * cardLevel;

        baseHP += playerData.HP;

        baseHP *= levelFactor * 5.0 * rarityMultiplier;

        // ������� ����
        foreach (var kvp in Base_Manager.Data.Status_Item_Holder)
        {
            string itemKey = kvp.Key;
            var holderData = kvp.Value;

            // �ش� �̸��� ScriptableObject �ҷ�����
            var scriptable = Base_Manager.Data.Status_Item_Dictionary[itemKey];

            if (holderData.Item_Amount > 0)
            {
                if (scriptable != null)
                {
                    
                    baseHP += scriptable.Base_HP;
                    baseHP += scriptable.Base_VIT * 1000000.0;
                    
                }
                baseHP += holderData.Additional_HP;
                
                baseHP += holderData.Additional_VIT * 1000000.0;
                
            }
        }

        // ���� ȿ��
        baseHP *= 1.0 + (Base_Manager.Data.Get_smelt_value(Smelt_Status.HP) * 0.01);

        // ������ ȿ�� - HP_UP
        if (Base_Manager.Item.Set_Item_Check("HP_UP"))
        {
            var hpUpLevel = Base_Manager.Data.Item_Holder["HP_UP"].Hero_Level;
            float effectValue = float.Parse(CSV_Importer.RELIC_HPUP_Design[hpUpLevel]["effect_percent"].ToString());
            baseHP *= effectValue;
        }

        // Ƽ�� ���ʽ�
        double tierMultiplier = TierBonusTable.GetBonusMultiplier(playerData.Player_Tier);
        baseHP *= tierMultiplier;

        // ���� ȿ�� (���� & ����)
        baseHP *= 1.0 + holdingEffect.GetValueOrDefault(Holding_Effect_Type.HP, 0.0);
        baseHP *= 1.0 + relicEffect.GetValueOrDefault(Holding_Effect_Type.HP, 0.0);

        return baseHP;
    }

    /// <summary>
    /// �÷��̾��� �� �հ� ���ݷ��� �ؽ�Ʈ�� �����մϴ�. ���� ����Ǵ� �ɷ�ġ�� �ƴմϴ�.
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
                Total_ATK += Base_Manager.Player.Get_ATK(Base_Manager.Data.Data_Character_Dictionary[data.Key].Data.Rarity, data.Value,data.Key);
            }
        }

        return Total_ATK;

    }
    /// <summary>
    /// �÷��̾��� �� �հ� ü���� �ؽ�Ʈ�� �����մϴ�. ���� ����Ǵ� �ɷ�ġ�� �ƴմϴ�.
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
                Total_HP += Base_Manager.Player.Get_HP(Base_Manager.Data.Data_Character_Dictionary[data.Key].Data.Rarity, data.Value, data.Key);
            }
        }

        return Total_HP;

    }    
    /// <summary>
    /// �÷��̾��� ���� �������� �����մϴ�.
    /// </summary>
    /// <returns></returns>
    public ulong Player_ALL_Ability_ATK_HP()
    {
        var value = Calculate_Player_ATK() + Calculate_Player_HP();

        return (ulong)value;
    }   
    #endregion

    #region �ΰ��ɷ�ġ ���
    public float Calculate_Item_Drop_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.ITEM_DROP, 0.0)) * 100;
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.ITEM_DROP, 0.0)) * 100;

        var ADS_Buff_Value = ADS_Item_Buff_Value;

        var total_value = (0.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.ITEM) +
            (float)Value + (float)Relic_Value + ADS_Buff_Value);

        if (Base_Manager.Item.Set_Item_Check("ITEM_DROP"))
        {
            var value = "ITEM_DROP";
            var effect_value = float.Parse(CSV_Importer.RELIC_HPUP_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());
            total_value *= effect_value;          
        }
      
        return total_value;
    }
    public float Calculate_Atk_Speed_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.ATK_SPEED, 0.0));
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.ATK_SPEED, 0.0));

        var total_Value = (Base_Manager.Data.Get_smelt_value(Smelt_Status.ATK_SPEED) / 100) + (float)Value + (float)Relic_Value;

        if (Base_Manager.Item.Set_Item_Check("ATK_SPEED"))
        {
            var value = "ATK_SPEED";
            var effect_value = float.Parse(CSV_Importer.RELIC_HPUP_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());
            total_Value *= effect_value;
        }

        return total_Value;
    }
    public float Calculate_Gold_Drop_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = ((holding_effect.GetValueOrDefault(Holding_Effect_Type.GOLD_DROP, 0.0)));
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.GOLD_DROP, 0.0));

        var ADS_Buff_Value = ADS_Gold_Buff_Value;

        var Total_Value = 0.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.MONEY) / 100) + (float)Value + (float)Relic_Value + ADS_Buff_Value;

        if (Base_Manager.Item.Set_Item_Check("GOLD_DROP"))
        {
            var value = "GOLD_DROP";
            var effect_value = float.Parse(CSV_Importer.RELIC_GOLD_DROP_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());
            Total_Value += (effect_value / 100);
        }

        return Total_Value;
    }
    public float Calculate_Critical_Percentage()
    {
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.CRITICAL_PERCENTAGE, 0.0) * 100);
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.CRITICAL_PERCENTAGE, 0.0) * 100);

        var Total_Value = 20.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_PERCENTAGE) + (float)Value + (float)Relic_Value;

        if (Base_Manager.Item.Set_Item_Check("CRI_PER"))
        {
            var value = "CRI_PER";
            var effect_value = float.Parse(CSV_Importer.RELIC_HPUP_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());
            Total_Value += effect_value;
        }

        return Total_Value;
    }
    public float Calculate_Cri_Damage_Percentage()
    {
        double Dex_Value = 0;

        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.CRITICAL_DAMAGE, 0.0) * 100);
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.CRITICAL_DAMAGE, 0.0) * 100);

        var total_Value = 140.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_DAMAGE) + (float)Value + (float)Relic_Value;

        foreach (var kvp in Base_Manager.Data.Status_Item_Holder)
        {
            string itemKey = kvp.Key;
            var holderData = kvp.Value;

            
            // �ش� �̸��� ScriptableObject �ҷ�����
            var scriptable = Base_Manager.Data.Status_Item_Dictionary[itemKey];

            if (holderData.Item_Amount > 0)
            {
                if (scriptable != null)
                {

                    Dex_Value += scriptable.Base_DEX;
                    

                }

                Dex_Value += holderData.Additional_DEX;              
            }

        }

        
        float dexBonus = (float)Dex_Value * 10.0f;
        
        total_Value += dexBonus;

        if (Base_Manager.Item.Set_Item_Check("CRI_DMG"))
        {
            var value = "CRI_DMG";
            var effect_value = float.Parse(CSV_Importer.RELIC_HPUP_Design[Base_Manager.Data.Item_Holder[value].Hero_Level]["effect_percent"].ToString());
            total_Value += effect_value;
        }

        return total_Value;
    }
    #endregion

    #region ����ȿ�� ���

    public static class TierBonusTable
    {
        public static readonly Dictionary<Player_Tier, double> TierBonus = new Dictionary<Player_Tier, double>
    {
        { Player_Tier.Tier_Beginner, 1.0 },
        { Player_Tier.Tier_Bronze, 1.5 },    
        { Player_Tier.Tier_Silver, 2.0 },    
        { Player_Tier.Tier_Gold, 2.5 },      
        { Player_Tier.Tier_Diamond, 3.0 },   
        { Player_Tier.Tier_Master, 3.5 },
        { Player_Tier.Tier_Master_1, 3.5 },
        { Player_Tier.Tier_Master_2, 4.0 },
        { Player_Tier.Tier_Master_3, 4.5 },
        { Player_Tier.Tier_Master_4, 5.0 },
        { Player_Tier.Tier_Master_5, 5.5 },
        { Player_Tier.Tier_GrandMaster, 6.0 },
        { Player_Tier.Tier_Challenger, 6.5 },
        { Player_Tier.Tier_Challenger_1, 7.0 },
        { Player_Tier.Tier_Challenger_2, 8.5 },
        { Player_Tier.Tier_Challenger_3, 9.5 },
        { Player_Tier.Tier_Challenger_4, 11.0 },
        { Player_Tier.Tier_Challenger_5, 15.0 },
        { Player_Tier.Tier_Challenger_6, 20.0 },
        { Player_Tier.Tier_Challenger_7, 25.0 },
        { Player_Tier.Tier_Challenger_8, 30.0 },
        { Player_Tier.Tier_Challenger_9, 38.0 },
        { Player_Tier.Tier_Challenger_10, 50.0 },
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
            { Rarity.UnCommon, 1.2 },
            { Rarity.Rare, 1.5 },
            { Rarity.Epic, 2.0 },
            { Rarity.Legendary, 30.0 },
            { Rarity.Chaos, 150.0 }
        };

    }

    #endregion
}

