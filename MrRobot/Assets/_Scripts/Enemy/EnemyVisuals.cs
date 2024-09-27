using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum EnemyMeleeWeaponType
{
    OneHand,
    Throw
}


public class EnemyVisuals : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    
    [Header("Weapon model")] 
    [SerializeField] private EnemyWeaponModel[] weaponModels;
    [SerializeField] private EnemyMeleeWeaponType weaponType;

    [Header("Corruption visuals")] 
    [SerializeField] private GameObject[] corruptionCrystal;
    [SerializeField] private int corruptionAmount;
    
    
    public GameObject CurrentWeaponModel { get; private set; }

    private void Awake()
    {
        weaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
        CollectCorruptionCrystal();
    }
    private void CollectCorruptionCrystal()
    {
        EnemyCorruptionCrystal[] crystalCoponents = GetComponentsInChildren<EnemyCorruptionCrystal>(true);
        corruptionCrystal = new GameObject[crystalCoponents.Length];

        for (int i = 0; i < crystalCoponents.Length; i++)
        {
            corruptionCrystal[i] = crystalCoponents[i].gameObject;
        }
    }

    private void SetupRandomWeapon()
    {
        foreach (EnemyWeaponModel weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }

        List<EnemyWeaponModel> filteredWeaponModels = new List<EnemyWeaponModel>();

        foreach (EnemyWeaponModel weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponModel);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);
        CurrentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        CurrentWeaponModel.SetActive(true);
    }

    public void SetupWeaponType(EnemyMeleeWeaponType weaponType)
    {
        this.weaponType = weaponType;
    }
    
    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);
        
        Material newMaterial = new Material(skinnedMeshRenderer.material)
        {
            mainTexture = colorTextures[randomIndex]
        };

        skinnedMeshRenderer.material = newMaterial;
    }

    private void SetupRandomCorruption()
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < corruptionCrystal.Length; i++)
        {
            availableIndexes.Add(i);
            corruptionCrystal[i].SetActive(false);
        }

        for (int i = 0; i < corruptionAmount; i++)
        {
            
            if(availableIndexes.Count <= 0)
                break;
            int randomIndex = Random.Range(0, availableIndexes.Count);
            int objectIndex = availableIndexes[randomIndex];
            
            corruptionCrystal[objectIndex].SetActive(true);
            
            availableIndexes.RemoveAt(randomIndex);
        }
    }

    public void SetupRandomLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }
}
