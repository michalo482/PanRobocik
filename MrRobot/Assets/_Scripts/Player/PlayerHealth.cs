using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthController
{

    private Player player;
    public bool IsDead {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }
    public override void ReduceHealth(int damage)
    {
        base.ReduceHealth(damage);

        if(ShouldDie())
        {
            Die();
        }

        UI.instance.inGameUI.UpdateHPBar(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;
        player.Animator.enabled = false;
        player.Ragdoll.RagdollActive(true);

        GameManager.Instance.GameOver();
    }
}
