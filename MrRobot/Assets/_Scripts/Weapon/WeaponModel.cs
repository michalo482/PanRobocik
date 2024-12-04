using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EquipType
{
    SideEquipAnimation,
    BackEquipAnimation
};

public enum HoldType
{
    CommonHold = 1,
    LowHold,
    HighHold
};

public class WeaponModel : MonoBehaviour
{
    public WeaponType WeaponType;
    [FormerlySerializedAs("equipType")] [FormerlySerializedAs("GrabType")] public EquipType equipAnimationType;
    public HoldType HoldType;

    public Transform GunPoint;
    public Transform HoldPoint;

}
