using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStateMelee : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 movementDirection;

    private const float maxMovementDistance = 20;
    private float moveSpeed;
    public AbilityStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyVisuals.EnableWeaponModel(true);
        
        moveSpeed = enemy.walkSpeed;
        movementDirection = enemy.transform.position + (enemy.transform.forward * maxMovementDistance);
    }

    public override void Update()
    {
        base.Update();
        if (enemy.ManualRotationActive())
        {
            enemy.FaceTarget(enemy.Player.position);
            movementDirection = enemy.transform.position + (enemy.transform.forward * maxMovementDistance);
        }
        
        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, movementDirection,
                enemy.walkSpeed * Time.deltaTime);
            
        }
        
        if(triggerCalled)
            stateMachine.ChangeState(enemy.RecoveryStateMelee);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.walkSpeed = moveSpeed;
        enemy.Anim.SetFloat("RecoveryIndex", 0);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        GameObject newAxe = ObjectPool.Instance.GetObject(enemy.axePrefab);
        newAxe.transform.position = enemy.axeStartPoint.position;
        
        newAxe.GetComponent<EnemyAxe>().AxeSetup(enemy.axeFlySpeed, enemy.Player, enemy.axeAimTimer);
    }
}
