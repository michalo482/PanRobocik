using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "WeaponAudioDataEnemy", menuName = "Audio/WeaponAudioDataMeeleeEnemy")]
public class WeaponAudioDataMeeleeEnemy : ScriptableObject
{
    public AudioClip attackSound;
    public AudioClip AxeThrowSound;
    public AudioClip AxeHitSound;
    public AudioClip granadeSound;


}
