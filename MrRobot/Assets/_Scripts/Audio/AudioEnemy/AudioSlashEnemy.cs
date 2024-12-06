using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSlashEnemy : MonoBehaviour
{
    [SerializeField] private AudioSource slashSource;
    [SerializeField] private AudioSource screamSource;
    [SerializeField] private AudioSource spinSource;
    [SerializeField] private AudioSource screamtauntSource;  
    [SerializeField] private AudioSource rollSource;
    [SerializeField] private AudioSource tauntmeeleSource;
    [SerializeField] private AudioSource boxattackSource;  

    public void PlayScreamTauntSFX()
    {
        if (screamtauntSource != null && !screamtauntSource.isPlaying)
        {
            screamtauntSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            screamtauntSource.Play();
        }
    }

    public void PlayBoxAttackSFX()
    {
        if (boxattackSource != null && !boxattackSource.isPlaying)
        {
            boxattackSource.pitch = Random.Range(0.85f, 1.1f);
            boxattackSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            boxattackSource.Play();
        }
    }

    public void PlayMeeleTauntSFX()
    {
        if (tauntmeeleSource != null && !tauntmeeleSource.isPlaying)
        {
            tauntmeeleSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            tauntmeeleSource.Play();
        }
    }

    public void PlayRollSFX()
    {
        if (rollSource != null && !rollSource.isPlaying)
        {
            rollSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            rollSource.Play();
        }
    }

    public void PlaySlashSFX()
    {
        if (slashSource != null && !slashSource.isPlaying)
        {
            slashSource.pitch = Random.Range(0.95f, 1.05f);
            slashSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            slashSource.Play();
        }
    }

    public void PlaySpinSFX()
    {
        if (spinSource != null && !spinSource.isPlaying)
        {
            spinSource.pitch = Random.Range(0.95f, 1.05f);
            spinSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            spinSource.Play();
        }
    }

    public void PlayScreamSFX()
    {
        if (screamSource != null && !screamSource.isPlaying)
        {
            screamSource.pitch = Random.Range(0.85f, 1.15f);
            screamSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            screamSource.Play();
        }
    }
}
