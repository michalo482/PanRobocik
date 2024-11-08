using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponAudioData", menuName = "Audio/WeaponAudioData")]
public class WeaponAudioData : ScriptableObject
{
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptyMagazineSound;

}
