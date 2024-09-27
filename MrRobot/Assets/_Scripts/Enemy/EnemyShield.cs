using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private EnemyMelee enemy;
    [SerializeField] private int durability;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyMelee>();
    }

    public void ReduceDurability()
    {
        durability--;
        if (durability <= 0)
        {
            //enemy.Anim.SetFloat("ChaseIndex", 0);
            Destroy(gameObject);
        }
    }
}
