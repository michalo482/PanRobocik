using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateBoss : EnemyState
{

    private EnemyBoss _enemyBoss;
    public float lastTimeAttacked { get; private set; }
    public AttackStateBoss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyBoss = enemyBase as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();

        _enemyBoss.Anim.SetFloat("AttackAnimIndex", Random.Range(0, 2));

        _enemyBoss.Agent.isStopped = true;
        _enemyBoss.bossVisuals.EnableWeaponTrail(true);

        stateTimer = 1f;

    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time;
        _enemyBoss.bossVisuals.EnableWeaponTrail(false);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            _enemyBoss.FaceTarget(_enemyBoss.Player.position, 20);
        }

        if(triggerCalled)
        {
            if(_enemyBoss.PlayerInAttackRange())
            {
                stateMachine.ChangeState(_enemyBoss.IdleStateBoss);
            }
            else
            {
                stateMachine.ChangeState(_enemyBoss.MoveStateBoss);

            }
        }
    }
}
