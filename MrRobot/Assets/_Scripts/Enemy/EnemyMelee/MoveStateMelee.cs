using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveStateMelee : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 destination;
    public MoveStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.Agent.speed = enemy.moveSpeed;
        
        destination = enemy.GetPatrolDestination();
        enemy.Agent.SetDestination(destination);
        Debug.Log("i enter move state");
    }

    public override void Update()
    {
        base.Update();

        Debug.Log("i move");
        enemy.FaceTarget(GetNextPathPoint());
        
        
        if(enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance + .05f)
            stateMachine.ChangeState(enemy.IdleStateMelee);
    }

    public override void Exit()
    {
        base.Exit();
        
        Debug.Log("i exit move");
    }

    
}
