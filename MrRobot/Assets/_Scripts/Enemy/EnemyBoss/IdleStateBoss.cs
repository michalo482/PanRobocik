using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateBoss : EnemyState
{

    private EnemyBoss _enemyBoss;
    public IdleStateBoss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyBoss = enemyBase as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = _enemyBoss.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(_enemyBoss.inBattleMode && _enemyBoss.PlayerInAttackRange())
        {
            stateMachine.ChangeState(_enemyBoss.AttackStateBoss);
        }


        if(stateTimer < 0)
        {
            stateMachine.ChangeState(_enemyBoss.MoveStateBoss);
        }
        
    }
}
