using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateMelee : EnemyState
{

    private EnemyMelee enemy;
    public IdleStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemyBase.idleTime;

    }

    public override void Update()
    {
        base.Update();
        
        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveStateMelee);
        
    }

    public override void Exit()
    {
        base.Exit();
                  
    }
}
