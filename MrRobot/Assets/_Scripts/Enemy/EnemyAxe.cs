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
    private float lifetime = 4f; // Czas, po którym toporek zostanie zatrzymany

    private int damage;

    [SerializeField] private AudioSource audioSource;       // AudioSource do odtwarzania dŸwiêków
    [SerializeField] private AudioClip flySound;            // DŸwiêk lotu topora

    private void Start()
    {
        // Upewnij siê, ¿e AudioSource i dŸwiêk lotu s¹ przypisane
        if (audioSource != null && flySound != null)
        {
            audioSource.clip = flySound;
            audioSource.loop = true;  // Zapêtlenie dŸwiêku lotu
            audioSource.Play();       // Rozpoczêcie odtwarzania dŸwiêku lotu
        }
    }
    private void Update()
    {
        axeVisuals.Rotate(_rotationSpeed * Time.deltaTime * Vector3.right);
        _timer -= Time.deltaTime;

        // Jeœli timer > 0, aktualizuj kierunek lotu topora w stronê gracza
        if (_timer > 0)
        {
            _direction = _player.position + Vector3.up - transform.position;
        }

        transform.forward = rb.velocity;

        // Aktualizacja g³oœnoœci dŸwiêku na podstawie odleg³oœci od gracza
        UpdateSoundVolume();

        // Zwiêksz licznik czasu
        timeSinceLaunch += Time.deltaTime;

        // Jeœli minê³o lifetime sekund, zatrzymaj dŸwiêk lotu
        if (timeSinceLaunch >= lifetime && audioSource.isPlaying)
        {
            audioSource.Stop(); // Zatrzymaj dŸwiêk po zakoñczeniu lotu
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = _direction.normalized * _flySpeed;
        
    }
        // Aktualizacja g³oœnoœci dŸwiêku na podstawie odleg³oœci od gracza
    private void UpdateSoundVolume()
    {
        if (_player != null && audioSource != null)
        {
            float distance = Vector3.Distance(transform.position, _player.position);
            // Ustawienie g³oœnoœci na podstawie odleg³oœci (im bli¿ej, tym g³oœniej)
            audioSource.volume = Mathf.Clamp(1 / (distance + 1), 0.6f, 1f); // Ograniczenie g³oœnoœci miêdzy 0.6 a 1
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

        // ZnajdŸ AudioSource na ImpactFX
        AudioSource impactAudioSource = newFx.GetComponent<AudioSource>();

        // Dynamicznie dostosuj g³oœnoœæ dŸwiêku uderzenia w zale¿noœci od odleg³oœci od gracza
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

