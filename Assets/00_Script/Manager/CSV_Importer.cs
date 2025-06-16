using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CSV_Importer
{
    public static List<Dictionary<string, object>> Spawn_Design = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
    public static List<Dictionary<string, object>> Summon_Design = new List<Dictionary<string, object>>(CSVReader.Read("Summon"));
    public static List<Dictionary<string, object>> Summon_Design_Relic = new List<Dictionary<string, object>>(CSVReader.Read("Summon_Relic"));
    public static List<Dictionary<string, object>> Hero_Skill_Design = new List<Dictionary<string, object>>(CSVReader.Read("Hero_Skill"));
    public static List<Dictionary<string, object>> Hero_DES_Design = new List<Dictionary<string, object>>(CSVReader.Read("Hero_DES"));
    public static List<Dictionary<string, object>> Relic_Skill_Design = new List<Dictionary<string, object>>(CSVReader.Read("Relic_Skill"));
    public static List<Dictionary<string, object>> Relic_DES_Design = new List<Dictionary<string, object>>(CSVReader.Read("Relic_DES"));   
    public static List<Dictionary<string, object>> Quest_Design = new List<Dictionary<string, object>>(CSVReader.Read("Quest"));
    public static List<Dictionary<string, object>> Localization_Design = new List<Dictionary<string, object>>(CSVReader.Read("Localization"));
    public static List<Dictionary<string, object>> Daily_Quest_Design = new List<Dictionary<string, object>>(CSVReader.Read("Daily_Quest"));
    public static List<Dictionary<string, object>> DPS_Design = new List<Dictionary<string, object>>(CSVReader.Read("DPS"));
    public static List<Dictionary<string, object>> DPS_REWARD_Design = new List<Dictionary<string, object>>(CSVReader.Read("DPS_REWARD"));

    #region 유물 아이템 발동확률, 효과퍼센트
    public static List<Dictionary<string, object>> RELIC_SWORD_Design = new List<Dictionary<string, object>>(CSVReader.Read("SWORD"));
    public static List<Dictionary<string, object>> RELIC_MANA_Design = new List<Dictionary<string, object>>(CSVReader.Read("MANA"));
    public static List<Dictionary<string, object>> RELIC_HP_Design = new List<Dictionary<string, object>>(CSVReader.Read("HP"));
    public static List<Dictionary<string, object>> RELIC_HPUP_Design = new List<Dictionary<string, object>>(CSVReader.Read("HP_UP"));
    public static List<Dictionary<string, object>> RELIC_ITEM_DROP_Design = new List<Dictionary<string, object>>(CSVReader.Read("ITEM_DROP"));
    public static List<Dictionary<string, object>> RELIC_ATK_SPEED_Design = new List<Dictionary<string, object>>(CSVReader.Read("ATK_SPEED"));
    public static List<Dictionary<string, object>> RELIC_CRI_DMG_Design = new List<Dictionary<string, object>>(CSVReader.Read("CRI_DMG"));
    public static List<Dictionary<string, object>> RELIC_CRI_PER_Design = new List<Dictionary<string, object>>(CSVReader.Read("CRI_PER"));
    public static List<Dictionary<string, object>> RELIC_GOLD_REWARD_Design = new List<Dictionary<string, object>>(CSVReader.Read("GOLD_REWARD"));
    public static List<Dictionary<string, object>> RELIC_GOLD_PER_ATK_Design = new List<Dictionary<string, object>>(CSVReader.Read("GOLD_PER_ATK"));
    public static List<Dictionary<string, object>> RELIC_GOLD_DROP_Design = new List<Dictionary<string, object>>(CSVReader.Read("GOLD_DROP"));
    public static List<Dictionary<string, object>> RELIC_STAFF_Design = new List<Dictionary<string, object>>(CSVReader.Read("STAFF"));
    public static List<Dictionary<string, object>> RELIC_ATK_Design = new List<Dictionary<string, object>>(CSVReader.Read("ATK"));
    public static List<Dictionary<string, object>> RELIC_DICE_Design = new List<Dictionary<string, object>>(CSVReader.Read("DICE"));
    #endregion

    public static Dictionary<string, List<Dictionary<string, object>>> Relic_CSV_DATA_AUTO_Map = new Dictionary<string, List<Dictionary<string, object>>>
    {
        { "SWORD", RELIC_SWORD_Design },
        { "DICE", RELIC_DICE_Design },
        { "MANA", RELIC_MANA_Design },
        { "HP", RELIC_HP_Design },
        { "ATK",RELIC_ATK_Design },
        { "HP_UP", RELIC_HPUP_Design },
        { "ITEM_DROP", RELIC_ITEM_DROP_Design },
        { "ATK_SPEED", RELIC_ATK_SPEED_Design },
        { "STAFF", RELIC_STAFF_Design },
        { "CRI_DMG", RELIC_CRI_DMG_Design },
        { "CRI_PER", RELIC_CRI_PER_Design },
        { "GOLD_REWARD", RELIC_GOLD_REWARD_Design },
        { "GOLD_PER_ATK", RELIC_GOLD_PER_ATK_Design },
        { "GOLD_DROP", RELIC_GOLD_DROP_Design },
        { "DPS", DPS_Design },
        { "DPS_REWARD", DPS_REWARD_Design },

    };

}



