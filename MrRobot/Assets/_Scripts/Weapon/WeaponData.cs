using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon System/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    [Header("Bullet")]
    public int bulletDamage;

    public WeaponType weaponType;
    public ShootType shootType;
    public int bulletsPerShot = 1;
    public float fireRate;

    [Header("Bullets info")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;
    
    [Header("Spread")] 
    public float baseSpread;
    public float maxSpread;
    public float spreadIncreaseRate = .15f;

    [Header("Weapon general info")] 
    [Range(1, 3)]
    public float reloadSpeed = 1;
    [Range(1, 3)] 
    public float equipmentSpeed = 1;
    [Range(3, 25)] 
    public float gunDistance = 4;
    [Range(4, 10)] 
    public float cameraDistance = 6;

    [Header("Burst")] 
    public bool burstAvailable;
    public bool burstActive;
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .1f;

    [Header("UI elements")]
    public Sprite weaponIcon;
}
