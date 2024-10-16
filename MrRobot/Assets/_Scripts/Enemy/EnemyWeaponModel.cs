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
   

   public void EnableTrailEffect(bool enable)
   {
      foreach (var vEffect in traileEffects)
      {
         vEffect.SetActive(enable);
      }
   }
}
