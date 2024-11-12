using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private AudioEvent startGameAudioEvent;


    private void Awake()
    {
        Controls = new PlayerControlls();

        Animator = GetComponentInChildren<Animator>();
        Ragdoll = GetComponent<Ragdoll>();
        PlayerHealth = GetComponent<PlayerHealth>();
        Aim = GetComponent<PlayerAim>();
        Movement = GetComponent<PlayerMovement>();
        Weapon = GetComponent<PlayerWeaponController>();
        WeaponVisuals = GetComponent<PlayerWeaponVisuals>();
        PlayerInteraction = GetComponent<PlayerInteraction>();

        // Wywo³anie muzyki przy starcie gry
        startGameAudioEvent?.Raise();

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
