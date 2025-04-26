using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Relic_CSV_Mapper
{
    private static readonly Dictionary<string, string> RelicCSVMap = new Dictionary<string, string>
    {
        { "SWORD", "RELIC_SWORD_Design" },
        { "DICE", "RELIC_DICE_Design" },
        { "HP", "RELIC_HP_Design" },
        { "MANA", "RELIC_MANA_Design" },
    };

    public static string GetRelicCSV(string relicName)
    {
        return RelicCSVMap.TryGetValue(relicName, out string csvFile) ? csvFile : null;
    }

}