using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateMelee : EnemyState
{

    private EnemyMelee enemy;
    private EnemyRagdoll ragdoll;
    private bool _interactionDisabled;
    public DeadStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        ragdoll = enemy.GetComponent<EnemyRagdoll>();
    }

    public override void Enter()
    {
        base.Enter();

        _interactionDisabled = false;

        enemy.Anim.enabled = false;
        enemy.Agent.isStopped = true;
        
        ragdoll.RagdollActive(true);

        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTimer <= 0 && _interactionDisabled == false)
        {
            _interactionDisabled = true;
            ragdoll.RagdollActive(false);
            ragdoll.CollidersActive(false);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
