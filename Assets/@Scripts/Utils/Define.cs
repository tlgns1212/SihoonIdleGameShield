public class Define
{
    public enum Scene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
    }

    public enum ObjectType
    {
        Player,
        Monster,
        Criminal,
        RaidMonster,
        Friend
    }

    public enum CreatureState
    {
        Idle,
        Moving,
        Attacking,
    }

    public enum DungeonType
    {
        DungeonBreak,
        BossRaid,
        PVP,
        Mine,
    }

    public enum EquipmentType
    {
        Helmet,
        Necklace,
        Armor,
        Gloves,
        Boots,
        Belt,
        Ring,
    }

    public enum JewelGrade
    {
        EX,
        SSS,
        SS,
        S,
        A,
        B,
        C,
        D,
        E,
        F
    }

    public enum JewelSelectType
    {
        Nothing,
        Assemble,
        Disassemble,
        Sell,
    }

    public enum JewelSortType
    {
        Grade,
        Name,
        GetDate,
    }

    public enum FriendEffectType
    {
        Atk,
        AtkRate,
        etc,
    }

    public enum AccessoriesType
    {
        NoMaxAccessoriesItem,
        MaxAccessoriesItem
    }

    public enum AccessoriesItemType
    {
        AtkDamage,
        AtkRate,
        MoveSpeed,
        CriticalDamage,
        CriticalRate,
        KillGoldRate,
        SaveGoldRate,
        ManaGetRate,
        DEnergyGetRate,
        RubyGetRate,
        ShieldSaleRate,
        SaveSaleRate
    }

    public enum ResourceType
    {
        Gold,
        Mana,
        DimensionEnergy,
        Ruby
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        Drag,
        BeginDrag,
        EndDrag,
    }

    public enum Sound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }
}
