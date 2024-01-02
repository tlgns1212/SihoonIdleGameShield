using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class CreatureController : BaseController
{
    [SerializeField]
    protected SpriteRenderer CreatureSprite;
    protected string SpriteName;
    public Material DefaultMat;
    public Material HitEffectMat;
    [SerializeField]
    protected bool isPlayDamagedAnim = false;

    public Rigidbody2D _rigidBody { get; set; }
    public Animator Anim { get; set; }
    public CreatureData CreatureData;
    public UI_HpBar HpBar;

    public virtual int DataID { get; set; }
    public virtual float Hp { get; set; }
    public virtual float MaxHp { get; set; }
    public virtual float MaxHpBonus { get; set; }
    public float BaseSpeed { get; } = 40f;

    public Vector3 CenterPosition
    {
        get { return _offset.bounds.center; }
    }

    private Collider2D _offset;
    Define.CreatureState _creatureState = Define.CreatureState.Idle;
    public virtual Define.CreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }

    void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _rigidBody = GetComponent<Rigidbody2D>();
        _offset = GetComponent<Collider2D>();
        CreatureSprite = GetComponent<SpriteRenderer>();
        if (CreatureSprite == null)
            CreatureSprite = Util.FindChild<SpriteRenderer>(gameObject);

        Anim = GetComponent<Animator>();
        if (Anim == null)
            Anim = Util.FindChild<Animator>(gameObject);

        HpBar = GetComponent<UI_HpBar>();
        if (HpBar == null)
            HpBar = Util.FindChild<UI_HpBar>(gameObject);

        return true;
    }

    public virtual void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                UpdateIdle();
                break;
            case Define.CreatureState.Attacking:
                UpdateAttacking();
                break;
            case Define.CreatureState.Moving:
                UpdateMoving();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateAttacking() { }
    protected virtual void UpdateMoving() { }

    public virtual void OnDamaged(BaseController attacker, float damage = 0)
    {
        bool isCritical = false;
        PlayerController player = attacker as PlayerController;
        if (player != null)
        {
            if (UnityEngine.Random.value <= player.CriRate)
            {
                damage = damage * player.CriDamage;
                isCritical = true;
            }
        }

        Hp -= damage;
        Managers.Object.ShowDamageFont(CenterPosition, damage, transform, isCritical);

        if (gameObject.IsValid() || this.IsValid())
            StartCoroutine(PlayDamageAnimation());
    }

    public virtual void OnDead()
    {
        transform.position = new Vector3(Managers.Game.Player.transform.position.x + 10, 0, 0);
        // TODO 몬스터 ID 찾아주기
        SetInfo(10000002);
    }

    public virtual void InitCreatureStat(bool isFullHp = true)
    {
        // TODO 웨이브 별로 체력 증가량 넣기
        // float waveRate = Managers.Game.CurrentWaveData.HpIncreaseRate;

        MaxHp = CreatureData.MaxHp + (CreatureData.MaxHpBonus /*  * waveRate*/);
        Hp = MaxHp;
        MaxHpBonus = CreatureData.MaxHpBonus;
    }

    public virtual void SetInfo(int creatureID)
    {
        DataID = creatureID;
        CreatureData = Managers.Data.CreatureDic[creatureID];
        InitCreatureStat();
        CreatureSprite.sprite = Managers.Resource.Load<Sprite>(CreatureData.IconLabel);

        Init();
    }

    public bool IsMonster()
    {
        switch (ObjectType)
        {
            case Define.ObjectType.Monster:
            case Define.ObjectType.Criminal:
            case Define.ObjectType.RaidMonster:
                return true;
            default:
                return false;
        }
    }

    IEnumerator PlayDamageAnimation()
    {
        if (isPlayDamagedAnim == false)
        {
            isPlayDamagedAnim = true;
            DefaultMat = Managers.Resource.Load<Material>("CreatureDefaultMat");
            HitEffectMat = Managers.Resource.Load<Material>("DamagedEffectMat");

            // Damaged Animation
            CreatureSprite.material = HitEffectMat;
            yield return new WaitForSeconds(0.1f);
            CreatureSprite.material = DefaultMat;

            if (Hp <= 0)
            {
                OnDead();
            }
            isPlayDamagedAnim = false;
        }
    }

    private void Update()
    {
        HpBar.SetHpRatio(Hp / MaxHp);
    }
}
