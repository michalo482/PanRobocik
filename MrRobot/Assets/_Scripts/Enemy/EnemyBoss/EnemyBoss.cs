using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossWeaponType
{
    Flamethrower,
    Hammer
}
public class EnemyBoss : Enemy
{
    [Header("Boss Details")]
    public BossWeaponType bossWeaponType;
    public float actionCooldown = 10;
    public Transform impactPoint;


    [Header("Ability")]
    public float minAbilityDistance;
    public float abilityCooldown;
    private float lastTimeUsedAbility;

    [Header("Flamethrower")]
    public float flameDamageCooldown;
    public ParticleSystem flamethrower;
    public float flamethrowDuration;
    public int flameDamage;
    public bool flamethrowActive { get; private set; }

    [Header("Hammer")]
    public int hammerActiveDamage;
    public GameObject activationPrefab;
    [SerializeField] private float hammerCheckRadius;



    [Header("Jump Attack")]
    public int jumpAttackDamage;
    public float impactRadius = 2.5f;
    public float impactPower = 5;
    [SerializeField] private float upforceMultiplayer = 10;

    public float jumpAttackCooldown = 10;
    private float lastTimeJumped;
    public float travelTimeToTarget = 1;
    public float minJumpDistanceRange;

    public float attackRange;

    [Header("Attack")]
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private Transform[] damagePoints;
    [SerializeField] private float attackRadius;
    [SerializeField] private GameObject meleeAttackFx;



    public EnemyBossVisuals bossVisuals { get; private set; }

    [SerializeField] private LayerMask whatToIgnore;
    public IdleStateBoss IdleStateBoss {  get; private set; }
    public MoveStateBoss MoveStateBoss { get; private set; }
    public AttackStateBoss AttackStateBoss { get; private set; }
    public JumpAttackStateBoss JumpAttackStateBoss { get; private set; }
    public AbilityStateBoss AbilityStateBoss { get; private set; }
    public DeadStateBoss DeadStateBoss { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        bossVisuals = GetComponent<EnemyBossVisuals>();

        IdleStateBoss = new IdleStateBoss(this, StateMachine, "Idle");
        MoveStateBoss = new MoveStateBoss(this, StateMachine, "Move");
        AttackStateBoss = new AttackStateBoss(this, StateMachine, "Attack");
        JumpAttackStateBoss = new JumpAttackStateBoss(this, StateMachine, "JumpAttack");
        AbilityStateBoss = new AbilityStateBoss(this, StateMachine, "Ability");
        DeadStateBoss = new DeadStateBoss(this, StateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleStateBoss);
    }

    protected override void Update()
    {
        base.Update();

       

        StateMachine.currentState.Update();

        if(ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }

        AttackCheck(damagePoints, attackRadius, meleeAttackFx, meleeAttackDamage);
    }

    public override void EnterBattleMode()
    {

        if(inBattleMode)
        {
            return;
        }

        base.EnterBattleMode();
        StateMachine.ChangeState(MoveStateBoss);

    }

    public void ActivateFlamethrower(bool activate)
    {

        flamethrowActive = activate;

        if(!activate)
        {
            flamethrower.Stop();
            Anim.SetTrigger("StopFlamethrower");
            return;
        }

        var mainModule = flamethrower.main;
        var extraModule = flamethrower.transform.GetChild(0).GetComponent<ParticleSystem>().main;

        mainModule.duration = flamethrowDuration;
        extraModule.duration = flamethrowDuration;


        flamethrower.Clear();
        flamethrower.Play();
    }

    public void ActivateHammer()
    {
        GameObject newActivation = ObjectPool.Instance.GetObject(activationPrefab, impactPoint);

        ObjectPool.Instance.ReturnObject(newActivation, 1);

        MassDamage(damagePoints[0].position, hammerCheckRadius, hammerActiveDamage);
    }

    public bool CanDoAbility()
    {
        bool playerWithinDistance = Vector3.Distance(transform.position, Player.position) <= minAbilityDistance;

        if(playerWithinDistance == false)
        {
            return false;
        }

        if(Time.time > lastTimeUsedAbility + abilityCooldown)
        {
            //lastTimeUsedAbility = Time.time;
            return true;
        }
        return false;
    }

    public void JumpImpact()
    {
        MassDamage(transform.position, impactRadius, jumpAttackDamage);
    }

    private void MassDamage(Vector3 impactPoint, float impactRadius, int damage)
    {
        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(impactPoint, impactRadius, ~whatIsAlly);
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();

            if (damagable != null)
            {
                GameObject rootEntity = hit.transform.root.gameObject;
                if (uniqueEntities.Add(rootEntity) == false)
                {
                    continue;
                }
                damagable.TakeDamage(damage);
            }

            ApplyPhysicalForceTo(impactPoint, impactRadius, hit);
        }
    }

    private void ApplyPhysicalForceTo(Vector3 impactPoint,float impactRadius, Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, impactPoint, impactRadius, upforceMultiplayer, ForceMode.Impulse);
        }
    }

    public override void Die()
    {
        base.Die();
        if (StateMachine.currentState != DeadStateBoss)
            StateMachine.ChangeState(DeadStateBoss);
    }

    public bool CanDoJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if(distanceToPlayer < minJumpDistanceRange)
        {
            return false;
        }

        if(Time.time > lastTimeJumped + jumpAttackCooldown && IsPlayerInClearSight())
        {
            //lastTimeJumped = Time.time;
            return true;
        }
        return false;
    }

    public bool IsPlayerInClearSight()
    {
        Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
        Vector3 playerPos = Player.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - myPos).normalized;

        if(Physics.Raycast(myPos, directionToPlayer,out RaycastHit hit, 100, ~whatToIgnore))
        {
            if(hit.transform.root == Player.root)
            {
                return true;
            }
        }

        return false;
    }

    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, Player.position) < attackRange;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, attackRange);

        if(Player != null)
        {
            Vector3 myPos = transform.position + new Vector3(0, 1.5f, 0);
            Vector3 playerPos = Player.position + Vector3.up;

            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(myPos, playerPos);
        }

        Gizmos.color = Color.yellow;

        if(damagePoints.Length > 0)
        {
            foreach(var damagePoint in damagePoints)
            {
                Gizmos.DrawWireSphere(damagePoint.position, attackRadius);

            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(damagePoints[0].position, hammerCheckRadius);
        }


    }

    public void SetAbilityOnCooldown()
    {
        lastTimeUsedAbility = Time.time;
    }

    public void SetJumpAttackOnCooldown()
    {
        lastTimeJumped = Time.time;
    }

}
