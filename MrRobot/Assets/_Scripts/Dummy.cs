using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamagable
{
    public int currentHealth;
    public int maxHealth = 100;

    public MeshRenderer mesh;
    public Material whiteMat;
    public Material redMat;

    public float refreshCooldown;
    public float lastTimeDamaged;


    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        if(Time.time > refreshCooldown + lastTimeDamaged || Input.GetKeyDown(KeyCode.K))
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        currentHealth = maxHealth;
        mesh.sharedMaterial = whiteMat;
    }

    public void TakeDamage(int damage)
    {
        lastTimeDamaged = Time.time;
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        mesh.sharedMaterial = redMat;
    }
}
