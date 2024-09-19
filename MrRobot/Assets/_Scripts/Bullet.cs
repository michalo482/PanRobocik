using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private BoxCollider _cd;
    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;
    private TrailRenderer _trailRenderer;
    
    
    [SerializeField] private GameObject bulletImpactFX;



    private Vector3 _startPosition;
    private float _flyDistance;
    private bool bulletDisabled;

    private void Awake()
    {
        _cd = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        FadeTrailIfNeeded();
            
        DisableBulletIfNeeded();
        
        ReturnToPoolIfNeeded();
            
    }

    private void ReturnToPoolIfNeeded()
    {
        if(_trailRenderer.time < 0)
            ObjectPool.Instance.ReturnBullet(gameObject);
    }

    private void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(_startPosition, transform.position) > _flyDistance && !bulletDisabled)
        {
            _cd.enabled = false;
            _meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    private void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(_startPosition, transform.position) > _flyDistance - 1.5f)
            _trailRenderer.time -= 2 * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        CreateImpactFX(other);
        ObjectPool.Instance.ReturnBullet(gameObject);
    }

    private void CreateImpactFX(Collision other)
    {
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];

            GameObject newImpactFX = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
            
            Destroy(newImpactFX, 1f);
        }
    }

    public void BulletSetup(float flyDistance)
    {
        bulletDisabled = false;
        _cd.enabled = true;
        _meshRenderer.enabled = true;
        _trailRenderer.time = .25f;
        _startPosition = transform.position;
        _flyDistance = flyDistance + 1;
        
    }
}
