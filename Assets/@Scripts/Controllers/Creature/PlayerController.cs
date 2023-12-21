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
}
