using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform PlayerBody;
    public PlayerControlls Controls { get; private set; }
    public PlayerAim Aim { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerWeaponController Weapon { get; private set; }
    public PlayerWeaponVisuals WeaponVisuals { get; private set; }
    public PlayerInteraction PlayerInteraction { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public Ragdoll Ragdoll { get; private set; }
    public Animator Animator { get; private set; }

    public bool ControlsEnabled { get; private set; }

    private void Awake()
    {
        Controls = new PlayerControlls();
        //Controls = ControlsManager.Instance.Controls;
        Animator = GetComponentInChildren<Animator>();
        Ragdoll = GetComponent<Ragdoll>();
        PlayerHealth = GetComponent<PlayerHealth>();
        Aim = GetComponent<PlayerAim>();
        Movement = GetComponent<PlayerMovement>();
        Weapon = GetComponent<PlayerWeaponController>();
        WeaponVisuals = GetComponent<PlayerWeaponVisuals>();
        PlayerInteraction = GetComponent<PlayerInteraction>();
    }
    
    private void OnEnable()
    {
        Controls.Enable();
        Controls.Character.UIMissionTooltipSwitch.performed += ctx => UI.instance.inGameUI.SwitchMissionTooltip();
        Controls.Character.UIPause.performed += ctx => UI.instance.PauseSwitch(); 
    }

    private void OnDisable()
    {
        Controls.Disable();
    }

    public void SetControlsEnabledTo(bool enabled)
    { 
        ControlsEnabled = enabled;
    }
}
