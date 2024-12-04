using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStateBoss : EnemyState
{
    private EnemyBoss _enemyBoss;
    public AbilityStateBoss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyBoss = enemyBase as EnemyBoss;
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        if(_enemyBoss.bossWeaponType == BossWeaponType.Flamethrower)
        {
            _enemyBoss.ActivateFlamethrower(true);
            _enemyBoss.bossVisuals.DischargeBatteries();
        }
        if(_enemyBoss.bossWeaponType == BossWeaponType.Hammer)
        {
            _enemyBoss.ActivateHammer();
        }
    }

    public override void Enter()
    {
        base.Enter();

        _enemyBoss.Agent.isStopped = true;
        _enemyBoss.Agent.velocity = Vector3.zero;
        stateTimer = _enemyBoss.flamethrowDuration;
        _enemyBoss.bossVisuals.EnableWeaponTrail(true);

    }

    public override void Exit()
    {
        base.Exit();

        _enemyBoss.SetAbilityOnCooldown();
        _enemyBoss.bossVisuals.ResetBatteries();
        _enemyBoss.bossVisuals.EnableWeaponTrail(false);
    }

    public override void Update()
    {
        base.Update();

        _enemyBoss.FaceTarget(_enemyBoss.Player.position);

        if(stateTimer < 0 && _enemyBoss.bossWeaponType == BossWeaponType.Flamethrower)
        { 
            DisableFlamethrower(); 
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(_enemyBoss.MoveStateBoss);
        }
    }

    public void DisableFlamethrower()
    {
        if(_enemyBoss.flamethrowActive == false)
        {
            return;
        }

        _enemyBoss.ActivateFlamethrower(false);
    }
}
