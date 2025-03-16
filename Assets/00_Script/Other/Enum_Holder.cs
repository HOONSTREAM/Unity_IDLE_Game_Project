
public enum Rarity
{
    Common,
    UnCommon,
    Rare,
    Epic,
    Legendary
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
    ATK, // ���ݷ�
    HP, // ü��
    MONEY, // ��� �����
    ITEM, // ������ �����
    ATK_SPEED, // ���ݷ�
    CRITICAL_PERCENTAGE, //ũ��Ƽ�� Ȯ��
    CRITICAL_DAMAGE, // ũ��Ƽ�� ������
}

public enum Holding_Effect_Type
{
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