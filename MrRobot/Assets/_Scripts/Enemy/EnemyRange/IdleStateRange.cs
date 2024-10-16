using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateRange : EnemyState
{
    private EnemyRange _enemyRange;

    public IdleStateRange(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyRange = enemyBase as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = _enemyRange.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(_enemyRange.MoveStateRange);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    
}
