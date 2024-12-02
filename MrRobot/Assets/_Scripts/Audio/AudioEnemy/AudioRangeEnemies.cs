using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRangeEnemies : MonoBehaviour
{


    [SerializeField] private AudioSource throwgrenadeSource;

      public void PlayThrowGrenadeSFX()
    {

        if (throwgrenadeSource != null && !throwgrenadeSource.isPlaying)
        {
            throwgrenadeSource.pitch = Random.Range(0.95f, 1.05f);
            throwgrenadeSource.Play();
        }
    }
}
