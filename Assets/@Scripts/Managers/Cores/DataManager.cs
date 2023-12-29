using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.CreatureData> CreatureDic { get; private set; } = new Dictionary<int, Data.CreatureData>();
    public Dictionary<int, Data.MonsterData> MonsterDic { get; private set; } = new Dictionary<int, Data.MonsterData>();
    public Dictionary<int, Data.AccessoriesData> AccessoriesDic { get; private set; } = new Dictionary<int, Data.AccessoriesData>();
    public Dictionary<int, Data.DungeonData> DungeonDic { get; private set; } = new Dictionary<int, Data.DungeonData>();
    public Dictionary<int, Data.FriendData> FriendDic { get; private set; } = new Dictionary<int, Data.FriendData>();
    public Dictionary<int, Data.JewelData> JewelDic { get; private set; } = new Dictionary<int, Data.JewelData>();
    public Dictionary<int, Data.SaviourData> SaviourDic { get; private set; } = new Dictionary<int, Data.SaviourData>();
    public Dictionary<int, Data.ShieldData> ShieldDic { get; private set; } = new Dictionary<int, Data.ShieldData>();

    public void Init()
    {
        CreatureDic = LoadJson<Data.CreatureDataLoader, int, Data.CreatureData>("CreatureData").MakeDict();
        MonsterDic = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
        AccessoriesDic = LoadJson<Data.AccessoriesDataLoader, int, Data.AccessoriesData>("AccessoriesData").MakeDict();
        DungeonDic = LoadJson<Data.DungeonDataLoader, int, Data.DungeonData>("DungeonData").MakeDict();
        FriendDic = LoadJson<Data.FriendDataLoader, int, Data.FriendData>("FriendData").MakeDict();
        JewelDic = LoadJson<Data.JewelDataLoader, int, Data.JewelData>("JewelData").MakeDict();
        SaviourDic = LoadJson<Data.SaviourDataLoader, int, Data.SaviourData>("SaviourData").MakeDict();
        ShieldDic = LoadJson<Data.ShieldDataLoader, int, Data.ShieldData>("ShieldData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

}