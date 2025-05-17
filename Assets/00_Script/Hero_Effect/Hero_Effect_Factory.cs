using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static I_Hero_Effect;


public class NONE_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "보유효과 없음";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.NONE;

    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_NONE_Effect(data);
    }

}
public class Increase_ATK_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "아군 전체 물리공격력";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.ATK;

    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_ALL_ATK_Holding_Effect(data) / 100;
    }
    
}

public class Increase_GoldDrop_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "아군 골드획득량";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.GOLD_DROP;

    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_ALL_GOLD_DROP_Holding_Effect(data) / 100;
    }
}

public class Increase_CriticalDamage_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "아군 치명타 데미지";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.CRITICAL_DAMAGE;
    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_ALL_CRI_DMG_Effect(data) / 100;
    }
}

public class Increase_Critical_Percentage_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "아군 치명타 확률";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.CRITICAL_PERCENTAGE;
    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_ALL_CRI_PERCENT_Effect(data) / 100;
    }
}

public class Increase_ItemDrop_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "아군 아이템 드랍률";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.ITEM_DROP;
    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_ALL_ITEM_DROP_Holding_Effect(data) / 100;
    }
}

public class Increase_ATKSpeed_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "아군 전체 공격속도";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.ATK_SPEED;
    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_ALL_ATK_SPEED_Holding_Effect(data) / 100;
    }
}

public class Increase_HP_Effect : IHeroEffect
{
    public string Get_Effect_Name() => "아군 전체 체력";
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.HP;
    public double ApplyEffect(Character_Scriptable data)
    {
        return Utils.Data.Holding_Effect_Data.Get_ALL_HP_Holding_Effect(data) / 100;
    }
}

public static class HeroEffectFactory
{
    private static readonly Dictionary<string, List<IHeroEffect>> hero_Effects = new Dictionary<string, List<IHeroEffect>>
        {
            { "Dual_Blader", new List<IHeroEffect> { new Increase_ATK_Effect(), new Increase_Critical_Percentage_Effect(), new NONE_Effect() } },
            { "Hunter", new List<IHeroEffect> { new Increase_ATK_Effect(), new Increase_GoldDrop_Effect(), new NONE_Effect() } },
            { "Elemental_Master_White", new List<IHeroEffect> { new Increase_ATK_Effect(), new Increase_ItemDrop_Effect(), new NONE_Effect() } },
            { "Elemental_Master_Black", new List<IHeroEffect> { new Increase_ATK_Effect(), new Increase_ATKSpeed_Effect(), new NONE_Effect() } },
            { "PalaDin", new List<IHeroEffect> { new Increase_ATK_Effect(), new NONE_Effect(), new NONE_Effect() } },
            { "Sword_Master", new List<IHeroEffect> { new Increase_ATK_Effect(), new NONE_Effect(), new NONE_Effect() } },
            { "Dragon_Knight", new List<IHeroEffect> { new Increase_ATK_Effect(), new Increase_ATKSpeed_Effect(), new NONE_Effect() } },
            { "Fighter", new List<IHeroEffect> { new Increase_HP_Effect(), new NONE_Effect(), new NONE_Effect() } },
            { "Desperado", new List<IHeroEffect> { new Increase_CriticalDamage_Effect(), new Increase_Critical_Percentage_Effect(), new NONE_Effect() } },
            { "Winter_Bringer", new List<IHeroEffect> { new Increase_ATKSpeed_Effect(), new Increase_CriticalDamage_Effect(), new NONE_Effect() } },
            { "Druid", new List<IHeroEffect> { new Increase_GoldDrop_Effect(), new NONE_Effect(), new NONE_Effect() } },
            { "Magnus", new List<IHeroEffect> { new Increase_GoldDrop_Effect(), new Increase_ItemDrop_Effect(), new NONE_Effect() } },
            { "DarkHero", new List<IHeroEffect> { new Increase_CriticalDamage_Effect(), new Increase_Critical_Percentage_Effect(), new Increase_ATK_Effect() } },
            { "Luminers", new List<IHeroEffect> { new Increase_ATKSpeed_Effect(), new Increase_GoldDrop_Effect(), new NONE_Effect() } },
            { "Knight", new List<IHeroEffect> { new Increase_ItemDrop_Effect(), new NONE_Effect(), new NONE_Effect() } },
            { "Sniper", new List<IHeroEffect> { new Increase_ItemDrop_Effect(), new Increase_HP_Effect(), new NONE_Effect() } },
            { "Light_Wizard", new List<IHeroEffect> { new Increase_GoldDrop_Effect(), new Increase_Critical_Percentage_Effect(), new NONE_Effect() } },
            { "Starlist", new List<IHeroEffect> { new Increase_HP_Effect(), new NONE_Effect(), new NONE_Effect() } },
            { "Warlord", new List<IHeroEffect> { new Increase_CriticalDamage_Effect(), new Increase_ItemDrop_Effect(), new NONE_Effect() } },

        };

    public static List<IHeroEffect> Get_Holding_Effects(string heroName)
    {
        return hero_Effects.TryGetValue(heroName, out var effects) ? effects : new List<IHeroEffect>();
    }

}