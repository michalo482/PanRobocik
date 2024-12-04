using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePlayerStateRange : EnemyState
{
    private EnemyRange _enemyRange;
    private Vector3 playerPos;

    public float LastTimeAdvanced { get; private set; }

    public AdvancePlayerStateRange(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyRange = enemyBase as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();

        _enemyRange.EnemyVisuals.EnableIK(true, false);

        _enemyRange.Agent.isStopped = false;
        _enemyRange.Agent.speed = _enemyRange.advanceSpeed;
    }

    public override void Exit()
    {
        base.Exit();

        LastTimeAdvanced = Time.time;
    }

    public override void Update()
    {
        base.Update();

        playerPos = _enemyRange.Player.transform.position;

        _enemyRange.UpdateAimPosition();

        _enemyRange.Agent.SetDestination(playerPos);
        _enemyRange.FaceTarget(GetNextPathPoint());

        if(CanEnterBattleState())
        {
            stateMachine.ChangeState(_enemyRange.BattleStateRange);
        }
    }

    private bool CanEnterBattleState()
    {
        return Vector3.Distance(_enemyRange.transform.position, playerPos) < _enemyRange.advanceStoppingDistance && _enemyRange.IsSeeingPlayer();
    }
}
