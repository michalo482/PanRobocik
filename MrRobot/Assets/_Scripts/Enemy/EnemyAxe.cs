using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private GameObject impactFX;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisuals;

    private Transform _player;
    private float _flySpeed;
    private float _rotationSpeed;
    private Vector3 _direction;

    private float _timer = 1;

    private int damage;

    
    private void Update()
    {
        axeVisuals.Rotate(_rotationSpeed * Time.deltaTime * Vector3.right);

        _timer -= Time.deltaTime;

        if (_timer > 0)
        {
            _direction = _player.position + Vector3.up - transform.position;
        }

        transform.forward = rb.velocity;
    }

    private void FixedUpdate()
    {
        rb.velocity = _direction.normalized * _flySpeed;
        
    }

    public void AxeSetup(float flySpeed, Transform player, float timer, int damage)
    {
        _rotationSpeed = 1600;
        this.damage = damage;
        _flySpeed = flySpeed;
        _player = player;
        _timer = timer;
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        damagable?.TakeDamage(damage);

        GameObject newFx = ObjectPool.Instance.GetObject(impactFX, transform);
        //newFx.transform.position = transform.position;
        ObjectPool.Instance.ReturnObject(gameObject);
        ObjectPool.Instance.ReturnObject(newFx, 1f);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    IDamagable damagable = other.GetComponent<IDamagable>();
    //    if (damagable != null)
    //    {

            
    //    }
        

    //}
}
