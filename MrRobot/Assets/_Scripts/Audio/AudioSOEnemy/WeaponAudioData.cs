using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "WeaponAudioData", menuName = "Audio/WeaponAudioData")]
public class WeaponAudioData : ScriptableObject
{
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptyMagazineSound;
    public AudioClip weaponSwitchSound;


}
