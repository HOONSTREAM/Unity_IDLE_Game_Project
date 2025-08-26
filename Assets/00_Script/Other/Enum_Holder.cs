
public enum Rarity
{
    Common,
    UnCommon,
    Rare,
    Epic,
    Legendary,
    Chaos
}

public enum IAP_Holder
{
    dia_1400,
    dia_19000,
    dia_4900,
    dia_68000,
    dungeon_dia_20,
    gold_30,
    remove_ads,
    steel_1000,
    package_1,
    package_2,
    start,
    package_3,
    hondon_2000,
    dia_gacha,
    enhancement,
    def_enhancement,
    dia_pass,
    enhancement_package,
    ticket,
    research_book,
    research_package,
    hondon_comb,
    start_dia,
    sword_hondon,

}

public enum Sound
{
    BGM,
    BGS,
    MAX
}

public enum Status_Type
{
    Status,
    Mastery,
    Costume
}

public enum Daily_Quest_Type
{
    Daily_Attendance = 0,
    Level_up,
    Summon,
    Relic,
    Dungeon_Gold,
    Dungeon_Dia,
}

public enum Smelt_Status
{
    ATK, // 공격력
    HP, // 체력
    MONEY, // 골드 드랍률
    ITEM, // 아이템 드랍률
    ATK_SPEED, // 공격력
    CRITICAL_PERCENTAGE, //크리티컬 확률
    CRITICAL_DAMAGE, // 크리티컬 데미지
}

public enum Research_Stat
{

    ATK, // 공격력
    HP, // 체력
    MONEY, // 골드 드랍률
    ITEM, // 아이템 드랍률
    ATK_SPEED, // 공격력
    CRITICAL_PERCENTAGE, //크리티컬 확률
    CRITICAL_DAMAGE, // 크리티컬 데미지

}

public enum Player_Tier
{
    Tier_Beginner = 0,
    Tier_Bronze = 1,
    Tier_Silver = 2,
    Tier_Gold = 3,    
    Tier_Diamond = 4,
    Tier_Master = 5,
    Tier_Master_1 = 6,
    Tier_Master_2 = 7,
    Tier_Master_3 = 8,
    Tier_Master_4 = 9,
    Tier_Master_5 = 10,
    Tier_GrandMaster = 11,
    Tier_Challenger = 12,
    Tier_Challenger_1 = 13,
    Tier_Challenger_2 = 14,
    Tier_Challenger_3 = 15,
    Tier_Challenger_4 = 16,
    Tier_Challenger_5 = 17,
    Tier_Challenger_6 = 18,
    Tier_Challenger_7 = 19,
    Tier_Challenger_8 = 20,
    Tier_Challenger_9 = 21,
    Tier_Challenger_10 = 22,

}

public enum Holding_Effect_Type
{
    NONE,
    ATK,
    HP,
    ATK_SPEED,
    GOLD_DROP,
    ITEM_DROP,
    CRITICAL_DAMAGE,
    CRITICAL_PERCENTAGE
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