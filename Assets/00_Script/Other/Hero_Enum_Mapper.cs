using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Hero_Enum_Mapper
{
    private static readonly Dictionary<string, int> HeroIDMap = new Dictionary<string, int>
    {
        { "Dual_Blader", 0 },
        { "Hunter", 1 },
        { "Elemental_Master_White", 2 },
        { "PalaDin", 3 },
        { "Elemental_Master_Black", 4 },
        { "Sword_Master", 5 }

    };

    public static int GetHeroID(string heroName)
    {
        return HeroIDMap.TryGetValue(heroName, out int id) ? id : -1;
    }
}
