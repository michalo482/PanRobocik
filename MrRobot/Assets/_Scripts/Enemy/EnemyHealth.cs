using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthController
{
    // Ta metoda jest wywo³ywana, gdy przeciwnik otrzymuje obra¿enia
    public override void ReduceHealth(int damage)
    {
        base.ReduceHealth(damage); // Wywo³aj ReduceHealth z klasy bazowej
    }
}
