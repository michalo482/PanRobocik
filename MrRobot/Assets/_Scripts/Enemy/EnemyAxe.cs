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
    private float timeSinceLaunch = 0f; // Licznik czasu od momentu rzutu toporkiem
    private float lifetime = 4f; // Czas, po kt�rym toporek zostanie zniszczony (5 sekund)

    private int damage;

    [SerializeField] private AudioSource audioSource; // Dodaj zmienn� AudioSource
    [SerializeField] private AudioClip flySound; // Zmienna dla d�wi�ku lotu

    private void Start()
    {
        // Upewnij si�, �e d�wi�k b�dzie odtwarzany tylko w locie
        if (audioSource != null && flySound != null)
        {
            audioSource.clip = flySound;
            audioSource.loop = true;  // Zap�tlenie d�wi�ku
            audioSource.Play(); // Rozpocz�cie odtwarzania
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

        // Zwi�ksz licznik czasu
        timeSinceLaunch += Time.deltaTime;

        // Je�li min�o 5 sekund od rzutu toporkiem, zniszcz go
        if (timeSinceLaunch >= lifetime)
        {
            Destroy(gameObject); // Zniszczenie toporka
        }
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

        // Stw�rz efekty wizualne
        GameObject newFx = ObjectPool.Instance.GetObject(impactFX, transform);
        ObjectPool.Instance.ReturnObject(gameObject);
        ObjectPool.Instance.ReturnObject(newFx, 1f);

        // Zatrzymaj d�wi�k po uderzeniu
        if (audioSource != null)
        {
            audioSource.Stop(); // Zatrzymanie d�wi�ku
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    IDamagable damagable = other.GetComponent<IDamagable>();
    //    if (damagable != null)
    //    {

            
    //    }
        

    //}
}