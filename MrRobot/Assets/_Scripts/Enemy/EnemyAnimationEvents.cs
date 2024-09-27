using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    public void AnimationTrigger() => _enemy.AnimationTrigger();

    public void StartManualMovement() => _enemy.ActivateManualMovement(true);
    public void StopManualMovement() => _enemy.ActivateManualMovement(false);
    public void StartManualRotation() => _enemy.ActivateManualRotation(true);
    public void StopManualRotation() => _enemy.ActivateManualRotation(false);
    public void AbilityEvent() => _enemy.AbilityTrigger();
}
