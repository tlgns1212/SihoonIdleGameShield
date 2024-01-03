using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public static readonly Dictionary<Type, Array> _enumDict = new Dictionary<Type, Array>();

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
        Sort,
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
