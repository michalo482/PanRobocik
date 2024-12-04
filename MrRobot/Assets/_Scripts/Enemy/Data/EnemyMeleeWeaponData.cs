using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Enemy Data/Melee Weapon Data")]
public class EnemyMeleeWeaponData : ScriptableObject
{
    public List<AttackDataEnemyMelee> attackData;
    public float turnSpeed = 10;
}
