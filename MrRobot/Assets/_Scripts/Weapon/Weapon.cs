using UnityEngine;
using UnityEngine.Serialization;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}

public enum ShootType
{
    Single,
    Auto
}


[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public ShootType shootType;
    public int bulletDamage;
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;
    public float ReloadSpeed { get; private set; }
    public float EquipSpeed { get; private set; }
    public float GunDistance { get; private set; }
    public float CameraDistance { get; private set; }

    public float fireRate = 1;
    public float defaultFireRate;

    private float _lastShootTime;

    private float _baseSpread;
    private float _currentSpread = 2;
    private float _maximumSpread = 3;
    private float _spreadIncreaseRate = .15f;

    private float _lastSpreadUpdateTime;
    private float _spreadCooldown = 1;

    public int BulletsPerShot { get; private set; }
    [Header("Burst Fire")] 
    private bool _burstAvailable;
    public bool burstActive;
    private int _burstBulletsPerShot;
    private float _burstFireRate;
    public float BurstFireDelay { get; private set; }

    public WeaponData WeaponData { get; private set; }


    public Weapon(WeaponData weaponData)
    {
        bulletDamage = weaponData.bulletDamage;

        fireRate = weaponData.fireRate; 
        weaponType = weaponData.weaponType;
        _baseSpread = weaponData.baseSpread;
        _maximumSpread = weaponData.maxSpread;
        _spreadIncreaseRate = weaponData.spreadIncreaseRate;

        ReloadSpeed = weaponData.reloadSpeed;
        EquipSpeed = weaponData.equipmentSpeed;
        GunDistance = weaponData.gunDistance;
        CameraDistance = weaponData.cameraDistance;

        _burstAvailable = weaponData.burstAvailable;
        burstActive = weaponData.burstActive;
        _burstBulletsPerShot = weaponData.burstBulletsPerShot;
        _burstFireRate = weaponData.burstFireRate;
        BurstFireDelay = weaponData.burstFireDelay;

        BulletsPerShot = weaponData.bulletsPerShot;
        shootType = weaponData.shootType;

        bulletsInMagazine = weaponData.bulletsInMagazine;
        magazineCapacity = weaponData.magazineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;
        
        defaultFireRate = fireRate;

        this.WeaponData = weaponData;
    }


    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            BurstFireDelay = 0;
            return true;
        }
        
        return burstActive;
    }

    public void ToggleBurst()
    {
        if(_burstAvailable == false)
            return;
        burstActive = !burstActive;

        if (burstActive)
        {
            BulletsPerShot = _burstBulletsPerShot;
            fireRate = _burstFireRate;
        }
        else
        {
            BulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();
        float randomizedValue = Random.Range(-_currentSpread, _currentSpread);
        
        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue / 2, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > _lastSpreadUpdateTime + _spreadCooldown)
        {
            _currentSpread = _baseSpread;
        }
        else
        {
            IncreaseSpread();
        }
        _lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        _currentSpread = Mathf.Clamp(_currentSpread + _spreadIncreaseRate, _baseSpread, _maximumSpread);
    }
    
    private bool ReadyToFire()
    {
        if (Time.time > _lastShootTime + 1 / fireRate)
        {
            _lastShootTime = Time.time;
            return true;
        }

        return false;
    }
    public bool CanShoot()
    {
        if (HaveEnoughBullets() && ReadyToFire())
        {
            
            return true;
        }

        return false;
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            
            return true;
        }

        return false;
    }

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;
        
        if (totalReserveAmmo > 0)
        {
            return true;
        }

        return false;
    }

    public void RefillBullets()
    {
        int bulletsToReload = magazineCapacity;

        if (bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;
        }
    }
        public WeaponData GetWeaponData()
{
    return WeaponData;
}
}