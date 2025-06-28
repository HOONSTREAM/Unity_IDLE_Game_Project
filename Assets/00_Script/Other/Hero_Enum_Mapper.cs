using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Hero_Enum_Mapper
{
    private static readonly Dictionary<string, int> HeroIDMap = new Dictionary<string, int>
    {
        { "Dual_Blader", 0 },
        { "Elemental_Master_White", 1 },
        { "Hunter", 2 },
        { "PalaDin", 3 },
        { "Elemental_Master_Black", 4 },
        { "Sword_Master", 5 },
        { "Dragon_Knight", 6 },
        { "Fighter", 7 },
        { "Desperado", 8 },
        { "Winter_Bringer", 9 },
        { "Druid", 10 },
        { "Magnus", 11 },
        { "DarkHero", 12 },
        { "Luminers", 13 },
        { "Knight", 14 },
        { "Sniper", 15 },
        { "Light_Wizard", 16 },
        { "Starlist", 17 },
        { "Warlord", 18 },
        { "Hammer_Knight", 19 },
        { "Chaos_Caster", 20 },
        { "Aqua_Tempest", 21 },
        { "Guardian", 22 },
        { "Scimitar", 23 },

    };

    public static int GetHeroID(string heroName)
    {
        return HeroIDMap.TryGetValue(heroName, out int id) ? id : -1;
    }
}
