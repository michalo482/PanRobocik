using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    private bool isDead;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void ReduceHealth(int damage)
    {
        if (damage < 0 && currentHealth >= maxHealth)
        {
            AudioManager.Instance.PlayHealthSound("heal2");
            return;
        }
        currentHealth -= damage;

        AudioManager.Instance.PlayHealthSound("damage");

    }

    public virtual void IncreaseHealth()
    {
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        AudioManager.Instance.PlayHealthSound("heal");

    }

    public bool ShouldDie()
    {
        if (isDead)
            return false;

        if (currentHealth <= 0) 
        {
            isDead = true;
            AudioManager.Instance.PlayHealthSound("death");
            return true;
        }

        return false;
    }

}
