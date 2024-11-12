using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerWeaponController : MonoBehaviour
{

    [SerializeField] private LayerMask whatIsAlly;
    private Player _player;

    [SerializeField] private Weapon currentWeapon;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletImpactForce = 100;

    private const float REFERENCE_BULLET_SPEED = 20f;

    [SerializeField] private WeaponData defaultWeaponData;
    
    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")] 
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    [SerializeField] private GameObject weaponPickupPrefab;

    private bool _weaponReady;
    private bool _isShooting;
       
    //audioSO
    [SerializeField] private AudioEvent weaponPickupAudioEvent;

    [SerializeField] private WeaponAudioData weaponAudioData;
    private AudioSource audioSource;
    [Header("Audio Cooldowns")]
    [SerializeField] private float shootSoundCooldown = 0.1f;
    public AudioClip weaponSwitchSound;



    private bool isOutOfAmmo = false;
    private float lastShootSoundTime = -1f;

    

private void Start()
{
    _player = GetComponent<Player>();
    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    AssignInputEvents();
    Invoke("EquipStartingWeapon", .1f);
}


    private void Update()
    {
        if(_isShooting)
            Shoot();
        
    }

    private void AssignInputEvents()
    {
        PlayerControlls controls = _player.Controls;
        
        controls.Character.Fire.performed += context => _isShooting = true;
        controls.Character.Fire.canceled += context => _isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += context => EquipWeapon(4);


        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };

        controls.Character.ToggleWeaponMode.performed += context => currentWeapon.ToggleBurst();
    }

    private void Reload()
    {
         isOutOfAmmo = false;  // Reset flagi przy prze³adowaniu
         SetWeaponReady(false);
         _player.WeaponVisuals.PlayReloadAnimation();

        // Odtwarzanie dŸwiêku prze³adowania
        if (weaponAudioData != null && weaponAudioData.reloadSound != null)
            {
            audioSource.PlayOneShot(weaponAudioData.reloadSound);
            }
    }

    private void EquipWeapon(int i)
{
        if (i >= weaponSlots.Count)
            return;

        SetWeaponReady(false);
        currentWeapon = weaponSlots[i];

        // Aktualizowanie audioSO dla nowej broni
        weaponAudioData = currentWeapon.GetWeaponData().weaponAudioData;

        _player.WeaponVisuals.PlayWeaponEquipAnimation();
    
        // Odtworzenie dŸwiêku zmiany broni
        if (weaponAudioData.weaponSwitchSound != null)
        {
            audioSource.PlayOneShot(weaponAudioData.weaponSwitchSound);
        }

        CameraManager.instance.ChangeCameraDistance(currentWeapon.CameraDistance);
}


    private void DropWeapon()
    {
        if(HasOnlyOneWeapon())
            return;

        CreateWeaponOnTheGround();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    private void CreateWeaponOnTheGround()
    {
        GameObject dropedWeapon = ObjectPool.Instance.GetObject(weaponPickupPrefab, transform);

        dropedWeapon.GetComponent<PickupWeapon>()?.SetupPickupWeapon(currentWeapon, transform);
    }

    public void SetWeaponReady(bool ready)
    {
        _weaponReady = ready;
    }

    public bool WeaponReady()
    {
        return _weaponReady;
    }

    public Weapon WeaponInSlot(WeaponType weaponType)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == weaponType)
            {
                return weapon;
            }
        }

        return null;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

public void PickupWeapon(Weapon newWeapon)
{
    // Wywo³aj dŸwiêk podniesienia broni
    weaponPickupAudioEvent?.Raise();

    if (WeaponInSlot(newWeapon.weaponType) != null)
    {
        WeaponInSlot(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletsInMagazine;
        return;
    }

    if (weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
    {
        int weaponIndex = weaponSlots.IndexOf(currentWeapon);
        _player.WeaponVisuals.SwitchOffWeaponModels();
        weaponSlots[weaponIndex] = newWeapon;
        CreateWeaponOnTheGround();
        EquipWeapon(weaponIndex);
        
        return;
    }
    
    weaponSlots.Add(newWeapon);
    _player.WeaponVisuals.SwitchOnBackupWeaponModel();
}

    
    private void EquipStartingWeapon()
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);
        
        EquipWeapon(0);
    }

    public Weapon CurrentWeapon() => currentWeapon;


    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
                return weapon;

        }

        return null;
    }

    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);
        for (int i = 1; i <= currentWeapon.BulletsPerShot; i++)
        {
            FireSingleBullet();
            yield return new WaitForSeconds(currentWeapon.BurstFireDelay);
            
            if(i >= currentWeapon.BulletsPerShot)
                SetWeaponReady(true);
        }
    }

private void Shoot()
{
    if (!WeaponReady())
        return;

    if (currentWeapon.bulletsInMagazine <= 0)
    {
        if (!isOutOfAmmo && weaponAudioData != null && weaponAudioData.emptyMagazineSound != null)
        {
            audioSource.PlayOneShot(weaponAudioData.emptyMagazineSound);
            isOutOfAmmo = true;
        }
        return;
    }

    isOutOfAmmo = false;
    _player.WeaponVisuals.PlayFireAnimation();

    if (currentWeapon.shootType == ShootType.Single)
        _isShooting = false;

    if (currentWeapon.BurstActivated())
    {
        StartCoroutine(BurstFire());
        return;
    }

    FireSingleBullet();
    TriggerEnemyDodge();
}


    private void FireSingleBullet()
{
    currentWeapon.bulletsInMagazine--;

    // Odtwarzanie dŸwiêku strza³u z CD
    if (weaponAudioData != null && weaponAudioData.shootSound != null && Time.time >= lastShootSoundTime + shootSoundCooldown)
    {
        audioSource.PlayOneShot(weaponAudioData.shootSound);
        lastShootSoundTime = Time.time;
    }

    GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab, GunPoint());
    newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);
    Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

    Bullet bulletScript = newBullet.GetComponent<Bullet>();
    bulletScript.BulletSetup(whatIsAlly, currentWeapon.bulletDamage, currentWeapon.GunDistance, bulletImpactForce);

    Vector3 bulletsDirection = currentWeapon.ApplySpread(BulletDirection());
    rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
    rbNewBullet.velocity = bulletsDirection * bulletSpeed;
}


    public Vector3 BulletDirection()
    {
        Transform aim = _player.Aim.Aim();
        
        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if (!_player.Aim.CanAimPrecisly() && _player.Aim.Target() == null)
        {
            direction.y = 0;
        }
        
        return direction;
    }

    private void TriggerEnemyDodge()
    {
        Vector3 rayOrigin = GunPoint().position;
        Vector3 rayDirection = BulletDirection();
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity))
        {
            EnemyMelee enemyMelee = hit.collider.GetComponentInParent<EnemyMelee>();
            if (enemyMelee != null)
            {
                enemyMelee.ActivateDodgeRoll();
            }
        }
       
    }

    public Transform GunPoint() => _player.WeaponVisuals.CurrentWeaponModel().GunPoint;
}