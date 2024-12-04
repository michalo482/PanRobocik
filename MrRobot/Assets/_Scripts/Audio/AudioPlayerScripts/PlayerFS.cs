using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFS : MonoBehaviour  
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource leftSource;  // lewastopa
    [SerializeField] private AudioSource rightSource;      // prawastopa
    [SerializeField] private AudioSource leftrunSource;  // lewastopa bieg
    [SerializeField] private AudioSource rightrunSource;      // prawastopa bieg

    public void PlayleftSFX()
    {

        if (leftSource != null && !leftSource.isPlaying)
        {
            leftSource.pitch = Random.Range(0.95f, 1.05f);
            leftSource.Play();
        }
    }

    public void PlayrightSFX()
    {

        if (rightSource != null && !rightSource.isPlaying)
        {
            rightSource.pitch = Random.Range(0.95f, 1.05f);
            rightSource.Play();
        }
    }

    public void PlayleftrunSFX()
    {
        if (leftrunSource != null && !leftrunSource.isPlaying)
        {
            leftrunSource.pitch = Random.Range(0.95f, 1.05f);
            leftrunSource.Play();
        }
    }

    public void PlayrightrunSFX()
    {
        if (rightrunSource != null && !rightrunSource.isPlaying)
        {
            rightrunSource.pitch = Random.Range(0.95f, 1.05f);
            rightrunSource.Play();
        }
    }
}
