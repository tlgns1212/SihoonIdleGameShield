using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    SpawningPool _spawningPool;
    GameManager _game;
    PlayerController _player;
    UI_GameScene _ui;

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.GameScene;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _game = Managers.Game;

        if (_game.ContinueInfo.IsContinue == true)
        {
            _player = Managers.Object.Spawn<PlayerController>(Vector3.zero, _game.ContinueInfo.PlayerDataID);
        }
        else
        {
            _game.ClearContinueData();
            // 기본 플레이어 정보
            _player = Managers.Object.Spawn<PlayerController>(Vector3.zero, 10000001);
        }

        LoadStage();
        Managers.Game.CameraController = FindObjectOfType<CameraController>();

        _ui = Managers.UI.ShowSceneUI<UI_GameScene>();


    }


    public void LoadStage()
    {
        if (_spawningPool == null)
            _spawningPool = gameObject.AddComponent<SpawningPool>();

        // TODO MapName
        Managers.Object.LoadMap("MapName");
    }

    public override void Clear()
    {
    }

}
