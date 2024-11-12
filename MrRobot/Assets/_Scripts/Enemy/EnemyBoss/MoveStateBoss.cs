using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateBoss : EnemyState
{
    private EnemyBoss _enemyBoss;
    private Vector3 destination;
    private float actionTimer;
    private float timeBeforSpeedUp = 7;

    private bool speedUpActivated;
    public MoveStateBoss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyBoss = enemyBase as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();
        SpeedReset();
        _enemyBoss.Agent.isStopped = false;

        destination = _enemyBoss.GetPatrolDestination();
        _enemyBoss.Agent.SetDestination(destination);

        actionTimer = _enemyBoss.actionCooldown;
    }

    private void SpeedReset()
    {
        speedUpActivated = false;
        _enemyBoss.Anim.SetFloat("MoveAnimIndex", 0);
        _enemyBoss.Anim.SetFloat("MoveAnimSpeedMulti", 1);

        _enemyBoss.Agent.speed = _enemyBoss.walkSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        actionTimer -= Time.deltaTime;

        _enemyBoss.FaceTarget(GetNextPathPoint());

        if(_enemyBoss.inBattleMode)
        {
            if(ShouldSpeedUp())
            {
                SpeedUp();
            }

            Vector3 playerPos = _enemyBoss.Player.position;

            _enemyBoss.Agent.SetDestination(playerPos);

            if(actionTimer < 0)
            {
                PerformRandomAction();
            }

            
            else if(_enemyBoss.PlayerInAttackRange())
            {
                stateMachine.ChangeState(_enemyBoss.AttackStateBoss);
            }
        }
        else
        {
            if (Vector3.Distance(_enemyBoss.transform.position, destination) < 0.25f)
                stateMachine.ChangeState(_enemyBoss.IdleStateBoss);
        }      
    }

    private void SpeedUp()
    {
        _enemyBoss.Agent.speed = _enemyBoss.runSpeed;
        _enemyBoss.Anim.SetFloat("MoveAnimIndex", 1);
        _enemyBoss.Anim.SetFloat("MoveAnimSpeedMulti", 1.5f);
        speedUpActivated = true;
    }

    private void PerformRandomAction()
    {
        actionTimer = _enemyBoss.actionCooldown;

        if(Random.Range(0,2) == 0)
        {
            TryAbility();
        }
        else
        {
            if(_enemyBoss.CanDoJumpAttack())
            {
                stateMachine.ChangeState(_enemyBoss.JumpAttackStateBoss);
            }
            else if(_enemyBoss.bossWeaponType == BossWeaponType.Hammer)
            {
                TryAbility();
            }
        }
    }

    private void TryAbility()
    {
        if (_enemyBoss.CanDoAbility())
        {
            stateMachine.ChangeState(_enemyBoss.AbilityStateBoss);
        }
    }

    private bool ShouldSpeedUp()
    {

        if(speedUpActivated)
        {
            return false;
        }
        if(Time.time > _enemyBoss.AttackStateBoss.lastTimeAttacked + timeBeforSpeedUp)
        {
            return true;
        }
        return false;
    }
}
