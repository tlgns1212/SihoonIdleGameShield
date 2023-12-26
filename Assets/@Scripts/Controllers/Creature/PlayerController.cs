using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
    // public override int DataID { get => base.DataID; set => base.DataID = value; }
    // public override float Hp { get => base.Hp; set => base.Hp = value; }
    // public override float MaxHp { get => base.MaxHp; set => base.MaxHp = value; }
    public float Atk { get; set; }
    public float AtkBonus { get; set; }
    public float MoveSpeed { get; set; }
    public float MoveSpeedBonus { get; set; }
    public float AtkRate { get; set; }
    public float AtkRateBonus { get; set; }
    public float CriRate { get; set; }
    public float CriRateBonus { get; set; }
    public float CriDamage { get; set; }
    public float CriDamageBonus { get; set; }

    [SerializeField]
    bool _shouldAttack = false;
    bool ShouldAttack
    {
        get { return _shouldAttack; }
        set { _shouldAttack = value;
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

    protected override void UpdateMoving()
    {
        ShouldAttack = false;
        Vector3 moveVel = (Vector3.forward * MoveSpeed) * MoveSpeedBonus;
        _rigidBody.AddForce(moveVel, ForceMode2D.Force);
    }

    protected override void UpdateAttacking()
    {
        ShouldAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CreatureState = Define.CreatureState.Attacking;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CreatureState = Define.CreatureState.Moving;
    }
}
