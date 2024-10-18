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
    private float _rotationSpeed = 1600;
    private Vector3 _direction;

    private float _timer = 1;

    public void AxeSetup(float flySpeed, Transform player, float timer)
    {
        _flySpeed = flySpeed;
        _player = player;
        _timer = timer;
    }
    
    private void Update()
    {
        axeVisuals.Rotate(Vector3.right * _rotationSpeed * Time.deltaTime);

        _timer -= Time.deltaTime;

        if (_timer > 0)
        {
            _direction = _player.position + Vector3.up - transform.position;
        }
        rb.velocity = _direction.normalized * _flySpeed;

        transform.forward = rb.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Player player = other.GetComponent<Player>();

        if (bullet != null || player != null)
        {
            GameObject newFx = ObjectPool.Instance.GetObject(impactFX, transform);
            //newFx.transform.position = transform.position;
            ObjectPool.Instance.ReturnObject(gameObject);
            ObjectPool.Instance.ReturnObject(newFx, 1f);
            
        }

    }
}
