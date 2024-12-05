using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemySpawner : MonoBehaviour
{
    private LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        levelGenerator.enemySpawnerList.Add(this.gameObject);
    }
}
