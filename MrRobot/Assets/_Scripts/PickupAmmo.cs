using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AmmoData
{
    public WeaponType WeaponType;
    //public int Amount;
    [Range(10, 100)] public int minAmount;
    [Range(10, 100)] public int maxAmount;
}

public enum AmmoBoxType
{
    smallBox,
    bigBox
}

public class PickupAmmo : Interactable
{
    [SerializeField] private AmmoBoxType boxType;
    [SerializeField]
    [Range(10, 100)] 
    public int healthAmount;
    [SerializeField] private List<AmmoData> smallBoxAmmo;
    [SerializeField] private List<AmmoData> bigBoxAmmo;

    [SerializeField] private GameObject[] boxModels;
    [SerializeField] private AudioEvent pickupAmmoAudioEvent;

    protected override void Start()
    {
        base.Start();

        SetupBoxModel();
    }

    private int GetBulletsAmount(AmmoData ammoData)
    {
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);

        float randomAmmoAmount = Random.Range(min, max);
        return Mathf.RoundToInt(randomAmmoAmount);
    }

    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModels.Length; i++)
        {
            boxModels[i].SetActive(false);

            if (i == (int)boxType)
            {
                boxModels[i].SetActive(true);
                UpdateMeshAndMaterial(boxModels[i].GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if (boxType == AmmoBoxType.bigBox)
        {
            currentAmmoList = bigBoxAmmo;
        }

        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = _weaponController.WeaponInSlot(ammo.WeaponType);

            AddBulletsToWeapon(weapon, GetBulletsAmount(ammo), healthAmount);
        }


        
        Destroy(gameObject);
        //ObjectPool.Instance.ReturnObject(gameObject);
    }

    private void AddBulletsToWeapon(Weapon weapon, int amountOfBullets, int healAmount = 0)
    {
        if (weapon == null)
            return;

        _weaponController.PickupWeapon(weapon);
        playerHealth.ReduceHealth(-healAmount);
        _weaponController.UpdateWeaponUI();
        pickupAmmoAudioEvent?.Raise();
        AudioManager.Instance.PlayHealthSound("heal");

        //weapon.totalReserveAmmo += amountOfBullets;
    }
}
