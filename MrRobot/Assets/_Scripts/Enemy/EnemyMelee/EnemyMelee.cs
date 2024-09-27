using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct AttackData
{
    public string attackName;
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;
    [Range(1,2)]
    public float animationSpeed;
    public AttackTypeMelee AttackTypeMelee;
}

public enum AttackTypeMelee
{
    Close,
    Charge
}

public enum EnemyMeleeType
{
    Regular,
    Shield,
    Dodge,
    AxeThrow
}


public class EnemyMelee : Enemy
{
    public IdleStateMelee IdleStateMelee { get; private set; }
    public MoveStateMelee MoveStateMelee { get; private set; }
    public RecoveryStateMelee RecoveryStateMelee { get; private set; }
    public ChaseStateMelee ChaseStateMelee { get; private set; }
    public AttackStateMelee AttackStateMelee { get; private set; }
    public DeadStateMelee DeadStateMelee { get; private set; }
    public AbilityStateMelee AbilityStateMelee { get; private set; }


    private EnemyVisuals _enemyVisuals;
    
    [Header("Enemy settings")] 
    public EnemyMeleeType MeleeType;
    public Transform shieldTransform;
    public float dodgeCooldown;
    private float lastTimeDodged = -10;


    [Header("Attack data")] 
    public AttackData AttackData;

    [Header("Axe throw ability")] 
    public GameObject axePrefab;
    public float axeFlySpeed;
    [FormerlySerializedAs("aimTimer")] public float axeAimTimer;
    public float axeThrowCooldown;
    public Transform axeStartPoint;
    private float _lastTimeAxeThrown;

    public List<AttackData> attackList;

    protected override void Awake()
    {
        base.Awake();

        _enemyVisuals = GetComponent<EnemyVisuals>();
        
        IdleStateMelee = new IdleStateMelee(this, StateMachine, "Idle");
        MoveStateMelee = new MoveStateMelee(this, StateMachine, "Move");
        RecoveryStateMelee = new RecoveryStateMelee(this, StateMachine, "Recovery");
        ChaseStateMelee = new ChaseStateMelee(this, StateMachine, "Chase");
        AttackStateMelee = new AttackStateMelee(this, StateMachine, "Attack");
        DeadStateMelee = new DeadStateMelee(this, StateMachine, "Idle");
        AbilityStateMelee = new AbilityStateMelee(this, StateMachine, "AxeThrow");
    }

    protected override void Start()
    {
        base.Start();
        
        StateMachine.Initialize(IdleStateMelee);
        InitializeSpeciality();
        _enemyVisuals.SetupRandomLook();
    }

    protected override void Update()
    {
        base.Update();
        
        StateMachine.currentState.Update();
        
        if(ShouldEnterBattleMode())
            EnterBattleMode();
    }

    public override void EnterBattleMode()
    {
        if(inBattleMode)
            return;
        
        base.EnterBattleMode();
        StateMachine.ChangeState(RecoveryStateMelee);
    }

    public override void GetHit()
    {
        base.GetHit();
        if(healthPoints <= 0)
            StateMachine.ChangeState(DeadStateMelee);
    }

    private void InitializeSpeciality()
    {
        if (MeleeType == EnemyMeleeType.AxeThrow)
        {
            _enemyVisuals.SetupWeaponType(EnemyMeleeWeaponType.Throw);
        }
        
        if (MeleeType == EnemyMeleeType.Shield)
        {
            Anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
            _enemyVisuals.SetupWeaponType(EnemyMeleeWeaponType.OneHand);
        }
    }

    public void EnableWeaponModel(bool isActive)
    {
        _enemyVisuals.CurrentWeaponModel.gameObject.SetActive(isActive);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackData.attackRange);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        moveSpeed = moveSpeed * 0.6f;
        Debug.Log("siekiera");
        EnableWeaponModel(false);
    }

    public void ActivateDodgeRoll()
    {
        if(MeleeType != EnemyMeleeType.Dodge)
            return;
        if(StateMachine.currentState != ChaseStateMelee)
            return;
        if(Vector3.Distance(transform.position, Player.position) < 2f)
            return;

        float dodgeAnimationDuration = GetAnimationClipDuration("Roll");
        
        if (Time.time > dodgeCooldown + dodgeAnimationDuration + lastTimeDodged)
        {
            lastTimeDodged = Time.time;
            Anim.SetTrigger("DodgeRoll");
        }
            
    }

    private float GetAnimationClipDuration(string clipName)
    {
        AnimationClip[] clips = Anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        Debug.Log(  clipName + "nie ma takiej animacji");
        return 0f;
    }

    public bool CanThrowAxe()
    {
        if (MeleeType != EnemyMeleeType.AxeThrow)
            return false;
        
        if (Time.time > _lastTimeAxeThrown + axeThrowCooldown)
        {
            _lastTimeAxeThrown = Time.time;
            return true;
        }

        return false;
    }
    
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, Player.position) < AttackData.attackRange;
}
