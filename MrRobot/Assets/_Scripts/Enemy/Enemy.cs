using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 20;
    
    public float turnSpeed;
    public float aggressionRange;
    public Transform Player { get; private set; }
    
    [Header("Idle data")] 
    public float idleTime;

    [Header("Move data")] 
    public float moveSpeed;
    public float chaseSpeed;
    private bool _manualMovement;
    private bool _manualRotation;

    [SerializeField] private Transform[] patrolPoints;
    private int _currentPatrolIndex;
    private Vector3[] _patrolPointsPosition;
    
    
    public bool inBattleMode { get; private set; }

    public Animator Anim { get; private set; }

    public NavMeshAgent Agent { get; private set; }
    
    public EnemyStateMachine StateMachine { get; private set; }

    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine();
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponentInChildren<Animator>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
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

    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngels = transform.rotation.eulerAngles;

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
    

    public virtual void GetHit()
    {
        EnterBattleMode();
        healthPoints--;
    }

    public virtual void DeathImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        StartCoroutine(DeathImpactCoroutine(force, hitPoint, rb));
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

    protected bool ShouldEnterBattleMode()
    {
        bool inAggressionRange = Vector3.Distance(transform.position, Player.position) < aggressionRange;

        if (inAggressionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }

        return false;
    }
}
