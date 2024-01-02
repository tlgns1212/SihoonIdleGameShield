using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using static Define;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using UnityEngine.Networking;



[Serializable]
public class GameData
{
    public int UserLevel = 1;
    public string UserName = "Player";

    public int Gold = 0;
    public int Mana = 0;
    public int Ruby = 0;
    public int DimensionEnergy = 0;

    public int BGMSound = 100;
    public int EffectSound = 100;

    public ContinueData ContinueInfo = new ContinueData();
    public MoneyBonusData MoneyBonusInfo = new MoneyBonusData();
    public Dictionary<int, int> JewelDictionary = new Dictionary<int, int>(); // <ID, 갯수>
    public Dictionary<int, LevelData> AccLevelDictionary = new Dictionary<int, LevelData>();
    public Dictionary<int, ShieldData> ShieldLevelDictionary = new Dictionary<int, ShieldData>();
    //TODO EquipDictionary, FriendDictionary, ShieldDictionary, SaviourDicitonary, 
}

[Serializable]
public class ContinueData
{
    public int PlayerDataID;
    public bool IsContinue { get { return PlayerDataID != 0; } }
    public float Hp;
    public float MaxHp;
    public float MaxHpBonus;
    public float Atk;
    public float AtkBonus;
    public float MoveSpeed;
    public float MoveSpeedBonus;
    public float AtkRate;
    public float AtkRateBonus;
    public float CriDamage;
    public float CriDamageBonus;
    public float CriRate;
    public float CriRateBonus;
    public int Level;
    public float Exp;
    public float TotalExp;
    public float KillGold;
    public float KillGoldBonus;
    public float WaitGold;
    public float WaitGoldBonus;


    public void Clear()
    {
        PlayerDataID = 0;
        Hp = 0f;
        MaxHp = 0f;
        MaxHpBonus = 1f;
        Atk = 0f;
        AtkBonus = 1f;
        MoveSpeed = 0f;
        MoveSpeedBonus = 1f;
        AtkRate = 0.1f;
        AtkRateBonus = 1f;
        CriDamage = 1f;
        CriDamageBonus = 1f;
        CriRate = 0f;
        CriRateBonus = 1f;
        Level = 1;
        Exp = 0f;
        TotalExp = 0f;
        KillGold = 1f;
        KillGoldBonus = 1f;
        WaitGold = 0f;
        WaitGoldBonus = 1f;
    }
}
[Serializable]
public class MoneyBonusData
{
    public float KillManaBonus = 1f;
    public float KillRubyBonus = 1f;
    public float KillDimensionEnergyBonus = 1f;
}
[Serializable]
public class LevelData
{
    public bool isOpen = true;
    public int Level = 0;
    public int Value = 0;
}
[Serializable]
public class ShieldData
{
    public int Level = 0;
    public bool isCompleted = false;
    public bool isLocked = false;
}

public class GameManager
{
    #region GameData
    public GameData _gameData = new GameData();
    public int UserLevel
    {
        get { return _gameData.UserLevel; }
        set { _gameData.UserLevel = value; }
    }
    public string UserName
    {
        get { return _gameData.UserName; }
        set { _gameData.UserName = value; }
    }
    public int Gold
    {
        get { return _gameData.Gold; }
        set
        {
            _gameData.Gold = value;
            // TODO 재화 습득시 저장이 아니라 일정 시간마다 저장으로 하
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }
    public int Mana
    {
        get { return _gameData.Mana; }
        set
        {
            _gameData.Mana = value;
            // TODO 재화 습득시 저장이 아니라 일정 시간마다 저장으로 하기
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }
    public int Ruby
    {
        get { return _gameData.Ruby; }
        set
        {
            _gameData.Ruby = value;
            // TODO 재화 습득시 저장이 아니라 일정 시간마다 저장으로 하기
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }
    public int DimensionEnergy
    {
        get { return _gameData.DimensionEnergy; }
        set
        {
            _gameData.DimensionEnergy = value;
            // TODO 재화 습득시 저장이 아니라 일정 시간마다 저장으로 하기
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }
    public int BGMSound
    {
        get { return _gameData.BGMSound; }
        set { _gameData.BGMSound = value; }
    }
    public int EffectSound
    {
        get { return _gameData.EffectSound; }
        set { _gameData.EffectSound = value; }
    }
    public ContinueData ContinueInfo
    {
        get { return _gameData.ContinueInfo; }
        set { _gameData.ContinueInfo = value; }
    }
    public MoneyBonusData MoneyBonusInfo
    {
        get { return _gameData.MoneyBonusInfo; }
        set { _gameData.MoneyBonusInfo = value; }
    }
    public Dictionary<int, int> JewelDictionary
    {
        get { return _gameData.JewelDictionary; }
        set { _gameData.JewelDictionary = value; }
    }
    public Dictionary<int, LevelData> AccLevelDictionary
    {
        get { return _gameData.AccLevelDictionary; }
        set { _gameData.AccLevelDictionary = value; }
    }
    public Dictionary<int, ShieldData> ShieldLevelDictionary
    {
        get { return _gameData.ShieldLevelDictionary; }
        set { _gameData.ShieldLevelDictionary = value; }
    }


    #region Action
    public event Action OnResourcesChanged;
    #endregion

    public CameraController CameraController { get; set; }

    #region Player
    public PlayerController Player { get; set; }
    #endregion

    #region Option
    public bool EffectSoundOn
    {
        get { return EffectSoundOn; }
        set { EffectSoundOn = value; }
    }

    public bool BGMOn
    {
        get { return BGMOn; }
        set
        {
            if (BGMOn == value)
                return;
            BGMOn = value;
            if (BGMOn == false)
            {
                Managers.Sound.Stop(Define.Sound.Bgm);
            }
            else
            {
                string name = "Bgm_Lobby";
                if (Managers.Scene.CurrentScene.SceneType == Define.Scene.GameScene)
                    name = "Bgm_Game";

                Managers.Sound.Play(Define.Sound.Bgm, name);
            }
        }
    }

    #endregion
    #endregion

    public void RefreshGame()
    {
        _path = Application.persistentDataPath + "/" + UserName + ".json";
        _gameData = new GameData();
        SaveGame();
    }

    public void Init()
    {
        _path = Application.persistentDataPath + "/" + UserName + ".json";

        if (File.Exists(_path))
        {
            string fileStr = File.ReadAllText(_path);
            GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
            if (data != null)
                _gameData = data;
        }

        SaveGame();
    }

    #region Save&Load
    public string _path = "";
    public void SaveGame()
    {
        _path = Application.persistentDataPath + "/" + UserName + ".json";
        string jsonStr = JsonConvert.SerializeObject(_gameData);
        File.WriteAllText(_path, jsonStr);
        Debug.Log($"Save Game Completed : {_path}");
    }

    public bool LoadGame()
    {
        _path = Application.persistentDataPath + "/" + UserName + ".json";

        // TODO TEmp Clear
        // _gameData.ContinueInfo.Clear();

        if (File.Exists(_path) == false)
        {
            Init();
            return false;
        }

        string fileStr = File.ReadAllText(_path);
        GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
        if (data != null)
            _gameData = data;

        Debug.Log($"Save Game Loaded : {_path}");
        return true;
    }

    public void ClearContinueData()
    {
        // TODO Clear 후 저장
    }

    #endregion

}
