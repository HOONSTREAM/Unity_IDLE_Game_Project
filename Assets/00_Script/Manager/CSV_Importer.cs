using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CSV_Importer
{
    public static List<Dictionary<string, object>> Spawn_Design = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
    public static List<Dictionary<string, object>> Summon_Design = new List<Dictionary<string, object>>(CSVReader.Read("Summon"));
    public static List<Dictionary<string, object>> Hero_Skill_Design = new List<Dictionary<string, object>>(CSVReader.Read("Hero_Skill"));
    public static List<Dictionary<string, object>> Hero_DES_Design = new List<Dictionary<string, object>>(CSVReader.Read("Hero_DES"));
    public static List<Dictionary<string, object>> Relic_Skill_Design = new List<Dictionary<string, object>>(CSVReader.Read("Relic_Skill"));
    public static List<Dictionary<string, object>> Relic_DES_Design = new List<Dictionary<string, object>>(CSVReader.Read("Relic_DES"));   
    public static List<Dictionary<string, object>> Quest_Design = new List<Dictionary<string, object>>(CSVReader.Read("Quest"));
    public static List<Dictionary<string, object>> Localization_Design = new List<Dictionary<string, object>>(CSVReader.Read("Localization"));
    public static List<Dictionary<string, object>> Daily_Quest_Design = new List<Dictionary<string, object>>(CSVReader.Read("Daily_Quest"));


    #region 유물 아이템 발동확률, 효과퍼센트
    public static List<Dictionary<string, object>> RELIC_SWORD_Design = new List<Dictionary<string, object>>(CSVReader.Read("SWORD"));
    public static List<Dictionary<string, object>> RELIC_DICE_Design = new List<Dictionary<string, object>>(CSVReader.Read("DICE"));
    #endregion

    public static Dictionary<string, List<Dictionary<string, object>>> Relic_CSV_DATA_AUTO_Map = new Dictionary<string, List<Dictionary<string, object>>>
    {
        { "SWORD", RELIC_SWORD_Design },
        { "DICE", RELIC_DICE_Design }
    };

}



