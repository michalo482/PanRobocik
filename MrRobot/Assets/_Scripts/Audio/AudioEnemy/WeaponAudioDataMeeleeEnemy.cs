using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "WeaponAudioDataEnemy", menuName = "Audio/WeaponAudioDataMeeleeEnemy")]
public class WeaponAudioDataMeeleeEnemy : ScriptableObject
{
    public AudioClip attackSound;
    [Range(0f, 1f)] public float volume = 1.0f;  // Nowa zmienna do ustawiania g³oœnoœci
    public AudioClip AxeThrowSound;
     [Range(0f, 1f)] public float volume2 = 1.0f;
    public AudioClip AxeHitSound;
     [Range(0f, 1f)] public float volume3 = 1.0f;
    public AudioClip granadeSound;
     [Range(0f, 1f)] public float volume4 = 1.0f;


}
