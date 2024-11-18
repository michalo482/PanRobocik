using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemy;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, enemy.Length);
        Instantiate(enemy[rand], transform.position, transform.rotation * Quaternion.Euler (0f, 180f, 0f));        
    }
}
