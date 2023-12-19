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
    // public Dictionary<int, Data.SectionItemData> SectionItemDataDic { get; private set; } = new Dictionary<int, Data.SectionItemData>();

    public void Init()
    {
        // SectionItemDataDic = LoadJson<Data.SectionItemDataLoader, int, Data.SectionItemData>("SectionItemData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

}