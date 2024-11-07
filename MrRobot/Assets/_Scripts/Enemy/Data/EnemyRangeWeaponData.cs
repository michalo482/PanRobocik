using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Enemy Data/Range Weapon Data")]
public class EnemyRangeWeaponData : ScriptableObject
{
    [Header("Weapon Details")]
    public EnemyRangeWeaponType weaponType;
    public float fireRate = 1f;

    public int minBulletsPerAttack = 1;
    public int maxBulletsPerAttack = 1;

    public float minWeaponCooldown = 2f;
    public float maxWeaponCooldown = 3f;

    [Header("Bullet details")]
    public int bulletDamage;
    public float bulletSpeed = 20;
    public float weaponSpread = .1f;

    public int GetBulletsPerAttack() => Random.Range(minBulletsPerAttack, maxBulletsPerAttack + 1);
    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);

    public Vector3 ApplyWeaponSpread(Vector3 originalDirection)
    {
        float randomizedValue = Random.Range(-weaponSpread, weaponSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }
}
