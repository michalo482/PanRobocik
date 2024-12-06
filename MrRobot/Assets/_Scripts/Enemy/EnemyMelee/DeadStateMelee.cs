using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateMelee : EnemyState
{

    private EnemyMelee enemy;
    //private EnemyRagdoll ragdoll;
    private bool _interactionDisabled;
    public DeadStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        //ragdoll = enemy.GetComponent<EnemyRagdoll>();
    }

    public override void Enter()
    {
        base.Enter();
        MissionObjectHuntTarget huntTarget = enemy.GetComponent<MissionObjectHuntTarget>();
        huntTarget?.InvokeOnTargetKilled();

        _interactionDisabled = false;

        enemy.Anim.enabled = false;
        enemy.Agent.isStopped = true;
        
        enemy.Ragdoll.RagdollActive(true);

        stateTimer = 1.5f;


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
            enemy.Ragdoll.RagdollActive(false);
            enemy.Ragdoll.CollidersActive(false);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
