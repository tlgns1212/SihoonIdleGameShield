using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class MonsterController : CreatureController
{
    protected MonsterData MonsterData;

    private void OnEnable()
    {
        if (DataID != 0)
            SetInfo(DataID);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = Define.ObjectType.Monster;
        CreatureState = Define.CreatureState.Idle;

        _rigidBody.simulated = true;

        return true;
    }

    public override void SetInfo(int creatureID)
    {
        DataID = creatureID;
        MonsterData = Managers.Data.MonsterDic[creatureID];
        InitCreatureStat();
        CreatureSprite.sprite = Managers.Resource.Load<Sprite>(MonsterData.IconLabel);
    }

    public override void InitCreatureStat(bool isFullHp = true)
    {
        // TODO 웨이브 별로 체력 증가량 넣기
        // float waveRate = Managers.Game.CurrentWaveData.HpIncreaseRate;

        MaxHp = MonsterData.MaxHp + (MonsterData.MaxHpBonus /*  * waveRate*/);
        Hp = MaxHp;
        MaxHpBonus = MonsterData.MaxHpBonus;
    }


    public override void OnDead()
    {
        base.OnDead();
        float goldDRate = MonsterData.GoldDropRate;
        float manaDRate = MonsterData.ManaDropRate;
        float dimensionEnergyDRate = MonsterData.DimensionEnergyDropRate;
        float rubyDRate = MonsterData.RubyDropRate;

        float gotGold = 0;
        float gotMana = 0;
        float gotDEnergy = 0;
        float gotRuby = 0;
        if (goldDRate >= 1.0f)
        {
            gotGold = Managers.Game.UserLevel * goldDRate * Managers.Game.ContinueInfo.KillGold;
            Managers.Object.ShowResourceFont(CenterPosition, transform, gotGold.ToString(), Define.ResourceType.Gold);
            Managers.Game.Gold += (int)gotGold;
        }
        else
        {
            if (UnityEngine.Random.value < goldDRate)
            {
                gotGold = Managers.Game.UserLevel * Managers.Game.ContinueInfo.KillGold;
                Managers.Object.ShowResourceFont(CenterPosition, transform, gotGold.ToString(), Define.ResourceType.Gold);
                Managers.Game.Gold += (int)gotGold;
            }
        }
        if (manaDRate >= 1.0f)
        {
            gotMana = Managers.Game.UserLevel * manaDRate * Managers.Game.ContinueInfo.ManaGetRate;
            Managers.Object.ShowResourceFont(CenterPosition, transform, gotMana.ToString(), Define.ResourceType.Mana);
            Managers.Game.Mana += (int)gotMana;
        }
        else
        {
            if (UnityEngine.Random.value < manaDRate)
            {
                gotMana = Managers.Game.UserLevel * Managers.Game.ContinueInfo.ManaGetRate;
                Managers.Object.ShowResourceFont(CenterPosition, transform, gotMana.ToString(), Define.ResourceType.Mana);
                Managers.Game.Mana += (int)gotMana;
            }
                
        }
        if (dimensionEnergyDRate >= 1.0f)
        {
            gotDEnergy = Managers.Game.UserLevel * dimensionEnergyDRate * Managers.Game.ContinueInfo.DEnergyGetRate;
            Managers.Object.ShowResourceFont(CenterPosition, transform, gotDEnergy.ToString(), Define.ResourceType.DimensionEnergy);
            Managers.Game.DimensionEnergy += (int)gotDEnergy;
        }
        else
        {
            if (UnityEngine.Random.value < dimensionEnergyDRate)
            {
                gotDEnergy = Managers.Game.UserLevel * Managers.Game.ContinueInfo.DEnergyGetRate;
                Managers.Object.ShowResourceFont(CenterPosition, transform, gotDEnergy.ToString(), Define.ResourceType.DimensionEnergy);
                Managers.Game.DimensionEnergy += (int)gotDEnergy;
            }
                
        }
        if (rubyDRate >= 1.0f)
        {
            gotRuby = Managers.Game.UserLevel * rubyDRate * Managers.Game.ContinueInfo.RubyGetRate;
            Managers.Object.ShowResourceFont(CenterPosition, transform, gotRuby.ToString(), Define.ResourceType.Ruby);
            Managers.Game.Ruby += (int)gotRuby;
        }
        else
        {
            if (UnityEngine.Random.value < rubyDRate)
            {
                gotRuby = Managers.Game.UserLevel * Managers.Game.ContinueInfo.RubyGetRate;
                Managers.Object.ShowResourceFont(CenterPosition, transform, gotRuby.ToString(), Define.ResourceType.Ruby);
                Managers.Game.Ruby += (int)gotRuby;
            }
                
        }
    }
}
