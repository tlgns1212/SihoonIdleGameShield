using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    #region  CreatureData
    public class CreatureData
    {
        public int DataID;
        public string DescriptionTextID;
        public string PrefabLabel;
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
        public string IconLabel;
    }

    [Serializable]
    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> creatures = new List<CreatureData>();
        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
            foreach (CreatureData creature in creatures)
                dict.Add(creature.DataID, creature);
            return dict;
        }
    }
    #endregion

    #region MonsterData
    public class MonsterData
    {
        public int DataID;
        public string DescriptionTextID;
        public string PrefabLabel;
        public float MaxHp;
        public float MaxHpBonus;
        public float GoldDropRate;
        public float ManaDropRate;
        public float DimensionEnergyDropRate;
        public float RubyDropRate;
        public string IconLabel;
    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();
        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
                dict.Add(monster.DataID, monster);
            return dict;
        }
    }
    #endregion

    #region LevelData
    public class LevelData
    {
        public int Level;
        public int LValue;
        public int NextCost;
    }
    #endregion

    #region AccessoriesData
    public class AccessoriesData
    {
        public int DataID;
        public string TitleText;
        public string PrefabLabel;
        public string ItemEffectText;
        public string ItemEffectNumText;
        public string IconLabel;
        public string BuyResource;
        public List<LevelData> LevelDatas = new List<LevelData>();
        public List<LevelData> RaiseLevelDatas = new List<LevelData>();
    }

    [Serializable]
    public class AccessoriesDataLoader : ILoader<int, AccessoriesData>
    {
        public List<AccessoriesData> accessories = new List<AccessoriesData>();
        public Dictionary<int, AccessoriesData> MakeDict()
        {
            Dictionary<int, AccessoriesData> dict = new Dictionary<int, AccessoriesData>();
            foreach (AccessoriesData accessory in accessories)
                dict.Add(accessory.DataID, accessory);
            return dict;
        }
    }
    #endregion

    #region DungeonData
    public class DungeonData
    {
        public int DataID;
        public string TitleText;
        public string MaxVisitText;
        public int MaxVisitNum;
        public int LockOpenLevel;
        public string PrefabLabel;
        public string IconLabel;
        public string BuyResource;
    }

    [Serializable]
    public class DungeonDataLoader : ILoader<int, DungeonData>
    {
        public List<DungeonData> dungeons = new List<DungeonData>();
        public Dictionary<int, DungeonData> MakeDict()
        {
            Dictionary<int, DungeonData> dict = new Dictionary<int, DungeonData>();
            foreach (DungeonData dungeon in dungeons)
                dict.Add(dungeon.DataID, dungeon);
            return dict;
        }
    }
    #endregion

    #region FriendData
    public class FriendData
    {
        public int DataID;
        public string TitleText;
        public string ItemEffectText;
        public int MaxLevel;
        public string PrefabLabel;
        public string IconLabel;
        public string BuyResource;
        public List<LevelData> LevelDatas = new List<LevelData>();
    }

    [Serializable]
    public class FriendDataLoader : ILoader<int, FriendData>
    {
        public List<FriendData> friends = new List<FriendData>();
        public Dictionary<int, FriendData> MakeDict()
        {
            Dictionary<int, FriendData> dict = new Dictionary<int, FriendData>();
            foreach (FriendData friend in friends)
                dict.Add(friend.DataID, friend);
            return dict;
        }
    }
    #endregion

    #region JewelData
    public class JewelData
    {
        public int DataID;
        public string JewelName;
        public string Grade;
        public string PrefabLabel;
        public string IconLabel;
    }

    [Serializable]
    public class JewelDataLoader : ILoader<int, JewelData>
    {
        public List<JewelData> jewels = new List<JewelData>();
        public Dictionary<int, JewelData> MakeDict()
        {
            Dictionary<int, JewelData> dict = new Dictionary<int, JewelData>();
            foreach (JewelData jewel in jewels)
                dict.Add(jewel.DataID, jewel);
            return dict;
        }
    }
    #endregion

    #region SaviourData
    public class SaviourData
    {
        public int DataID;
        public string TitleText;
        public string PrefabLabel;
        public string IconLabel;
        public string BuyResource;
        public List<LevelData> LevelDatas = new List<LevelData>();
        public List<LevelData> RaiseLevelDatas = new List<LevelData>();
    }

    [Serializable]
    public class SaviourDataLoader : ILoader<int, SaviourData>
    {
        public List<SaviourData> saviours = new List<SaviourData>();
        public Dictionary<int, SaviourData> MakeDict()
        {
            Dictionary<int, SaviourData> dict = new Dictionary<int, SaviourData>();
            foreach (SaviourData saviour in saviours)
                dict.Add(saviour.DataID, saviour);
            return dict;
        }
    }
    #endregion

    #region ShieldData
    public class ShieldData
    {
        public int DataID;
        public string TitleText;
        public string ItemEffectText;
        public string PrefabLabel;
        public string IconLabel;
        public string BuyResource;
        public List<LevelData> LevelDatas = new List<LevelData>();
        public List<LevelData> RaiseLevelDatas = new List<LevelData>();
    }

    [Serializable]
    public class ShieldDataLoader : ILoader<int, ShieldData>
    {
        public List<ShieldData> shields = new List<ShieldData>();
        public Dictionary<int, ShieldData> MakeDict()
        {
            Dictionary<int, ShieldData> dict = new Dictionary<int, ShieldData>();
            foreach (ShieldData shield in shields)
                dict.Add(shield.DataID, shield);
            return dict;
        }
    }
    #endregion
}
