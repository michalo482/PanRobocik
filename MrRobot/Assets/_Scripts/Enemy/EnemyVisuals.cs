using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum EnemyMeleeWeaponType
{
    OneHand,
    Throw,
    Unarmed
    
}
public enum EnemyRangeWeaponType
{
    Pistol,
    Revolver,
    Shotgun,
    AutoRifle,
    Rifle
}


public class EnemyVisuals : MonoBehaviour
{

    public GameObject grenadeModel;

    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    
    /*[Header("Weapon model")] 
    [SerializeField] private EnemyWeaponModel[] weaponModels;*/
    

    [Header("Corruption visuals")] 
    [SerializeField] private GameObject[] corruptionCrystal;
    [SerializeField] private int corruptionAmount;

    [Header("Rig References")]
    [SerializeField] private Transform leftHandIK;
    [SerializeField] private Transform leftElbowIK;
    [SerializeField] private TwoBoneIKConstraint leftHandIkConstrains;
    [SerializeField] private MultiAimConstraint weaponAimConstrains;

    private float _leftHandTargetWeight;
    private float _weaponAimTargetWeight;
    private float _rigChangeRate;


    public GameObject CurrentWeaponModel { get; private set; }

    private void Update()
    {
        if(leftHandIkConstrains != null)
            leftHandIkConstrains.weight = AdjustIKWeight(leftHandIkConstrains.weight, _leftHandTargetWeight);

        if(weaponAimConstrains != null)
            weaponAimConstrains.weight = AdjustIKWeight(weaponAimConstrains.weight, _weaponAimTargetWeight);
    }
    /*private void Awake()
    {
        //weaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
        CollectCorruptionCrystal();
    }*/
    private GameObject[] CollectCorruptionCrystal()
    {
        EnemyCorruptionCrystal[] crystalComponents = GetComponentsInChildren<EnemyCorruptionCrystal>(true);
        GameObject[] corruptionCrystals = new GameObject[crystalComponents.Length];

        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }

        return corruptionCrystals;
    }

    public void EnableWeaponModel(bool isActive)
    {
        CurrentWeaponModel?.gameObject.SetActive(isActive);
    }

    private void SetupRandomWeapon()
    {
        /*foreach (EnemyWeaponModel weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }*/

        bool thisEnemyIsMelee = GetComponent<EnemyMelee>() != null;
        bool thisEnemyIsRange = GetComponent<EnemyRange>() != null;

        if(thisEnemyIsRange)
        {
            CurrentWeaponModel = FindRangeWeaponModel();
        }

        if (thisEnemyIsMelee)
        {
            CurrentWeaponModel = FindMeleeWeaponModel();
        }
        CurrentWeaponModel.SetActive(true);

        OverrideAnimatorControllerIfCan();
    }

    public void EnableGrenadeModel(bool active)
    {
        grenadeModel?.gameObject.SetActive(active);
    }

    private GameObject FindRangeWeaponModel()
    {
        EnemyRangeWeaponModel[] weaponModels = GetComponentsInChildren<EnemyRangeWeaponModel>(true);
        EnemyRangeWeaponType weaponType = GetComponent<EnemyRange>().weaponType;

        foreach (var weaponModel in weaponModels)
        {
            if(weaponModel.weaponType == weaponType)
            {
                SwitchAnimationLayer((int)weaponModel.weaponHoldType);
                SetupLeftHandIK(weaponModel.leftHandTarget, weaponModel.leftElbowTarget);
                return weaponModel.gameObject;
            }
        }

        return null;
    }

    private GameObject FindMeleeWeaponModel()
    {
        EnemyWeaponModel[] weaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
        EnemyMeleeWeaponType weaponType = GetComponent<EnemyMelee>().weaponType;

        List<EnemyWeaponModel> filteredWeaponModels = new List<EnemyWeaponModel>();

        foreach (EnemyWeaponModel weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponModel);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);
        return filteredWeaponModels[randomIndex].gameObject;
    }

    public void EnableWeaponTrail(bool enable)
    {
        EnemyWeaponModel currentWeaponScript = CurrentWeaponModel.GetComponent<EnemyWeaponModel>();
        currentWeaponScript.EnableTrailEffect(enable);
    }

    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController =
            CurrentWeaponModel.GetComponent<EnemyWeaponModel>()?.OverrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
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
        corruptionCrystal = CollectCorruptionCrystal();

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

    private void SwitchAnimationLayer(int layerIndex)
    {

        Animator anim = GetComponentInChildren<Animator>();

        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);
    }

    private void SetupLeftHandIK(Transform leftHandTarget, Transform leftElbowTarget)
    {
        leftHandIK.localPosition = leftHandTarget.localPosition;
        leftHandIK.localRotation = leftHandTarget.localRotation;

        leftElbowIK.localPosition = leftElbowTarget.localPosition;
        leftElbowIK.localRotation = leftElbowTarget.localRotation;
    }

    public void EnableIK(bool enableLeftHand, bool enableAim, float changeRate = 10)
    {
        //rig.weight = enable ? 1 : 0;
        _rigChangeRate = changeRate;
        //Debug.Log(_rigChangeRate);
        _leftHandTargetWeight = enableLeftHand ? 1 : 0;
        //Debug.Log(_leftHandTargetWeight);
        _weaponAimTargetWeight = enableAim ? 1 : 0;
        //Debug.Log(_weaponAimTargetWeight);
    }

    private float AdjustIKWeight(float currentWeight, float targetWeight)
    {
        if(Mathf.Abs(currentWeight - targetWeight) > 0.05f)
        {
            return Mathf.Lerp(currentWeight, targetWeight, _rigChangeRate * Time.deltaTime);
        }
        else
        {
            return targetWeight;
        }
    }

    private GameObject FindSecondaryWeaponModel()
    {
        EnemySecondaryRangeWeaponModel[] weaponModels = GetComponentsInChildren<EnemySecondaryRangeWeaponModel>(true);
        EnemyRangeWeaponType weaponType = GetComponentInParent<EnemyRange>().weaponType;

        foreach(var weaponModel in weaponModels)
        {
            if(weaponModel.weaponType == weaponType)
            {
                //Debug.Log("znalazlem secondary weapon");
                return weaponModel.gameObject;
            }
        }

        return null;
    }

    public void EnableSecondaryWeaponModel(bool active)
    {
        FindSecondaryWeaponModel()?.SetActive(active);
    }
}
