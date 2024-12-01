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
    private float timeSinceLaunch = 0f; 
    private float lifetime = 4f; 

    private int damage;

    [SerializeField] private AudioSource audioSource;       
    [SerializeField] private AudioClip flySound;           

    private void Start()
    {
        if (audioSource != null && flySound != null)
        {
            audioSource.clip = flySound;
            audioSource.loop = true;  
            audioSource.Play();      
        }
    }
    private void Update()
    {
        axeVisuals.Rotate(_rotationSpeed * Time.deltaTime * Vector3.right);
        _timer -= Time.deltaTime;

        if (_timer > 0)
        {
            _direction = _player.position + Vector3.up - transform.position;
        }

        transform.forward = rb.velocity;

        UpdateSoundVolume();
        timeSinceLaunch += Time.deltaTime;

        if (timeSinceLaunch >= lifetime && audioSource.isPlaying)
        {
            audioSource.Stop(); 
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = _direction.normalized * _flySpeed;
        
    }
    private void UpdateSoundVolume()
    {
        if (_player != null && audioSource != null)
        {
            float distance = Vector3.Distance(transform.position, _player.position);
            audioSource.volume = Mathf.Clamp(1 / (distance + 1), 0.6f, 1f); 
        }
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


        ObjectPool.Instance.ReturnObject(gameObject);
        ObjectPool.Instance.ReturnObject(newFx, 1f);
    }



}

