using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Relic_Enum_Mapper
{
    private static readonly Dictionary<string, int> RelicIDMap = new Dictionary<string, int>
    {
        { "SWORD", 0 },
        { "DICE", 1 },
        { "MANA", 2 },
        { "HP", 3 },
        { "ATK", 4 },
        { "HP_UP", 5 },
        { "ITEM_DROP", 6 },
        { "ATK_SPEED", 7 },
        { "STAFF", 8 },
        { "CRI_DMG", 9 },
        { "CRI_PER", 10 },
        { "GOLD_REWARD", 11 },
        { "GOLD_PER_ATK", 12 },

    };
    
    public static int GetRelicID(string relicName)
    {
        return RelicIDMap.TryGetValue(relicName, out int id) ? id : -1;
    }
}
