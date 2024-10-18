using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateRange : EnemyState
{
    private bool _interactionDisabled;

    private EnemyRange _enemyRange;
    public DeadStateRange(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyRange = enemyBase as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();

        if(_enemyRange.ThrowGrenadeStateRange.finishedThrowingGrenade == false)
        {
            _enemyRange.ThrowGrenade();
        }

        _interactionDisabled = false;

        _enemyRange.Anim.enabled = false;
        _enemyRange.Agent.isStopped = true;

        _enemyRange.Ragdoll.RagdollActive(true);

        stateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        DisableInteractionIfShould();
    }

    private void DisableInteractionIfShould()
    {
        if (stateTimer <= 0 && _interactionDisabled == false)
        {
            _interactionDisabled = true;
            _enemyRange.Ragdoll.RagdollActive(false);
            _enemyRange.Ragdoll.CollidersActive(false);
        }
    }
}
