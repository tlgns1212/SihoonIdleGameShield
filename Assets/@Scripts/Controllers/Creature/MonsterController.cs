using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{

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

    public override void OnDead()
    {
        base.OnDead();

    }
}
