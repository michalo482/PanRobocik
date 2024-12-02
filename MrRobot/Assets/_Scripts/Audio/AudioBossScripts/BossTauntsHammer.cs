using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossTauntsHammer : MonoBehaviour  
{

    [Header("Audio Sources")]
    [SerializeField] private AudioSource tauntSource;  
    [SerializeField] private AudioSource roarrSource;     

 

private int animationCounter = 0; 

public void PlayTauntSFX()
{
    if (animationCounter == 0) 
    {
        if (tauntSource != null && !tauntSource.isPlaying)
        {
            tauntSource.Play(); 
        }
    }

    animationCounter++;

    if (animationCounter >= 4) 
    {
        animationCounter = 0;
    }
}

public void PlayRoarrSFX()
{
    if (roarrSource != null && !roarrSource.isPlaying)
    {
        roarrSource.Play();
    }
}

}