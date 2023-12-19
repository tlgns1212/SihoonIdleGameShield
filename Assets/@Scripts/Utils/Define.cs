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
