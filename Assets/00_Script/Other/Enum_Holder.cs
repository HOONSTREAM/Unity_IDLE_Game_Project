

public enum Rarity
{
    Common,
    UnCommon,
    Rare,
    Epic,
    Legendary
}

public enum Status_Type
{
    Status,
    Mastery,
    Costume
}

public enum Smelt_Status
{
    ATK, // 공격력
    HP, // 체력
    MONEY, // 골드 드랍률
    ITEM, // 아이템 드랍률
    SKILL_COOL, // 스킬 쿨타임 감소
    ATK_SPEED, // 공격력
    CRITICAL_PERCENTAGE, //크리티컬 확률
    CRITICAL_DAMAGE, // 크리티컬 데미지
}

public enum Coin_Type
{
    Dia,
    Gold,
}

public enum Quest_Type
{
    Monster,
    Stage,
    Gold_DG,
    Dia_DG,
    Upgrade,
    Hero,

}

public enum Slider_Type
{
    Default,
    Boss,
    Dungeon,
}

public enum ItemType
{
    ALL,
    Equipment,
    Consumable,   
    ETC,
}

public enum Hero_Name
{
    Dual_Blader = 0,
    Elemental_Master_White = 1,
    Hunter = 2,
    PalaDin = 3,
    Elemental_Master_Black = 4,
    Sword_Master = 5,

}

public enum Relic_Name
{
    SWORD = 0,
    DICE = 1,
    MANA = 2,
    DEF = 3,
    HP = 4,
}

public enum Stage_State
{
    Ready,
    Play,
    Boss,
    BossPlay,
    Clear,
    Dead,
    Dungeon,
    Dungeon_Clear,
    Dungeon_Dead,

}