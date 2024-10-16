using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateRange : EnemyState
{

    private EnemyRange _enemyRange;
    private Vector3 destination;
    public MoveStateRange(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyRange = enemyBase as EnemyRange;
    }


    public override void Enter()
    {
        base.Enter();
        _enemyRange.Agent.speed = _enemyRange.walkSpeed;
        
        destination = _enemyRange.GetPatrolDestination();
        _enemyRange.Agent.SetDestination(destination);
    }

    public override void Update()
    {
        base.Update();
        _enemyRange.FaceTarget(GetNextPathPoint());
        
        
       if(_enemyRange.Agent.remainingDistance <= _enemyRange.Agent.stoppingDistance + .05f)
           stateMachine.ChangeState(_enemyRange.IdleStateRange);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
