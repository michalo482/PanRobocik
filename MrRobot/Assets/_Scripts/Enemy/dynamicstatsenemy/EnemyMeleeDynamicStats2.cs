using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyMeleeDynamicStats2", menuName = "Enemy/AggressionOfMeleeEnemy")]
public class EnemyMeleeDynamicStats2 : ScriptableObject
{
    [Header("Agresja")]
    [Range(1f, 10f)] 
    [Tooltip("Zasiêg agresji wroga Ustaw od 1 do 10 (zalecane 5).")]
    public float aggression = 5f; 
}
