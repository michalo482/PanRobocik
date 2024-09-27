using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMelee : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 attackDirection;
    private float attackMoveSpeed;
    private const float maxAttackDistance = 50f;
    public AttackStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnableWeaponModel(true);

        attackMoveSpeed = enemy.AttackData.moveSpeed;
        enemy.Anim.SetFloat("AttackAnimationSpeed", enemy.AttackData.animationSpeed);
        enemy.Anim.SetFloat("AttackIndex", enemy.AttackData.attackIndex);
        enemy.Anim.SetFloat("SlashAttackIndex", Random.Range(0, 5));
        
        
        enemy.Agent.isStopped = true;
        enemy.Agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * maxAttackDistance);
    }

    public override void Update()
    {
        base.Update();
        if (enemy.ManualRotationActive())
        {
            enemy.FaceTarget(enemy.Player.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * maxAttackDistance);
        }
        
        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection,
            attackMoveSpeed * Time.deltaTime);
            
        }
        
        if(triggerCalled)
            stateMachine.ChangeState(enemy.RecoveryStateMelee);
    }

    public override void Exit()
    {
        base.Exit();

        SetupNextAttack();
    }

    private void SetupNextAttack()
    {
        int recoveryIndex = PlayerClose() ? 1 : 0;
        enemy.Anim.SetFloat("RecoveryIndex", recoveryIndex);

        enemy.AttackData = UpdatedAttackData();
    }

    private bool PlayerClose() => Vector3.Distance(enemy.transform.position, enemy.Player.position) <= 2;

    private AttackData UpdatedAttackData()
    {
        List<AttackData> validAttacks = new List<AttackData>(enemy.attackList);

        if (PlayerClose())
        {
            validAttacks.RemoveAll(parameter => parameter.AttackTypeMelee == AttackTypeMelee.Charge);
        }
        int random = Random.Range(0, validAttacks.Count);

        return validAttacks[random];
    }
}
