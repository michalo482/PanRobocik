using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public AudioSource damageAudioSource;
    public AudioSource healAudioSource;
    public AudioSource healAudioSource2;
    public AudioSource deathAudioSource;
    public AudioSource lowHealthAudioSource;

    private bool lowHealthPlayed = false;
    private bool isDead;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void ReduceHealth(int damage)
    {
        currentHealth -= damage;

        if (damageAudioSource != null)
        {
            damageAudioSource.Play();
        }

        CheckLowHealth();
    }

    public virtual void IncreaseHealth()
    {
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);

        if (healAudioSource && healAudioSource2 != null)
        {
            healAudioSource.Play();
            healAudioSource2.Play();

        }

        if (currentHealth > maxHealth * 0.33f)
        {
            lowHealthPlayed = false; 
        }
    }


    public bool ShouldDie() 
    {
        if (currentHealth <= 0)
        {
            if (deathAudioSource != null && !deathAudioSource.isPlaying)
            {
                deathAudioSource.Play();
            }
            return true;
        }
        return false;
    }

    private void CheckLowHealth()
    {
        if (lowHealthAudioSource == null)
            return;

      
        if (currentHealth <= 0)
        {
            if (lowHealthAudioSource.isPlaying)
                lowHealthAudioSource.Stop();

            lowHealthAudioSource.pitch = 1.0f;
            return;
        }

        float healthPercentage = (float)currentHealth / maxHealth * 100f;


        if (healthPercentage <= 33f && !lowHealthPlayed)
        {
            lowHealthAudioSource.Play();
            lowHealthPlayed = true;
        }

        if (healthPercentage <= 10f)
            lowHealthAudioSource.pitch = 1.3f;
        else if (healthPercentage <= 19f)
            lowHealthAudioSource.pitch = 1.2f;
        else if (healthPercentage <= 30f)
            lowHealthAudioSource.pitch = 1.0f;
    }

    public bool ShouldDie()
    {
        if (isDead)
            return false;

        if(currentHealth <= 0)
        {
            isDead = true;
            return true;
        }

        return false;
    }

}
