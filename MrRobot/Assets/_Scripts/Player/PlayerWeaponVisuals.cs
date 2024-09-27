using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class PlayerWeaponVisuals : MonoBehaviour
{
    

    private Player _player;
    private Animator _animator;

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    [Header("Left hand IK")] 
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [FormerlySerializedAs("leftHand")] [SerializeField] private Transform leftHandIK_Target;
    [FormerlySerializedAs("leftHandIKIncreaseStep")] [SerializeField] private float leftHandIKWeightIncreaseRate;
    private bool _shouldIncrease_LeftHandIKWeight;

    [Header("Rig")] 
    [FormerlySerializedAs("rigIncreaseStep")] [SerializeField] private float rigWeightIncreaseRate;

    private bool _shouldIncrease_RigWeight;
    private Rig _rig;
    

    private void Start()
    {
        _player = GetComponent<Player>();
        _animator = GetComponentInChildren<Animator>();
        _rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }

    private void Update()
    {
        UpdateRigWeight();

        UpdateLeftHandIKWeight();
    }

    public void PlayFireAnimation()
    {
        _animator.SetTrigger("Fire");
    }

    public void PlayReloadAnimation()
    {
        float reloadSpeed = _player.Weapon.CurrentWeapon().ReloadSpeed;
                
        _animator.SetFloat("ReloadSpeed", reloadSpeed);
        _animator.SetTrigger("Reload");
        ReduceRigWeight();
    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = _player.Weapon.CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].WeaponType == weaponType)
            {
                weaponModel = weaponModels[i];
            }
        }

        return weaponModel;
    }
    
    

    private void UpdateLeftHandIKWeight()
    {
        if (_shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                _shouldIncrease_LeftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (_shouldIncrease_RigWeight)
        {
            _rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (_rig.weight >= 1)
            {
                _shouldIncrease_RigWeight = false;
            }
        }
    }

    public void MaximizeLeftHandWeight()
    {
        _shouldIncrease_LeftHandIKWeight = true;
    }

    private void ReduceRigWeight()
    {
        _rig.weight = 0.15f;
    }

    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModel().equipAnimationType;

        float equipSpeed = _player.Weapon.CurrentWeapon().EquipSpeed;
        
        leftHandIK.weight = 0;
        ReduceRigWeight();
        _animator.SetFloat("EquipType", ((float)equipType));
        _animator.SetTrigger("EquipWeapon");
        _animator.SetFloat("EquipSpeed", equipSpeed);
        
    }
    
    public void MaximizeRigWeight() => _shouldIncrease_RigWeight = true;

    

    public void SwitchOnCurrentWeaponModel()
    {
        SwitchOffWeaponModels();
        SwitchOffBackupWeaponModels();
        if(!_player.Weapon.HasOnlyOneWeapon())
            SwitchOnBackupWeaponModel();
        SwitchAnimationLayer((int)CurrentWeaponModel().HoldType);
        CurrentWeaponModel().gameObject.SetActive(true);
        
        AttachLeftHand();
    }


    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }


    public void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeaponModel backupWeapon in backupWeaponModels)
        {
            backupWeapon.Activate(false);
        }
    }

    public void SwitchOnBackupWeaponModel()
    {
        SwitchOffBackupWeaponModels();
        BackupWeaponModel lowHangWeapon = null;
        BackupWeaponModel backHangWeapon = null;
        BackupWeaponModel sideHangWeapon = null;

        foreach (BackupWeaponModel weaponModel in backupWeaponModels)
        {
            
            if (weaponModel.WeaponType == _player.Weapon.CurrentWeapon().weaponType)
                continue;
            
            if (_player.Weapon.WeaponInSlot(weaponModel.WeaponType) != null)
            {
                if (weaponModel.HangTypeIs(HangType.LowBackHang))
                    lowHangWeapon = weaponModel;
                if (weaponModel.HangTypeIs(HangType.BackHang))
                    backHangWeapon = weaponModel;
                if (weaponModel.HangTypeIs(HangType.SideHang))
                    sideHangWeapon = weaponModel;
            }
        }
        
        lowHangWeapon?.Activate(true);
        backHangWeapon?.Activate(true);
        sideHangWeapon?.Activate(true);
    }
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().HoldPoint;

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < _animator.layerCount; i++)
        {
            _animator.SetLayerWeight(i, 0);
        }
        
        _animator.SetLayerWeight(layerIndex, 1);
    }
}


