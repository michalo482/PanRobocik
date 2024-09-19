using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{

    private PlayerWeaponVisuals _playerWeaponVisuals;

    private PlayerWeaponController _playerWeaponController;
    // Start is called before the first frame update
    void Start()
    {
        _playerWeaponVisuals = GetComponentInParent<PlayerWeaponVisuals>();
        _playerWeaponController = GetComponentInParent<PlayerWeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadIsOver()
    {
        _playerWeaponVisuals.MaximizeRigWeight();
        _playerWeaponController.CurrentWeapon().RefillBullets();
        _playerWeaponController.SetWeaponReady(true);
    }

    public void ReturnRig()
    {
        _playerWeaponVisuals.MaximizeRigWeight();
        _playerWeaponVisuals.MaximizeLeftHandWeight();
    }
    
    public void WeaponEquipingIsOver()
    {
        _playerWeaponController.SetWeaponReady(true);
    }

    public void SwitchOnWeaponModel() => _playerWeaponVisuals.SwitchOnCurrentWeaponModel();
}
