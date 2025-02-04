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
        { "DEF", 3 },
        { "HP", 4 },      
    };
    
    public static int GetRelicID(string relicName)
    {
        return RelicIDMap.TryGetValue(relicName, out int id) ? id : -1;
    }
}
