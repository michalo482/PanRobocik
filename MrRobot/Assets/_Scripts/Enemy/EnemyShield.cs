using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour, IDamagable
{
    private EnemyMelee enemy;
    [SerializeField] private int durability;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyMelee>();
        durability = enemy.shieldDurability;
    }

    public void ReduceDurability(int damage)
    {
        durability -= damage;
        if (durability <= 0)
        {
            enemy.Anim.SetFloat("ChaseIndex", 0);
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        ReduceDurability(damage);
    }
}
