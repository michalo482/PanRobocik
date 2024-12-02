using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyDynamicStats", menuName = "Enemy/DynamicRangeStats")]
public class EnemyDynamicStats : ScriptableObject
{
    [Header("Grenade")]
    [Range(1f, 100f)] 
        [Tooltip("Czas odnowienia rzutu granatem w sekundach. Ustaw od 1 do 100.")]

    public float grenadeCooldown = 5f;  

}
