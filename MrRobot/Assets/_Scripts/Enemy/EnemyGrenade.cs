using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionFX;
    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardsMultiplayer = 1;
    private float impactPower;
    private Rigidbody rb;
    private float timer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject newFX = ObjectPool.Instance.GetObject(explosionFX);
        newFX.transform.position = transform.position;

        ObjectPool.Instance.ReturnObject(newFX, 1);
        ObjectPool.Instance.ReturnObject(gameObject);

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplayer, ForceMode.Impulse);
            }
        }
    }

    public void SetupGrenade(Vector3 target, float timeToTarget, float countdown, float impactPower)
    {
        rb.velocity = CalculateLaunchVelocity(target, timeToTarget);
        timer = countdown + timeToTarget;
        this.impactPower = impactPower;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 target, float timeToTarget)
    {
        Vector3 direction = target - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);

        Vector3 velocityXZ = directionXZ / timeToTarget;

        float velocityY = (direction.y - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2) / timeToTarget;

        Vector3 launchVelocity = velocityXZ + Vector3.up * velocityY;
        return launchVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
