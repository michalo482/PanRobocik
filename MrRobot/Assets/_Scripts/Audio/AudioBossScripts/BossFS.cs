using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFS : MonoBehaviour  
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource leftSource;  // lewastopa
    [SerializeField] private AudioSource rightSource;      // prawastopa
    [SerializeField] private AudioSource leftrunSource;  // lewastopa bieg
    [SerializeField] private AudioSource rightrunSource;      // prawastopa bieg
    [SerializeField] private AudioSource leftrun2Source;  // lewastopa bieg szybko
    [SerializeField] private AudioSource rightrun2Source;      // prawastopa bieg szybko

    public void PlayleftSFX()
    {
        if (leftSource != null && !leftSource.isPlaying)
        {
            leftSource.pitch = Random.Range(0.95f, 1.05f);  // Dodanie losowego pitcha
            leftSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            leftSource.Play();
        }
    }

    public void PlayrightSFX()
    {
        if (rightSource != null && !rightSource.isPlaying)
        {
            rightSource.pitch = Random.Range(0.95f, 1.05f);  // Dodanie losowego pitcha
            rightSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            rightSource.Play();
        }
    }

    public void PlayleftrunSFX()
    {
        if (leftrunSource != null && !leftrunSource.isPlaying)
        {
            leftrunSource.pitch = Random.Range(0.95f, 1.05f);  // Dodanie losowego pitcha
            leftrunSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            leftrunSource.Play();
        }
    }

    public void PlayrightrunSFX()
    {
        if (rightrunSource != null && !rightrunSource.isPlaying)
        {
            rightrunSource.pitch = Random.Range(0.95f, 1.05f);  // Dodanie losowego pitcha
            rightrunSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            rightrunSource.Play();
        }
    }

    public void Playleftrun2SFX()
    {
        if (leftrun2Source != null && !leftrun2Source.isPlaying)
        {
            leftrun2Source.pitch = Random.Range(0.95f, 1.05f);  // Dodanie losowego pitcha
            leftrun2Source.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            leftrun2Source.Play();
        }
    }

    public void Playrightrun2SFX()
    {
        if (rightrun2Source != null && !rightrun2Source.isPlaying)
        {
            rightrun2Source.pitch = Random.Range(0.95f, 1.05f);  // Dodanie losowego pitcha
            rightrun2Source.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);  // Ustawienie g³oœnoœci na podstawie SFX
            rightrun2Source.Play();
        }
    }
}
