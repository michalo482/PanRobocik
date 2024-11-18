using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemy : MonoBehaviour
{
    private LevelEnemyList enemyList;

    // Start is called before the first frame update
    void Start()
    {
        enemyList = GameObject.FindGameObjectWithTag("Enemy").GetComponent<LevelEnemyList>();
        enemyList.enemies.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
