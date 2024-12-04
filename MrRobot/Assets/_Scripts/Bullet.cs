using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int bulletDamage;
    private float impactForce;
    
    private BoxCollider _cd;
    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;
    private TrailRenderer _trailRenderer;
    
    
    [SerializeField] private GameObject bulletImpactFX;



    private Vector3 _startPosition;
    private float _flyDistance;
    private bool bulletDisabled;

    private LayerMask allyLayerMask;

    protected virtual void Awake()
    {
        _cd = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    protected virtual void Update()
    {
        FadeTrailIfNeeded();
            
        DisableBulletIfNeeded();
        
        ReturnToPoolIfNeeded();
            
    }

    protected void ReturnToPoolIfNeeded()
    {
        if(_trailRenderer.time < 0)
            ReturnBulletToPool();
    }

    protected void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(_startPosition, transform.position) > _flyDistance && !bulletDisabled)
        {
            _cd.enabled = false;
            _meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    protected void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(_startPosition, transform.position) > _flyDistance - 1.5f)
            _trailRenderer.time -= 2 * Time.deltaTime;
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if(FriendlyFire() == false)
        {
            if((allyLayerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                ReturnBulletToPool(10);
                return;
            }
        }

        CreateImpactFX();
        ReturnBulletToPool();

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        damagable?.TakeDamage(bulletDamage);

        

        ApplyBulletImpactToEnemy(other);

    }

    private void ApplyBulletImpactToEnemy(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Vector3 force = _rb.velocity.normalized * impactForce;
            Rigidbody hitRb = other.collider.attachedRigidbody;

            enemy.BulletImpact(force, other.contacts[0].point, hitRb);
        }
    }

    protected void ReturnBulletToPool(float delay = 0)
    {
        ObjectPool.Instance.ReturnObject(gameObject, delay);
    }

    protected void CreateImpactFX()
    {
        GameObject newImpactFX = ObjectPool.Instance.GetObject(bulletImpactFX, transform);
        ObjectPool.Instance.ReturnObject(newImpactFX, 1f);
        //if (other.contacts.Length > 0)
        //{
        //    ContactPoint contact = other.contacts[0];

        //        //Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
        //    newImpactFX.transform.position = contact.point;
                
        //}
    }

    public void BulletSetup(LayerMask allyLayer, int bulletDamage, float flyDistance = 100, float impactForce = 100)
    {
        this.impactForce = impactForce;
        this.allyLayerMask = allyLayer;
        this.bulletDamage = bulletDamage;
        bulletDisabled = false;
        _cd.enabled = true;
        _meshRenderer.enabled = true;

        _trailRenderer.Clear();

        _trailRenderer.time = .25f;
        _startPosition = transform.position;
        _flyDistance = flyDistance + 1;
        
    }

    public bool FriendlyFire()
    {
        return GameManager.Instance.friendlyFire;
    }
}
