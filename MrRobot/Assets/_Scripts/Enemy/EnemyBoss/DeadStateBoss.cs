using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateBoss : EnemyState
{

    private EnemyBoss _enemyBoss;
    private bool _interactionDisabled;
    public DeadStateBoss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyBoss = enemyBase as EnemyBoss;
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        MissionObjectHuntTarget huntTarget = _enemyBoss.GetComponent<MissionObjectHuntTarget>();
        huntTarget?.InvokeOnTargetKilled();

        _enemyBoss.AbilityStateBoss.DisableFlamethrower();

        _interactionDisabled = false;

        _enemyBoss.Anim.enabled = false;
        _enemyBoss.Agent.isStopped = true;

        _enemyBoss.Ragdoll.RagdollActive(true);

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
            _enemyBoss.Ragdoll.RagdollActive(false);
            _enemyBoss.Ragdoll.CollidersActive(false);
        }
    }
}
