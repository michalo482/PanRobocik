using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSlot : MonoBehaviour
{
    public Image weaponIcon;
    public TextMeshProUGUI ammoText;

    private void Awake()
    {
        weaponIcon = GetComponentInChildren<Image>();
        ammoText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateWeaponSlot(Weapon weapon, bool activeWeapon)
    {
        if(weapon == null)
        {
            weaponIcon.color = Color.clear;
            ammoText.text = "";
            return;
        }

        Color newColor = activeWeapon ? Color.white : new Color(1,1,1,0.35f);
        weaponIcon.color = newColor;
        weaponIcon.sprite = weapon.WeaponData.weaponIcon;

        ammoText.text = weapon.bulletsInMagazine.ToString() + "/" + weapon.totalReserveAmmo.ToString();
        ammoText.color = Color.white;
    }
}
