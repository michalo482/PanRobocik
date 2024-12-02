using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShield : MonoBehaviour, IDamagable
{
    private EnemyMelee enemy;
    [SerializeField] private int durability;

    [SerializeField] private AudioSource shieldAudioSource; // Bezpoœrednie odwo³anie do AudioSource

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

            // Odtwarzanie dŸwiêku z przypisanego AudioSource
            if (shieldAudioSource != null)
            {
                shieldAudioSource.Play();
            }

            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        ReduceDurability(damage);
    }
}

