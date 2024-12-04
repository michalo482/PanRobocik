using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyRangeWeaponHoldType
{
    Common,
    LowHold,
    HighHold
}
public class EnemyRangeWeaponModel : MonoBehaviour
{
    public Transform gunPoint;
    [Space]
    public EnemyRangeWeaponType weaponType;
    public EnemyRangeWeaponHoldType weaponHoldType;

    public Transform leftHandTarget;
    public Transform leftElbowTarget;

    
}
