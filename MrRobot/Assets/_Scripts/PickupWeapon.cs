using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupWeapon : Interactable
{
    
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private BackupWeaponModel[] models;
    [SerializeField] private Weapon weapon;

    private bool _oldWeapon;


    protected override void Start()
    {
        if (_oldWeapon == false)
            weapon = new Weapon(weaponData);
        
        SetupGameObject();
    }

    public override void Interaction()
    {
        _weaponController.PickupWeapon(weapon);
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    public void SetupPickupWeapon(Weapon weaponPassed, Transform transformPassed)
    {
        _oldWeapon = true;
        this.weapon = weaponPassed;
        weaponData = weaponPassed.WeaponData;

        this.transform.position = transformPassed.position + new Vector3(0, .75f,0);
    }

    private void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if (model.WeaponType == weaponData.weaponType)
            {
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
                model.gameObject.SetActive(true);
            }
        }
    }
    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        gameObject.name = "Pickup weapon " + weaponData.weaponType.ToString();
        
        SetupWeaponModel();
    }
}
