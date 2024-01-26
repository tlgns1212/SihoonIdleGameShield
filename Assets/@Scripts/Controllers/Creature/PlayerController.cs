using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 하이어라키에서 보기 위함
[Serializable]
public class PlayerStat
{
    public int DataID;
    public float Hp;
    public float MaxHp;
    public float MaxHpBonus;
    public float Atk;
    public float AtkBonus;
    public float MoveSpeed;
    public float MoveSpeedBonus;
    public float AtkRate;
    public float AtkRateBonus;
    public float CriRate;
    public float CriRateBonus;
    public float CriDamage;
    public float CriDamageBonus;
}

public class PlayerController : CreatureController
{
    public PlayerStat StatViewer = new PlayerStat();
    protected MonsterController _targetMC;


    public override int DataID
    {
        get { return Managers.Game.ContinueInfo.PlayerDataID; }
        set { Managers.Game.ContinueInfo.PlayerDataID = StatViewer.DataID = value; }
    }
    public override float Hp
    {
        get { return Managers.Game.ContinueInfo.Hp; }
        set { Managers.Game.ContinueInfo.Hp = StatViewer.Hp = value; }
    }
    public override float MaxHp
    {
        get { return Managers.Game.ContinueInfo.MaxHp; }
        set { Managers.Game.ContinueInfo.MaxHp = StatViewer.MaxHp = value; }
    }
    public float Atk
    {
        get { return Managers.Game.ContinueInfo.Atk; }
        // set { Managers.Game.ContinueInfo.Atk = StatViewer.Atk = value; Managers.Game.SaveGame(); }
    }
    public float MoveSpeed
    {
        get { return Managers.Game.ContinueInfo.MoveSpeed; }
        //set { Managers.Game.ContinueInfo.MoveSpeed = StatViewer.MoveSpeed = value; ChangeAnimSpeed(); Managers.Game.SaveGame(); }
    }
    public float AtkRate
    {
        get { return Managers.Game.ContinueInfo.AtkRate; }
        //set { Managers.Game.ContinueInfo.AtkRate = StatViewer.AtkRate = value; ChangeAnimSpeed(); Managers.Game.SaveGame(); }
    }
    public float CriRate
    {
        get { return Managers.Game.ContinueInfo.CriRate; }
        //set { Managers.Game.ContinueInfo.CriRate = StatViewer.CriRate = value; Managers.Game.SaveGame(); }
    }
    public float CriDamage
    {
        get { return Managers.Game.ContinueInfo.CriDamage; }
        //set { Managers.Game.ContinueInfo.CriDamage = StatViewer.CriDamage = value; Managers.Game.SaveGame(); }
    }

    [SerializeField]
    bool _shouldAttack = false;
    bool ShouldAttack
    {
        get { return _shouldAttack; }
        set
        {
            _shouldAttack = value;
            Anim.SetBool("ShouldAttack", _shouldAttack);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        FindObjectOfType<CameraController>()._playerTransform = gameObject.transform;
        CreatureState = Define.CreatureState.Moving;

        return true;
    }

    public override void InitCreatureStat(bool isFullHp = true)
    {
        base.InitCreatureStat(isFullHp);
        // 플레이어는 필요 없음
        // Atk = CreatureData.Atk;
        // AtkBonus = CreatureData.AtkBonus;
        // MoveSpeed = CreatureData.MoveSpeed;
        // MoveSpeedBonus = CreatureData.MoveSpeedBonus;
        // AtkRate = CreatureData.AtkRate;
        // AtkRateBonus = CreatureData.AtkRateBonus;
        // CriDamage = CreatureData.CriDamage;
        // CriDamageBonus = CreatureData.CriDamageBonus;
        // CriRate = CreatureData.CriRate;
        // CriRateBonus = CreatureData.CriRateBonus;
    }

    protected override void UpdateMoving()
    {
        ShouldAttack = false;
        Vector2 moveVel = Vector2.right * (BaseSpeed + MoveSpeed);
        ChangeAnimSpeed();
        _rigidBody.AddForce(moveVel, ForceMode2D.Force);
    }

    public void ChangeAnimSpeed()
    {
        if (CreatureState == Define.CreatureState.Attacking)
            Anim.speed = AtkRate;
        else if (CreatureState == Define.CreatureState.Attacking)
            Anim.speed = MoveSpeed <= 0 ? 1 : Mathf.Min(MoveSpeed / 3, 5);
    }

    protected override void UpdateAttacking()
    {
        ShouldAttack = true;
        ChangeAnimSpeed();
        _rigidBody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Monster")) return;
        _targetMC = other.GetOrAddComponent<MonsterController>();
        CreatureState = Define.CreatureState.Attacking;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Monster")) return;
        _targetMC = null;
        CreatureState = Define.CreatureState.Moving;
    }

    private void Update()
    {
        if (CreatureState == Define.CreatureState.Attacking)
            return;
    }

    public void OnAttackEvent()
    {
        if (_attackCoroutine == null)
            StartCoroutine(Attack());
    }
    Coroutine _attackCoroutine;

    IEnumerator Attack()
    {
        if (_targetMC.IsValid())
        {
            _targetMC.OnDamaged(this, Atk);
        }
        yield return null;
    }
}







