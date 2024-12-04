using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryStateMelee : EnemyState
{

    private EnemyMelee enemy;
    public RecoveryStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Agent.isStopped = true;
    }


    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(enemy.Player.position);

        if (triggerCalled)
        {
            if (enemy.CanThrowAxe())
                stateMachine.ChangeState(enemy.AbilityStateMelee);
            else if(enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.AttackStateMelee);
            else
                stateMachine.ChangeState(enemy.ChaseStateMelee);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
