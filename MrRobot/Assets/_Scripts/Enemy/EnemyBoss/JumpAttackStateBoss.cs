using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackStateBoss : EnemyState
{

    private EnemyBoss _enemyBoss;
    private Vector3 lastPlayerPosition;

    private float jumpAttackMovementSpeed;
    public JumpAttackStateBoss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
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
        lastPlayerPosition = _enemyBoss.Player.position;


        _enemyBoss.Agent.isStopped = true;
        _enemyBoss.Agent.velocity = Vector3.zero;

        _enemyBoss.bossVisuals.PlaceLandingZone(lastPlayerPosition);
        _enemyBoss.bossVisuals.EnableWeaponTrail(true);

        float distanceToPlayer = Vector3.Distance(lastPlayerPosition, _enemyBoss.transform.position); 

        jumpAttackMovementSpeed = distanceToPlayer / _enemyBoss.travelTimeToTarget;

        _enemyBoss.FaceTarget(lastPlayerPosition, 1000);

        if(_enemyBoss.bossWeaponType == BossWeaponType.Hammer)
        {
            _enemyBoss.Agent.isStopped = false;
            _enemyBoss.Agent.speed = _enemyBoss.runSpeed;
            _enemyBoss.Agent.SetDestination(lastPlayerPosition);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _enemyBoss.SetJumpAttackOnCooldown();
        _enemyBoss.bossVisuals.EnableWeaponTrail(false);
    }

    public override void Update()
    {
        base.Update();

        Vector3 myPos = _enemyBoss.transform.position;
        _enemyBoss.Agent.enabled = !_enemyBoss.ManualMovementActive();

        if(_enemyBoss.ManualMovementActive())
        {
            _enemyBoss.transform.position = Vector3.MoveTowards(myPos, lastPlayerPosition, jumpAttackMovementSpeed * Time.deltaTime);
        }


        if(triggerCalled)
        {
            stateMachine.ChangeState(_enemyBoss.MoveStateBoss);
        }
    }
}
