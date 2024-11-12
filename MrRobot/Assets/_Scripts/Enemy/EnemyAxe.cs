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
    private float lifetime = 4f; // Czas, po którym toporek zostanie zniszczony (5 sekund)

    private int damage;

    [SerializeField] private AudioSource audioSource; // Dodaj zmienn¹ AudioSource
    [SerializeField] private AudioClip flySound; // Zmienna dla dŸwiêku lotu

    private void Start()
    {
        // Upewnij siê, ¿e dŸwiêk bêdzie odtwarzany tylko w locie
        if (audioSource != null && flySound != null)
        {
            audioSource.clip = flySound;
            audioSource.loop = true;  // Zapêtlenie dŸwiêku
            audioSource.Play(); // Rozpoczêcie odtwarzania
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

        // Zwiêksz licznik czasu
        timeSinceLaunch += Time.deltaTime;

        // Jeœli minê³o 5 sekund od rzutu toporkiem, zniszcz go
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

        // Stwórz efekty wizualne
        GameObject newFx = ObjectPool.Instance.GetObject(impactFX, transform);
        ObjectPool.Instance.ReturnObject(gameObject);
        ObjectPool.Instance.ReturnObject(newFx, 1f);

        // Zatrzymaj dŸwiêk po uderzeniu
        if (audioSource != null)
        {
            audioSource.Stop(); // Zatrzymanie dŸwiêku
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