using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnCollisionEnter(Collision other)
    {

        CreateImpactFX(other);
        ReturnBulletToPool();

        Player player = other.gameObject.GetComponentInParent<Player>();

        if (player != null)
        {
            Debug.Log("Shot the player");
        }
    }
}
