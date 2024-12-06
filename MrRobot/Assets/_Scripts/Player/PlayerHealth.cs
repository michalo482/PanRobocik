using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthController
{
    private Player player;
    public bool IsDead { get; private set; }

    private bool hasPlayedRepairSound = false;  // Flaga, aby upewni� si�, �e d�wi�k b�dzie odtworzony tylko raz

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    public override void ReduceHealth(int damage)
    {
        if (IsDead) return;  // Je�li gracz jest martwy, nie wykonujemy dalszych operacji.

        base.ReduceHealth(damage);
        float healthPercentage = (float)currentHealth / maxHealth;

        // Sprawdzamy, czy zdrowie spad�o poni�ej 50% i d�wi�k naprawy nie by� jeszcze odtworzony
        if (healthPercentage < 0.5f && !hasPlayedRepairSound)
        {
            AudioManager.Instance.PlayHealthSound("repair");
            hasPlayedRepairSound = true;  // Ustawiamy flag�, �eby d�wi�k nie by� odtwarzany ponownie
        }

        if (ShouldDie())
        {
            ControlsManager.Instance.SwitchToUIControls();
            Die();
            AudioManager.Instance.PlayHealthSound("dead");
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
