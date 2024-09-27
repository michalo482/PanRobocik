using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStateMelee : EnemyState
{
    private EnemyMelee enemy;
    private float lastTimeUpdatedDestination;
    public ChaseStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        
    }

    public override void Enter()
    {
        CheckChaseAnimation();
        base.Enter();
        enemy.Agent.speed = enemy.chaseSpeed;
        enemy.Agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();
        
        if(enemy.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.AttackStateMelee);
        
        enemy.FaceTarget(GetNextPathPoint());
        
        if (CanUpdateDestination())
        {
            enemy.Agent.destination = enemy.Player.position;
            
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanUpdateDestination()
    {
        if (Time.time > lastTimeUpdatedDestination + .25f)
        {
            lastTimeUpdatedDestination = Time.time;
            return true;
        }

        return false;
    }

    private void CheckChaseAnimation()
    {
        if (enemy.MeleeType == EnemyMeleeType.Shield && enemy.shieldTransform == null)
        {
            enemy.Anim.SetFloat("ChaseIndex", 0);
        }
            
    }
}
