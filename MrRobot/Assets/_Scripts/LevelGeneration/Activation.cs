using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    // private LevelPartTemplates templates;
    // private float distance = 90f;
    private bool active = false;
    private float distanceEnemy = 40f;

    // Start is called before the first frame update
    void Start()
    {
        // templates = GameObject.FindGameObjectWithTag("LevelParts").GetComponent<LevelPartTemplates>();
        // enemiesList = GameObject.FindGameObjectWithTag("Enemy").GetComponent<LevelEnemyList>();

    }

    // Update is called once per frame
    void Update()
    {
        // for(int x = 0 ; x < templates.generatedLevelParts.Count; x++){
        //     if (Vector3.Distance (templates.generatedLevelParts[x].transform.position, transform.position) < distance) {
        //         templates.generatedLevelParts[x].SetActive(true);
        //     } else {
        //         templates.generatedLevelParts[x].SetActive(false);
        //     }
        // }

        // for(int x = 0 ; x < enemyList.enemies.Count; x++){
        //     if (Vector3.Distance (enemyList.enemies[x].transform.position, transform.position) < distanceEnemy) {
        //         enemyList.enemies[x].SetActive(true);
        //         active = true;
        //     } else if (Vector3.Distance (enemyList.enemies[x].transform.position, transform.position) >= distanceEnemy && active == true){
        //         enemyList.enemies[x].SetActive(false);
        //         active = false;
        //     }
        // }
    }
}
