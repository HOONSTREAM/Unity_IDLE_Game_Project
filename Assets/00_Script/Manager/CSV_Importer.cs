using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV_Importer
{
    public static List<Dictionary<string, object>> Spawn_Design = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
    public static List<Dictionary<string, object>> Summon_Design = new List<Dictionary<string, object>>(CSVReader.Read("Summon"));
    public static List<Dictionary<string, object>> Hero_Skill_Design = new List<Dictionary<string, object>>(CSVReader.Read("Hero_Skill"));
    public static List<Dictionary<string, object>> Hero_DES_Design = new List<Dictionary<string, object>>(CSVReader.Read("Hero_DES"));
    public static List<Dictionary<string, object>> RELIC_SWORD_Design = new List<Dictionary<string, object>>(CSVReader.Read("SWORD"));
    public static List<Dictionary<string, object>> DICE_Design = new List<Dictionary<string, object>>(CSVReader.Read("DICE"));
}

