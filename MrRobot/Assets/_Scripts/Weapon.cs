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
    public WeaponType WeaponType;
    public ShootType ShootType;
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;
    [Range(1, 3)]
    public float ReloadSpeed = 1;
    [Range(1, 3)]
    public float EquipSpeed = 1;
    public float FireRate = 1;
    [Range(2, 12)] 
    public float gunDistance = 4;
    [Range(3, 8)] 
    public float cameraDistance = 6;

    public float defaultFireRate;

    private float _lastShootTime;

    public float baseSpread;
    public float currentSpread = 2;
    public float maximumSpread = 3;
    public float spreadIncreaseRate = .15f;

    private float _lastSpreadUpdateTime;
    private float _spreadCooldown = 1;
    
    [Header("Burst Fire")]
    public int bulletsPerShot;
    public float burstFireDelay = .1f;
    public int burstModeBulletsPerShot;
    public float burstModeFireRate;
    public bool burstAvailable;
    public bool burstActive;



    public bool BurstActivated()
    {
        if (WeaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }
        
        return burstActive;
    }

    public void ToggleBurst()
    {
        if(burstAvailable == false)
            return;
        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstModeBulletsPerShot;
            FireRate = burstModeFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            FireRate = defaultFireRate;
        }
    }

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();
        float randomizedValue = Random.Range(-currentSpread, currentSpread);
        
        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > _lastSpreadUpdateTime + _spreadCooldown)
        {
            currentSpread = baseSpread;
        }
        else
        {
            IncreaseSpread();
        }
        _lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }
    
    private bool ReadyToFire()
    {
        if (Time.time > _lastShootTime + 1 / FireRate)
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
}
