using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : HitBox
{
    private Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }
    public override void TakeDamage(int damage)
    {
        //base.TakeDamage();

        enemy.GetHit(damage);
    }
}
