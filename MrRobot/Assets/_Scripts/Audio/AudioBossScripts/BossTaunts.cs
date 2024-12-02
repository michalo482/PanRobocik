using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossTaunts : MonoBehaviour  
{

    [Header("Audio Sources")]
    [SerializeField] private AudioSource laughSource;  
    [SerializeField] private AudioSource roarSource;     

 

private int animationCounter = 0; 

public void PlayLaughSFX()
{
    if (animationCounter == 0) 
    {
        if (laughSource != null && !laughSource.isPlaying)
        {
            laughSource.Play(); 
        }
    }

    animationCounter++;

    if (animationCounter >= 4) 
    {
        animationCounter = 0;
    }
}

public void PlayRoarSFX()
{
    if (roarSource != null && !roarSource.isPlaying)
    {
        roarSource.Play();
    }
}

}