using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthController
{
    // Ta metoda jest wywo�ywana, gdy przeciwnik otrzymuje obra�enia
    public override void ReduceHealth(int damage)
    {
        base.ReduceHealth(damage); // Wywo�aj ReduceHealth z klasy bazowej
    }
}
