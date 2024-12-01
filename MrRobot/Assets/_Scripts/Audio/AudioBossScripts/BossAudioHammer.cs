using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossAudioHammer : MonoBehaviour  
{

    [Header("Audio Sources")]
    [SerializeField] private AudioSource hammerSource;  //zwykle uderzenie
    [SerializeField] private AudioSource hammerSource2; // zwykle uderzenie2
    [SerializeField] private AudioSource hammerSource3;      //ulti
    [SerializeField] private AudioSource ultihammerSource;    //grom
    [SerializeField] private AudioSource jumpAttackSource;      // wyskok   
    [SerializeField] private AudioSource jumpAttackSource2;      //spadek bosa
    [SerializeField] private AudioSource jumpAttackSource3;      //spadek m³ota


public void PlayHammerAttackSFX()
{
    if (hammerSource != null && !hammerSource.isPlaying)
    {
        hammerSource.Play();
    }
}

public void PlayHammerAttack2SFX()
{
    if (hammerSource2 != null && !hammerSource2.isPlaying)
    {
        hammerSource2.Play();
    }
}
public void PlayHammerAttack3SFX()
{
    if (hammerSource3 != null && !hammerSource3.isPlaying)
    {
        hammerSource3.Play();
    }
}

public void PlayUltiHammerSFX()
{
    if (ultihammerSource != null && !ultihammerSource.isPlaying)
    {
        ultihammerSource.Play();
    }
}
public void PlayJumpAttackSFX()
{
    if (jumpAttackSource != null && !jumpAttackSource.isPlaying)
    {
        jumpAttackSource.Play();
    }
}
public void PlayJumpAttack2SFX()
{
    if (jumpAttackSource2 != null && !jumpAttackSource2.isPlaying)
    {
        jumpAttackSource2.Play();
    }
}
public void PlayJumpAttack3SFX()
{
    if (jumpAttackSource3 != null && !jumpAttackSource3.isPlaying)
    {
        jumpAttackSource3.Play();
    }
}
}