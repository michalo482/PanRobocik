using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    //audio
    public AudioClip damageSound;
    public AudioClip healSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
    }

    public virtual void ReduceHealth(int damage)
    {
        currentHealth -= damage;

        if (damageSound != null && audioSource != null)
        {
            audioSource.clip = damageSound;
            audioSource.Play();
        }
    }

    public virtual void IncreaseHealth()
    {
        currentHealth++;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healSound != null && audioSource != null)
        {
            audioSource.clip = healSound;
            audioSource.Play();
        }
    }

    public bool ShouldDie() 
    {
        if (currentHealth <= 0)
        {
            // Odtwarzamy dŸwiêk œmierci, jeœli jest przypisany
            if (deathSound != null && audioSource != null)
            {
                audioSource.clip = deathSound;
                audioSource.Play();
            }
            return true;
        }
        return false;
    }
}
