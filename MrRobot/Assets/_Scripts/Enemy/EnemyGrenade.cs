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

    private LayerMask allyLayerMask;
    private bool canExplode = true;

    private int grenadeDamage;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0 && canExplode)
        {
            Explode();
        }
    }

    private void Explode()
    {
        canExplode = false;
        PlayerExplosionFx();

        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {



                if (IsTargetValid(hit) == false)
                {
                    continue;
                }

                GameObject rootEntity = hit.transform.root.gameObject;
                if (uniqueEntities.Add(rootEntity) == false)
                {
                    continue;
                }
                damagable.TakeDamage(grenadeDamage);
            }

            //ApplyDamagaTo(hit);
            ApplyPhysicalForceTo(hit);

        }
    }

    private void ApplyPhysicalForceTo(Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplayer, ForceMode.Impulse);
        }
    }

    //private static void ApplyDamagaTo(Collider hit)
    //{
    //    IDamagable damagable = hit.GetComponent<IDamagable>();
    //    damagable?.TakeDamage();
    //}

    private void PlayerExplosionFx()
    {
        GameObject newFX = ObjectPool.Instance.GetObject(explosionFX, transform);
        ObjectPool.Instance.ReturnObject(newFX, 1);
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private bool IsTargetValid(Collider collider)
    {
        if(GameManager.Instance.friendlyFire)
        {
            return true;
        }

        if((allyLayerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            return false;
        }

        return true;
    }

    public void SetupGrenade(LayerMask allyLayerMask, Vector3 target, float timeToTarget, float countdown, float impactPower, int grenadeDamage)
    {
        canExplode = true;
        this.grenadeDamage = grenadeDamage;
        this.allyLayerMask = allyLayerMask;
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
