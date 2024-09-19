using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HangType
{
    LowBackHang,
    BackHang,
    SideHang
}

public class BackupWeaponModel : MonoBehaviour
{
    public WeaponType WeaponType;
    [SerializeField] private HangType _hangType;

    public bool HangTypeIs(HangType hangType) => _hangType == hangType;

    public void Activate(bool activated) => gameObject.SetActive(activated);

}
