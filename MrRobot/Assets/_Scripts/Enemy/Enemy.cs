using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum EnemyType
{
    MELEE,
    RANGE,
    BOSS,
    RANDOM
}

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public LayerMask whatIsAlly;
    public LayerMask whatIsPlayer;
    //public int healthPoints = 20;
    
    public float aggressionRange;
    public Transform Player { get; private set; }
    
    [Header("Idle data")] 
    public float idleTime;

    [Header("Move data")] 
    public float walkSpeed = 1.5f;
    public float runSpeed = 4;
    public float turnSpeed;
    private bool _manualMovement;
    private bool _manualRotation;

    [SerializeField] private Transform[] patrolPoints;
    private int _currentPatrolIndex;
    private Vector3[] _patrolPointsPosition;

    protected bool isMeleeAttackReady;

    public EnemyDropController DropController {  get; private set; }
    
    
    public bool inBattleMode { get; private set; }

    public Animator Anim { get; private set; }

    public NavMeshAgent Agent { get; private set; }
    
    public EnemyStateMachine StateMachine { get; private set; }

    public EnemyVisuals EnemyVisuals { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }
    public Ragdoll Ragdoll { get; private set; }

    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine();

        EnemyHealth = GetComponent<EnemyHealth>();
        Ragdoll = GetComponent<Ragdoll>();
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponentInChildren<Animator>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        EnemyVisuals = GetComponent<EnemyVisuals>();
        DropController = GetComponent<EnemyDropController>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
        
    }

    private void InitializePatrolPoints()
    {
        _patrolPointsPosition = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            _patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        if(ShouldEnterBattleMode())
            EnterBattleMode();
    }

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = _patrolPointsPosition[_currentPatrolIndex];
        Debug.Log(destination);
        _currentPatrolIndex++;
        if (_currentPatrolIndex >= patrolPoints.Length)
            _currentPatrolIndex = 0;
        return destination;
    }

    public void FaceTarget(Vector3 target, float turnSpeed = 0)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngels = transform.rotation.eulerAngles;

        if(turnSpeed == 0)
        {
            turnSpeed = this.turnSpeed;
        }

        float yRotation = Mathf.LerpAngle(currentEulerAngels.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(currentEulerAngels.x, yRotation, currentEulerAngels.z);
    }

    public void AnimationTrigger()
    {
        StateMachine.currentState.AnimationTrigger();
    }
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggressionRange);
        
    }

    public void ActivateManualMovement(bool manualMovement) => _manualMovement = manualMovement;
    public bool ManualMovementActive() => _manualMovement;

    public bool ActivateManualRotation(bool manualRotation) => _manualRotation = manualRotation;
    public bool ManualRotationActive() => _manualRotation;
    

    public virtual void GetHit(int damage)
    {
        EnemyHealth.ReduceHealth(damage);
        if(EnemyHealth.ShouldDie())
        {
            DropController.DropItem();
            Die();
        }

        EnterBattleMode();
    }

    public virtual void Die()
    {
        MissionObjectHuntTarget huntTarget = GetComponent<MissionObjectHuntTarget>();
        huntTarget?.InvokeOnTargetKilled();
    }

    public virtual void AttackCheck(Transform[] damagePoints, float attackRadius, GameObject meleeAttackFx, int damage)
    {
        if (isMeleeAttackReady == false)
        {
            return;
        }

        foreach (Transform attackPoint in damagePoints)
        {
            Collider[] detectedHits = Physics.OverlapSphere(attackPoint.position, attackRadius, whatIsPlayer);
            for (int i = 0; i < detectedHits.Length; i++)
            {
                IDamagable damagable = detectedHits[i].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                    isMeleeAttackReady = false;
                    GameObject newAttackFx = ObjectPool.Instance.GetObject(meleeAttackFx, attackPoint);

                    ObjectPool.Instance.ReturnObject(newAttackFx, 1);
                    return;
                }
            }
        }
    }

    public void EnableAttackCheck(bool enable)
    {
        isMeleeAttackReady = enable;
    }


    public virtual void BulletImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        if (EnemyHealth.ShouldDie())
        {
            StartCoroutine(DeathImpactCoroutine(force, hitPoint, rb));
        }
    }

    private IEnumerator DeathImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(.1f);
        
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public virtual void AbilityTrigger()
    {
        StateMachine.currentState.AbilityTrigger();
    }

    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }

    public bool IsPlayerInAgressionRange()
    {
        return Vector3.Distance(transform.position, Player.position) < aggressionRange;
    }

    protected bool ShouldEnterBattleMode()
    {
        //bool inAggressionRange = Vector3.Distance(transform.position, Player.position) < aggressionRange;

        if (IsPlayerInAgressionRange() && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }

        return false;
    }

    public virtual void MakeEnemyVIP()
    {
        int additionalHealth = Mathf.RoundToInt(EnemyHealth.currentHealth * 1.5f);

        EnemyHealth.currentHealth += additionalHealth;

        transform.localScale = transform.localScale * 1.25f;
    }
}
