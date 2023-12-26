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
}
