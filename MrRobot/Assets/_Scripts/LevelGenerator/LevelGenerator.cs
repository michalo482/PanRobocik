using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // [SerializeField] private NavMeshSurface navMeshSurface;
    private LevelPartTemplates templates;
    public List<Enemy> enemyList;

    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;

    [SerializeField] private int levelSize;


    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("LevelParts").GetComponent<LevelPartTemplates>(); 
        enemyList = new List<Enemy>();       
        defaultSnapPoint = nextSnapPoint;
        InitializeGeneration();
    }

    private void Update()
    {
        if (generationOver)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            if (templates.generatedLevelParts.Count < levelSize)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if (generationOver == false)
            {
                FinishGeneration();
            }
        }
    }

    [ContextMenu("Restart generation")]
    private void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        templates.currentLevelParts = new List<Transform>(templates.levelParts);

        DestroyOldLevelPartsandEnemies();        
    }

    private void DestroyOldLevelPartsandEnemies()
    {
        foreach (Transform t in templates.generatedLevelParts)
        {
            Destroy(t.gameObject);
        }

        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }

        templates.generatedLevelParts = new List<Transform>();
        enemyList = new List<Enemy>();
    }    

    private void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();

        // navMeshSurface.BuildNavMesh();

        foreach (Enemy enemy in enemyList){
            enemy.transform.parent = null;
            enemy.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Create new level part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = null;

        if (generationOver)
            newPart = Instantiate(templates.lastLevelPart);
        else
            newPart = Instantiate(ChooseRandomPart());

        templates.generatedLevelParts.Add(newPart);

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();
        levelPartScript.FixedSnapTo(nextSnapPoint);

        if (levelPartScript.OverlapDetected())
        {
            InitializeGeneration();
            return;
        }

        nextSnapPoint = levelPartScript.GetExitPoint();
        enemyList.AddRange(levelPartScript.MyEnemies());
    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, templates.currentLevelParts.Count);

        Transform choosenPart = templates.currentLevelParts[randomIndex];

        templates.currentLevelParts.RemoveAt(randomIndex);

        return choosenPart;
    }
}
