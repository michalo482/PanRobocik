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

    private void Awake()
    {
        Controls = new PlayerControlls();
        Aim = GetComponent<PlayerAim>();
        Movement = GetComponent<PlayerMovement>();
        Weapon = GetComponent<PlayerWeaponController>();
        WeaponVisuals = GetComponent<PlayerWeaponVisuals>();
        PlayerInteraction = GetComponent<PlayerInteraction>();
    }
    
    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
