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
        leftSource.Play();
    }
}

public void PlayrightSFX()
{
    if (rightSource != null && !rightSource.isPlaying)
    {
        rightSource.Play();
    }
}
public void PlayleftrunSFX()
{
    if (leftrunSource != null && !leftrunSource.isPlaying)
    {
        leftrunSource.Play();
    }
}

public void PlayrightrunSFX()
{
    if (rightrunSource != null && !rightrunSource.isPlaying)
    {
        rightrunSource.Play();
    }
}
public void Playleftrun2SFX()
{
    if (leftrun2Source != null && !leftrun2Source.isPlaying)
    {
        leftrun2Source.Play();
    }
}

public void Playrightrun2SFX()
{
    if (rightrun2Source != null && !rightrun2Source.isPlaying)
    {
        rightrun2Source.Play();
    }
}
}