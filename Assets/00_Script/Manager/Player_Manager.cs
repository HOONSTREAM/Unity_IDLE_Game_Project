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

    #region EXP ó��
    public void EXP_UP()
    {
        Data_Manager.Main_Players_Data.EXP += Utils.Data.levelData.Get_EXP();
       
        if(Data_Manager.Main_Players_Data.EXP >= Utils.Data.levelData.Get_MAXEXP()) // ������ ���� �޼� ��
        {
            Base_Canvas.instance.Get_TOP_Popup().Initialize("������ �ö� ���� ���������ϴ�!");
            Base_Manager.SOUND.Play(Sound.BGS, "Level_Up");
            Data_Manager.Main_Players_Data.Player_Level++;
            Data_Manager.Main_Players_Data.EXP = 0;

            // ����ĳ���� ATK,HP ����
            Data_Manager.Main_Players_Data.ATK = Utils.Data.levelData.Get_Levelup_Next_ATK();
            Data_Manager.Main_Players_Data.HP = Utils.Data.levelData.Get_Levelup_Next_HP();

            Main_UI.Instance.Main_UI_PlayerInfo_Text_Check();

        }
        
        for(int i = 0; i<Spawner.m_players.Count; i++) // �� ���� ����� ���ݷ� �� ü�� ����
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

    #region ������ ���
    /// <summary>
    /// ���� ������ ATK�� �����մϴ�.
    /// </summary>
    /// <param name="rarity"></param>
    /// <param name="holder"></param>
    /// <returns></returns>
    public double Get_ATK(Rarity rarity, Holder holder, string Hero_name)
    {
        var holdingEffect = Check_Player_Holding_Effects();
        var holdingEffectRelic = Check_Relic_Holding_Effects();
        var adsBuffValue = ADS_Atk_Buff_Value;
        var playerData = Data_Manager.Main_Players_Data;

        int cardLevel = holder.Hero_Level + 1;
        double rarityMultiplier = RarityBonusTable.RarityMultiplier.TryGetValue(rarity, out double value) ? value : 1.0;
        double baseATK = Base_Manager.Data.Data_Character_Dictionary[Hero_name].Data.Base_ATK;

        // ī�� ���� ���� (���� �� ���)
        double levelFactor = cardLevel * cardLevel;

        // �⺻ ���ݷ� ��� (���� + ���Ƽ)
        baseATK *= levelFactor * 5.0 * rarityMultiplier;

        // ���� ATK �߰�
        baseATK += playerData.ATK;

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
            baseATK *= 1.5;
        }

        if (Base_Manager.Item.Set_Item_Check("GOLD_PER_ATK"))
        {
            var goldLevel = Base_Manager.Data.Item_Holder["GOLD_PER_ATK"].Hero_Level;
            float effectValue = float.Parse(CSV_Importer.RELIC_GOLD_PER_ATK_Design[goldLevel]["effect_percent"].ToString());
            double atkBonus = (playerData.Player_Money / 1000000.0) * (effectValue * 0.01);
            baseATK *= 1.0 + atkBonus;
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
    public double Get_HP(Rarity rarity, Holder holder)
    {
        var holdingEffect = Check_Player_Holding_Effects();
        var relicEffect = Check_Relic_Holding_Effects();
        var playerData = Data_Manager.Main_Players_Data;

        int cardLevel = holder.Hero_Level + 1;
        double rarityMultiplier = RarityBonusTable.RarityMultiplier.TryGetValue(rarity, out double rarityValue) ? rarityValue : 1.0;

        // ���� ��� HP ��� (���� ���� ���)
        double levelFactor = cardLevel * cardLevel;
        double baseHP = levelFactor * 5.0 * rarityMultiplier;

        // ���� HP �߰�
        baseHP += playerData.HP;

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
                Total_HP += Base_Manager.Player.Get_HP(Base_Manager.Data.Data_Character_Dictionary[data.Key].Data.Rarity, data.Value);
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
      
        return 0.0f + (Base_Manager.Data.Get_smelt_value(Smelt_Status.MONEY) / 100) + (float)Value + (float)Relic_Value + ADS_Buff_Value;
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
        var holding_effect = Check_Player_Holding_Effects();
        var holding_effect_Relic = Check_Relic_Holding_Effects();

        var Value = (holding_effect.GetValueOrDefault(Holding_Effect_Type.CRITICAL_DAMAGE, 0.0) * 100);
        var Relic_Value = (holding_effect_Relic.GetValueOrDefault(Holding_Effect_Type.CRITICAL_DAMAGE, 0.0) * 100);

        var total_Value = 140.0f + Base_Manager.Data.Get_smelt_value(Smelt_Status.CRITICAL_DAMAGE) + (float)Value + (float)Relic_Value;

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

    /// <summary>
    /// ���� ���� ���¸� �����Ͽ� ����ȿ���� ����մϴ�.
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
    /// ���� �������¸� �����Ͽ� ����ȿ���� ����մϴ�.
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
        { Player_Tier.Tier_Bronze, 1.5 },    
        { Player_Tier.Tier_Silver, 2.0 },    
        { Player_Tier.Tier_Gold, 2.5 },      
        { Player_Tier.Tier_Diamond, 3.0 },   
        { Player_Tier.Tier_Master, 3.5 },     
        { Player_Tier.Tier_GrandMaster, 4.0 },
        { Player_Tier.Tier_Challenger, 5.0 },
        { Player_Tier.Tier_Challenger_1, 7.5 },
        { Player_Tier.Tier_Challenger_2, 10.5 },
        { Player_Tier.Tier_Challenger_3, 12.5 },
        { Player_Tier.Tier_Challenger_4, 14.0 },
        { Player_Tier.Tier_Challenger_5, 16.5 },
        { Player_Tier.Tier_Challenger_6, 18.0 },
        { Player_Tier.Tier_Challenger_7, 20.0 },
        { Player_Tier.Tier_Challenger_8, 22.0 },
        { Player_Tier.Tier_Challenger_9, 24.0 },
        { Player_Tier.Tier_Challenger_10, 30.0 },
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
            { Rarity.Legendary, 60.0 },
            { Rarity.Chaos, 120.0 }
        };

    }

    #endregion
}

