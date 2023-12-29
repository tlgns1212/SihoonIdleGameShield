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
        if (goldDRate >= 1.0f)
        {
            Managers.Object.ShowResourceFont(CenterPosition, transform, (Managers.Game.UserLevel * goldDRate).ToString(), Define.ResourceType.Gold);
        }
        else
        {
            if (UnityEngine.Random.value < goldDRate)
                Managers.Object.ShowResourceFont(CenterPosition, transform, Managers.Game.UserLevel.ToString(), Define.ResourceType.Gold);
        }
        if (manaDRate >= 1.0f)
        {
            Managers.Object.ShowResourceFont(CenterPosition, transform, (Managers.Game.UserLevel * manaDRate).ToString(), Define.ResourceType.Mana);
        }
        else
        {
            if (UnityEngine.Random.value < manaDRate)
                Managers.Object.ShowResourceFont(CenterPosition, transform, Managers.Game.UserLevel.ToString(), Define.ResourceType.Mana);
        }
        if (dimensionEnergyDRate >= 1.0f)
        {
            Managers.Object.ShowResourceFont(CenterPosition, transform, (Managers.Game.UserLevel * dimensionEnergyDRate).ToString(), Define.ResourceType.DimensionEnergy);
        }
        else
        {
            if (UnityEngine.Random.value < dimensionEnergyDRate)
                Managers.Object.ShowResourceFont(CenterPosition, transform, Managers.Game.UserLevel.ToString(), Define.ResourceType.DimensionEnergy);
        }
        if (rubyDRate >= 1.0f)
        {
            Managers.Object.ShowResourceFont(CenterPosition, transform, (Managers.Game.UserLevel * rubyDRate).ToString(), Define.ResourceType.Ruby);
        }
        else
        {
            if (UnityEngine.Random.value < rubyDRate)
                Managers.Object.ShowResourceFont(CenterPosition, transform, Managers.Game.UserLevel.ToString(), Define.ResourceType.Ruby);
        }
    }
}
