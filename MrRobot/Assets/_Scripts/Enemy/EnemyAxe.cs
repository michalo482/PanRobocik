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
    private float lifetime = 4f; // Czas, po kt�rym toporek zostanie zatrzymany

    private int damage;

    [SerializeField] private AudioSource audioSource;       // AudioSource do odtwarzania d�wi�k�w
    [SerializeField] private AudioClip flySound;            // D�wi�k lotu topora

    private void Start()
    {
        // Upewnij si�, �e AudioSource i d�wi�k lotu s� przypisane
        if (audioSource != null && flySound != null)
        {
            audioSource.clip = flySound;
            audioSource.loop = true;  // Zap�tlenie d�wi�ku lotu
            audioSource.Play();       // Rozpocz�cie odtwarzania d�wi�ku lotu
        }
    }
    private void Update()
    {
        axeVisuals.Rotate(_rotationSpeed * Time.deltaTime * Vector3.right);
        _timer -= Time.deltaTime;

        // Je�li timer > 0, aktualizuj kierunek lotu topora w stron� gracza
        if (_timer > 0)
        {
            _direction = _player.position + Vector3.up - transform.position;
        }

        transform.forward = rb.velocity;

        // Aktualizacja g�o�no�ci d�wi�ku na podstawie odleg�o�ci od gracza
        UpdateSoundVolume();

        // Zwi�ksz licznik czasu
        timeSinceLaunch += Time.deltaTime;

        // Je�li min�o lifetime sekund, zatrzymaj d�wi�k lotu
        if (timeSinceLaunch >= lifetime && audioSource.isPlaying)
        {
            audioSource.Stop(); // Zatrzymaj d�wi�k po zako�czeniu lotu
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = _direction.normalized * _flySpeed;
        
    }
        // Aktualizacja g�o�no�ci d�wi�ku na podstawie odleg�o�ci od gracza
    private void UpdateSoundVolume()
    {
        if (_player != null && audioSource != null)
        {
            float distance = Vector3.Distance(transform.position, _player.position);
            // Ustawienie g�o�no�ci na podstawie odleg�o�ci (im bli�ej, tym g�o�niej)
            audioSource.volume = Mathf.Clamp(1 / (distance + 1), 0.6f, 1f); // Ograniczenie g�o�no�ci mi�dzy 0.6 a 1
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

        // Pobierz obiekt efektu uderzenia z ObjectPool
        GameObject newFx = ObjectPool.Instance.GetObject(impactFX, transform);

        // Znajd� AudioSource na ImpactFX
        AudioSource impactAudioSource = newFx.GetComponent<AudioSource>();

        // Dynamicznie dostosuj g�o�no�� d�wi�ku uderzenia w zale�no�ci od odleg�o�ci od gracza
        if (impactAudioSource != null)
        {
            float distance = Vector3.Distance(transform.position, _player.position);
            impactAudioSource.volume = Mathf.Clamp(1 / (distance + 1), 0.6f, 1f);
            impactAudioSource.Play();
        }

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

