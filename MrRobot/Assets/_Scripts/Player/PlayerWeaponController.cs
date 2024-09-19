using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player _player;

    [SerializeField] private Weapon currentWeapon;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    private const float REFERENCE_BULLET_SPEED = 20f;

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")] 
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    private bool _weaponReady;
    private bool _isShooting;
    

    private void Start()
    {
        _player = GetComponent<Player>();

        AssignInputEvents();

        
        Invoke("EquipStartingWeapon", .1f);
        //currentWeapon.bulletsInMagazine = currentWeapon.totalReserveAmmo;
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
        SetWeaponReady(false);
        _player.WeaponVisuals.PlayReloadAnimation();
    }

    private void EquipWeapon(int i)
    {
        if(i >= weaponSlots.Count)
            return;

        SetWeaponReady(false);
        currentWeapon = weaponSlots[i];

        //_player.WeaponVisuals.SwitchOffWeaponModels();
        _player.WeaponVisuals.PlayWeaponEquipAnimation();
        
        CameraManager.instance.ChangeCameraDistance(currentWeapon.cameraDistance);
    }

    private void DropWeapon()
    {
        if(HasOnlyOneWeapon())
            return;

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
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
            if (weapon.WeaponType == weaponType)
            {
                return weapon;
            }
        }

        return null;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            Debug.Log("Nie mam miejsca");
            return;
        }
        
        weaponSlots.Add(newWeapon);
        _player.WeaponVisuals.SwitchOnBackupWeaponModel();
    }
    
    private void EquipStartingWeapon()
    {
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
        for (int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();
            yield return new WaitForSeconds(currentWeapon.burstFireDelay);
            
            if(i >= currentWeapon.bulletsPerShot)
                SetWeaponReady(true);
        }
    }

    private void Shoot()
    {
        if (WeaponReady() == false)
            return;
        
        if (currentWeapon.CanShoot() == false)
            return;
        
        _player.WeaponVisuals.PlayFireAnimation();

        if (currentWeapon.ShootType == ShootType.Single)
            _isShooting = false;

        if (currentWeapon.BurstActivated() == true)
        {
            StartCoroutine(BurstFire());
            return;
        }
        
        FireSingleBullet();

    }

    private void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazine--;
        
        GameObject newBullet = ObjectPool.Instance.GetBullet();
        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);
        //Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(currentWeapon.gunDistance);
        
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

    public Transform GunPoint() => _player.WeaponVisuals.CurrentWeaponModel().GunPoint;
}
