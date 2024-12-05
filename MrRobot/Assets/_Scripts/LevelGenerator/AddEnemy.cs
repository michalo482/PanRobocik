using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemy : MonoBehaviour
{
    private LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        levelGenerator.enemyList.Add(this.gameObject);
    }

}
