using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    [SerializeField] private WeaponData _weaponData;
    private void OnTriggerEnter(Collider other)
    {
        //other.GetComponent<PlayerWeaponController>()?.PickupWeapon(_weaponData);
    }
}
