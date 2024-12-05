using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    private LevelPartTemplates templates;
    private float distance = 90f;
    private LevelGenerator levelGenerator;
    
    private float distanceEnemy = 40f;

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("LevelParts").GetComponent<LevelPartTemplates>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();

    }

    // Update is called once per frame
    void Update()
    {
        for(int x = 0 ; x < templates.generatedLevelParts.Count; x++){
            if (Vector3.Distance (templates.generatedLevelParts[x].transform.position, transform.position) < distance) {
                templates.generatedLevelParts[x].gameObject.SetActive(true);
            } else {
                templates.generatedLevelParts[x].gameObject.SetActive(false);
            }
        }

        for(int x = 0 ; x < levelGenerator.enemyList.Count; x++){
            if (Vector3.Distance (levelGenerator.enemyList[x].transform.position, transform.position) < distanceEnemy) {
                levelGenerator.enemyList[x].SetActive(true);
                
            } else {
                levelGenerator.enemyList[x].SetActive(false);
                
            }
        }
    }
}
