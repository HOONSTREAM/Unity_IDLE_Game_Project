using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static I_Relic_Effect;

public class Increase_ATK_Effect_Relic : IRelicEffect
{
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.ATK;

    public double ApplyEffect(Item_Scriptable data)
    {
        return Utils.Data.Hoiding_Effect_Data_Relic.Get_ALL_ATK_Holding_Effect_Relic(data) / 100;
    }

}

public class Increase_GoldDrop_Effect_Relic : IRelicEffect
{
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.GOLD_DROP;

    public double ApplyEffect(Item_Scriptable data)
    {
        return Utils.Data.Hoiding_Effect_Data_Relic.Get_ALL_GOLD_DROP_Holding_Effect_Relic(data) / 100;
    }
}

public class Increase_CriticalDamage_Effect_Relic : IRelicEffect
{
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.CRITICAL_DAMAGE;
    public double ApplyEffect(Item_Scriptable data)
    {
        return Utils.Data.Hoiding_Effect_Data_Relic.Get_ALL_CRI_DMG_Effect_Relic(data) / 100;
    }
}

public class Increase_Critical_Percentage_Effect_Relic : IRelicEffect
{
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.CRITICAL_PERCENTAGE;
    public double ApplyEffect(Item_Scriptable data)
    {
        return Utils.Data.Hoiding_Effect_Data_Relic.Get_ALL_CRI_PERCENT_Effect_Relic(data) / 100;
    }
}

public class Increase_ItemDrop_Effect_Relic : IRelicEffect
{
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.ITEM_DROP;
    public double ApplyEffect(Item_Scriptable data)
    {
        return Utils.Data.Hoiding_Effect_Data_Relic.Get_ALL_ITEM_DROP_Holding_Effect_Relic(data) / 100;
    }
}

public class Increase_ATKSpeed_Effect_Relic : IRelicEffect
{
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.ATK_SPEED;
    public double ApplyEffect(Item_Scriptable data)
    {
        return Utils.Data.Hoiding_Effect_Data_Relic.Get_ALL_ATK_SPEED_Holding_Effect_Relic(data) / 100;
    }
}

public class Increase_HP_Effect_Relic : IRelicEffect
{
    public Holding_Effect_Type Get_Effect_Type() => Holding_Effect_Type.HP;
    public double ApplyEffect(Item_Scriptable data)
    {
        return Utils.Data.Hoiding_Effect_Data_Relic.Get_ALL_HP_Holding_Effect_Relic(data) / 100;
    }
}

public static class RelicEffectFactory
{
    private static readonly Dictionary<string, List<IRelicEffect>> Relic_Effects = new Dictionary<string, List<IRelicEffect>>
        {
            { "SWORD", new List<IRelicEffect> { new Increase_ATKSpeed_Effect_Relic(), new Increase_ItemDrop_Effect_Relic() } },
            { "DICE", new List<IRelicEffect> { new Increase_GoldDrop_Effect_Relic(), new Increase_HP_Effect_Relic() } },
           // { "DEF", new List<IRelicEffect> { new Increase_ATK_Effect(), new Increase_ItemDrop_Effect() } },
           // { "HP", new List<IRelicEffect> { new Increase_ATK_Effect(), new Increase_ATKSpeed_Effect() } },
           // { "MANA", new List<IRelicEffect> { new Increase_ATK_Effect(), new Increase_CriticalDamage_Effect() } }
        };


    public static List<IRelicEffect> Get_Holding_Effects_Relic(string RelicName)
    {
        return Relic_Effects.TryGetValue(RelicName, out var effects) ? effects : new List<IRelicEffect>();
    }
}