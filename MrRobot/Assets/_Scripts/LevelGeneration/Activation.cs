using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    [Header("Activation Settings")]
    [SerializeField] public float activationDistance = 90f;
    [SerializeField] public float enemyActivationDistance = 40f;

    private LevelPartTemplates levelPartTemplates;
    private LevelGenerator levelGenerator;
    private Transform playerTransform;

    private void Start()
    {
        levelPartTemplates = GameObject.FindGameObjectWithTag("LevelParts").GetComponent<LevelPartTemplates>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();

        playerTransform = transform;
    }

    private void Update()
    {
        ActivateLevelParts();
        ActivateEnemies();
    }

    private void ActivateLevelParts()
    {
        foreach (Transform levelPart in levelPartTemplates.generatedLevelParts)
        {
            if (levelPart == null) continue;
            
            float distanceToPlayer = Vector3.Distance(levelPart.position, playerTransform.position);
            levelPart.gameObject.SetActive(distanceToPlayer < activationDistance);
        }
    }

    private void ActivateEnemies()
    {
        foreach (Enemy enemy in levelGenerator.enemyList)
        {
            if (enemy == null) continue;

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, playerTransform.position);
            enemy.gameObject.SetActive(distanceToPlayer < enemyActivationDistance);
        }
    }
}
