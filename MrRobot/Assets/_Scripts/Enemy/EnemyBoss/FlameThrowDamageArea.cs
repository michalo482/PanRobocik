using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowDamageArea : MonoBehaviour
{
    private EnemyBoss enemyBoss;
    private float damageCooldown;
    private float lastTimeDamaged;
    private int flameDamage;

    private void Awake()
    {
        enemyBoss = GetComponentInParent<EnemyBoss>();
        damageCooldown = enemyBoss.flameDamageCooldown;
        flameDamage = enemyBoss.flameDamage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemyBoss.flamethrowActive == false)
        {
            return;
        }


        if(Time.time - lastTimeDamaged < damageCooldown) { return; }
        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.TakeDamage(flameDamage);
            lastTimeDamaged = Time.time;    
        }
    }
}
