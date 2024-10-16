using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunToCoverStateRange : EnemyState
{
    private EnemyRange _enemyRange;
    private Vector3 destination;

    public float LastTimeTookCover { get; private set; }

    public RunToCoverStateRange(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyRange = enemyBase as EnemyRange;
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        _enemyRange.Agent.isStopped = false;

        _enemyRange.EnemyVisuals.EnableIK(true, false);
        _enemyRange.Agent.speed = _enemyRange.runSpeed;

        destination = _enemyRange.currentCover.transform.position;
        _enemyRange.Agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();

        LastTimeTookCover = Time.time;
    }

    public override void Update()
    {
        base.Update();
        _enemyRange.FaceTarget(GetNextPathPoint());

        if(Vector3.Distance(_enemyRange.transform.position, destination) < 0.5f)
        {
            stateMachine.ChangeState(_enemyRange.BattleStateRange);
        }
    }

}
