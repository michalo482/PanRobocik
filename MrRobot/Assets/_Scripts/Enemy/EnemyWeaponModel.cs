using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponModel : MonoBehaviour
{
   public EnemyMeleeWeaponType weaponType;
   public AnimatorOverrideController OverrideController;
   public EnemyMeleeWeaponData weaponData;

   [SerializeField] private GameObject[] traileEffects;

    [Header("Damage Attribute")]
    public Transform[] damagePoints;
    public float attackRadius;

    [ContextMenu("Assign damage points transforms")]
    private void GetDamagePoints()
    {
        damagePoints = new Transform[traileEffects.Length];
        for (int i = 0; i < traileEffects.Length; i++)
        {
            damagePoints[i] = traileEffects[i].transform;
        }
    }

   public void EnableTrailEffect(bool enable)
   {
      foreach (var vEffect in traileEffects)
      {
         vEffect.SetActive(enable);
      }
   }

    private void OnDrawGizmos()
    {
        if(damagePoints.Length > 0)
        {
            foreach(var point in damagePoints)
            {
                Gizmos.DrawWireSphere(point.position, attackRadius);
            }
        }
    }

}
