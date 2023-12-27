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
            Managers.UI.ShowResourceToast("Gold.sprite", (Managers.Game.UserLevel * goldDRate).ToString());
        }
        else
        {
            if (UnityEngine.Random.value < goldDRate)
                Managers.UI.ShowResourceToast("Gold.sprite", Managers.Game.UserLevel.ToString());
        }
        if (manaDRate >= 1.0f)
        {
            Managers.UI.ShowResourceToast("Mana.sprite", (Managers.Game.UserLevel * manaDRate).ToString());
        }
        else
        {
            if (UnityEngine.Random.value < manaDRate)
                Managers.UI.ShowResourceToast("Mana.sprite", Managers.Game.UserLevel.ToString());
        }
        if (dimensionEnergyDRate >= 1.0f)
        {
            Managers.UI.ShowResourceToast("DimensionEnergy.sprite", (Managers.Game.UserLevel * dimensionEnergyDRate).ToString());
        }
        else
        {
            if (UnityEngine.Random.value < dimensionEnergyDRate)
                Managers.UI.ShowResourceToast("DimensionEnergy.sprite", Managers.Game.UserLevel.ToString());
        }
        if (rubyDRate >= 1.0f)
        {
            Managers.UI.ShowResourceToast("Ruby.sprite", (Managers.Game.UserLevel * rubyDRate).ToString());
        }
        else
        {
            if (UnityEngine.Random.value < rubyDRate)
                Managers.UI.ShowResourceToast("Ruby.sprite", Managers.Game.UserLevel.ToString());
        }
    }
}
