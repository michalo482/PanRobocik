using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    private PathTemplates templates;
    private float distance = 90f;
    private LevelEnemyList enemyList;
    private float distanceEnemy = 40f;

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Paths").GetComponent<PathTemplates>();
        enemyList = GameObject.FindGameObjectWithTag("Enemy").GetComponent<LevelEnemyList>();

    }

    // Update is called once per frame
    void Update()
    {
        for(int x = 0 ; x < templates.mapParts.Count; x++){
            if (Vector3.Distance (templates.mapParts[x].transform.position, transform.position) < distance) {
                templates.mapParts[x].SetActive(true);
            } else {
                templates.mapParts[x].SetActive(false);
            }
        }

        for(int x = 0 ; x < enemyList.enemies.Count; x++){
            if (Vector3.Distance (enemyList.enemies[x].transform.position, transform.position) < distanceEnemy) {
                enemyList.enemies[x].SetActive(true);
            } else {
                enemyList.enemies[x].SetActive(false);
            }
        }
    }
}
