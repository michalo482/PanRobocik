using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossAudio : MonoBehaviour  
{

    [Header("Audio Sources")]
    [SerializeField] private AudioSource flamethrowerSource;  // DŸwiêk miotacza ognia
    [SerializeField] private AudioSource flamweaponSource;      // piêœæ
    [SerializeField] private AudioSource flamweaponSource2;    // zamach ³apskiem
    [SerializeField] private AudioSource jumpAttackSource;
    [SerializeField] private AudioSource jumpAttackSource2;  

public void PlayFlamethrowerSFX()
{
    if (flamethrowerSource != null && !flamethrowerSource.isPlaying)
    {
        flamethrowerSource.Play();
    }
}

public void PlayWeaponSFX()
{
    if (flamweaponSource != null && !flamweaponSource.isPlaying)
    {
        flamweaponSource.Play();
    }
}

public void PlayWeaponSFX2()
{
    if (flamweaponSource2 != null && !flamweaponSource2.isPlaying)
    {
        flamweaponSource2.Play();
    }
}
public void PlayJumpAttackSFX()
{
    if (jumpAttackSource != null && !jumpAttackSource.isPlaying)
    {
        jumpAttackSource.Play();
    }
}
public void PlayJumpAttackSFX2()
{
    if (jumpAttackSource2 != null && !jumpAttackSource2.isPlaying)
    {
        jumpAttackSource2.Play();
    }
}

}