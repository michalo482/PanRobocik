using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyMeleeDynamicStats", menuName = "Enemy/DynamicMeleeStats")]
public class EnemyMeleeDynamicStats : ScriptableObject
{
    [Header("Axe")]
    [Range(1f, 100f)] 
        [Tooltip("Czas odnowienia rzutu toporem w sekundach. Ustaw od 1 do 100.")]

    public float axeCooldown = 5f;  

}
