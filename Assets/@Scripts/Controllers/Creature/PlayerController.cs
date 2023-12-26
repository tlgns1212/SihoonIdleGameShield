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
    public override float MaxHpBonus
    {
        get { return Managers.Game.ContinueInfo.MaxHpBonus; }
        set { Managers.Game.ContinueInfo.MaxHpBonus = StatViewer.MaxHpBonus = value; }
    }
    public float Atk
    {
        get { return Managers.Game.ContinueInfo.Atk; }
        set { Managers.Game.ContinueInfo.Atk = StatViewer.Atk = value; }
    }
    public float AtkBonus
    {
        get { return Managers.Game.ContinueInfo.AtkBonus; }
        set { Managers.Game.ContinueInfo.AtkBonus = StatViewer.AtkBonus = value; }
    }
    public float MoveSpeed
    {
        get { return Managers.Game.ContinueInfo.MoveSpeed; }
        set { Managers.Game.ContinueInfo.MoveSpeed = StatViewer.MoveSpeed = value; }
    }
    public float MoveSpeedBonus
    {
        get { return Managers.Game.ContinueInfo.MoveSpeedBonus; }
        set { Managers.Game.ContinueInfo.MoveSpeedBonus = StatViewer.MoveSpeedBonus = value; }
    }
    public float AtkRate
    {
        get { return Managers.Game.ContinueInfo.AtkRate; }
        set { Managers.Game.ContinueInfo.AtkRate = StatViewer.AtkRate = value; }
    }
    public float AtkRateBonus
    {
        get { return Managers.Game.ContinueInfo.AtkRateBonus; }
        set { Managers.Game.ContinueInfo.AtkRateBonus = StatViewer.AtkRateBonus = value; }
    }
    public float CriRate
    {
        get { return Managers.Game.ContinueInfo.CriRate; }
        set { Managers.Game.ContinueInfo.CriRate = StatViewer.CriRate = value; }
    }
    public float CriRateBonus
    {
        get { return Managers.Game.ContinueInfo.CriRateBonus; }
        set { Managers.Game.ContinueInfo.CriRateBonus = StatViewer.CriRateBonus = value; }
    }
    public float CriDamage
    {
        get { return Managers.Game.ContinueInfo.CriDamage; }
        set { Managers.Game.ContinueInfo.CriDamage = StatViewer.CriDamage = value; }
    }
    public float CriDamageBonus
    {
        get { return Managers.Game.ContinueInfo.CriDamageBonus; }
        set { Managers.Game.ContinueInfo.CriDamageBonus = StatViewer.CriDamageBonus = value; }
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
        Atk = CreatureData.Atk;
        AtkBonus = CreatureData.AtkBonus;
        MoveSpeed = CreatureData.MoveSpeed;
        MoveSpeedBonus = CreatureData.MoveSpeedBonus;
        AtkRate = CreatureData.AtkRate;
        AtkRateBonus = CreatureData.AtkRateBonus;
        CriDamage = CreatureData.CriDamage;
        CriDamageBonus = CreatureData.CriDamageBonus;
        CriRate = CreatureData.CriRate;
        CriRateBonus = CreatureData.CriRateBonus;
    }

    protected override void UpdateMoving()
    {
        ShouldAttack = false;
        Vector2 moveVel = Vector2.right * (BaseSpeed + MoveSpeed * MoveSpeedBonus);
        _rigidBody.AddForce(moveVel, ForceMode2D.Force);
    }

    protected override void UpdateAttacking()
    {
        ShouldAttack = true;
        _rigidBody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _targetMC = other.GetOrAddComponent<MonsterController>();
        CreatureState = Define.CreatureState.Attacking;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _targetMC = null;
        CreatureState = Define.CreatureState.Moving;
    }

    private void Update()
    {
        if (CreatureState == Define.CreatureState.Attacking)
            return;
        // Vector3 moveVel = Vector3.right * MoveSpeed * MoveSpeedBonus * Time.deltaTime;
        // transform.position = transform.position + moveVel;
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
            _targetMC.OnDamaged(this, 15);
        }
        yield return null;

    }
}
