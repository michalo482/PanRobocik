using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct AttackDataEnemyMelee
{
    public string attackName;
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;
    [Range(1, 2)]
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
    [Header("Dynamic Stats")]
    [SerializeField] private EnemyMeleeDynamicStats meleeDynamicStats; 
    [SerializeField] private EnemyMeleeDynamicStats2 meleeDynamicStats2;

    public IdleStateMelee IdleStateMelee { get; private set; }
    public MoveStateMelee MoveStateMelee { get; private set; }
    public RecoveryStateMelee RecoveryStateMelee { get; private set; }
    public ChaseStateMelee ChaseStateMelee { get; private set; }
    public AttackStateMelee AttackStateMelee { get; private set; }
    public DeadStateMelee DeadStateMelee { get; private set; }
    public AbilityStateMelee AbilityStateMelee { get; private set; }

    [Header("Enemy settings")] 
    public EnemyMeleeType MeleeType;
    public EnemyMeleeWeaponType weaponType;
    public Transform shieldTransform;
    public float dodgeCooldown;
    private float lastTimeDodged = -10;

    [FormerlySerializedAs("enemyMeleeAttackData")] [FormerlySerializedAs("meleeAttackData")] [FormerlySerializedAs("AttackData")]
    [Header("Attack data")] 
    public AttackDataEnemyMelee attackDataEnemyMelee;

    [Header("Axe throw ability")] 
    public GameObject axePrefab;
    public float axeFlySpeed;
    public float axeAimTimer;
    private float axeThrowCooldown;
    public Transform axeStartPoint;
    private float _lastTimeAxeThrown;

    public List<AttackDataEnemyMelee> attackList;

    protected override void Awake()
    {
        base.Awake();

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
    InitializePerk();
    EnemyVisuals.SetupRandomLook();
    
    if (meleeDynamicStats != null)
    {
        axeThrowCooldown = meleeDynamicStats.axeCooldown;
    }
    else
    {
        axeThrowCooldown = 5f; // domyœlna wartoœæ, jeœli meleeDynamicStats nie jest przypisany
    }

    UpdateAttackData();
}

    protected override void Update()
    {
        base.Update();
        
        StateMachine.currentState.Update();
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;
        
        base.EnterBattleMode();
        StateMachine.ChangeState(RecoveryStateMelee);
    }

    public void UpdateAttackData()
    {
        EnemyWeaponModel currentWeapon = EnemyVisuals.CurrentWeaponModel.GetComponent<EnemyWeaponModel>();

        if (currentWeapon.weaponData != null)
        {
            attackList = new List<AttackDataEnemyMelee>(currentWeapon.weaponData.attackData);
            turnSpeed = currentWeapon.weaponData.turnSpeed;
        }
    }

    public override void GetHit()
    {
        base.GetHit();

        if (healthPoints <= 0 && StateMachine.currentState != DeadStateMelee)
            StateMachine.ChangeState(DeadStateMelee);
    }

    private void InitializePerk()
    {
        if (MeleeType == EnemyMeleeType.AxeThrow)
        {
            weaponType = EnemyMeleeWeaponType.Throw;
        }
        
        if (MeleeType == EnemyMeleeType.Shield)
        {
            Anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
            weaponType = EnemyMeleeWeaponType.OneHand;
        }

        if (MeleeType == EnemyMeleeType.Dodge)
        {
            weaponType = EnemyMeleeWeaponType.Unarmed;
        }
    }

       protected override void OnDrawGizmos()
    {
           base.OnDrawGizmos();
    
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, attackDataEnemyMelee.attackRange);

    if (meleeDynamicStats2 != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeDynamicStats2.aggression);
    }
       
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        walkSpeed = walkSpeed * 0.6f;
        Debug.Log("siekiera");
        EnemyVisuals.EnableWeaponModel(false);
    }

    public void ActivateDodgeRoll()
    {
        if (MeleeType != EnemyMeleeType.Dodge)
            return;
        if (StateMachine.currentState != ChaseStateMelee)
            return;
        if (Vector3.Distance(transform.position, Player.position) < 2f)
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
        Debug.Log(clipName + "nie ma takiej animacji");
        return 0f;
    }

    public void ThrowAxe()
    {
        GameObject newAxe = ObjectPool.Instance.GetObject(axePrefab, axeStartPoint);
        newAxe.GetComponent<EnemyAxe>().AxeSetup(axeFlySpeed, Player, axeAimTimer);
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
    
 public bool PlayerInAttackRange() => Vector3.Distance(transform.position, Player.position) < attackDataEnemyMelee.attackRange;

public bool PlayerInAggressionRange()
{
    if (meleeDynamicStats2 != null)
    {
        return Vector3.Distance(transform.position, Player.position) < meleeDynamicStats2.aggression;
    }
    
    return false; // lub domyœlna logika, jeœli brak przypisania
}}