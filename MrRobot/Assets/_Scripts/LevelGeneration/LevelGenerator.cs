using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;


    [Header("Level Generation Settings")]
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private SnapPoint nextSnapPoint;
    [SerializeField] private int levelSize = 5;
    [SerializeField] private float generationCooldown = 1f;
    [SerializeField] private GameObject[] blockPrefabs;

    private LevelPartTemplates templates;
    private Activation playerActivation;
    private SnapPoint defaultSnapPoint;

    private float cooldownTimer;
    private bool isGenerationComplete = true;

    private List<Transform> usedSnapPoints = new List<Transform>();
    public List<Transform> availableSnapPoints = new List<Transform>();
    public List<Enemy> enemyList = new List<Enemy>();


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("LevelParts").GetComponent<LevelPartTemplates>();
        playerActivation = GameObject.FindGameObjectWithTag("Player").GetComponent<Activation>();

        defaultSnapPoint = nextSnapPoint;
        
        usedSnapPoints.Add(nextSnapPoint.transform);
    }

    private void Update()
    {
        if (isGenerationComplete)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            if (templates.generatedLevelParts.Count < levelSize)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if (!isGenerationComplete)
            {
                FinishGeneration();
            }
        }
    }

    public void InitializeGeneration()
    {
        usedSnapPoints.Clear();
        availableSnapPoints.Clear();
        
        nextSnapPoint = defaultSnapPoint;
        usedSnapPoints.Add(nextSnapPoint.transform);

        isGenerationComplete = false;
        templates.activeLevelParts = new List<Transform>(templates.availableLevelParts);

        DestroyOldLevelPartsAndEnemies();
    }

    private void DestroyOldLevelPartsAndEnemies()
    {
        // Clean up previously generated level parts
        foreach (Transform part in templates.generatedLevelParts)
        {
            Destroy(part.gameObject);
        }

        // Clear enemy list and deactivate enemies
        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        
        templates.generatedLevelParts = new List<Transform>();
        
        enemyList = new List<Enemy>();
    }    

    private void FinishGeneration()
    {
        isGenerationComplete = true;
        GenerateNextLevelPart();

        // Rebuild NavMesh for pathfinding
        navMeshSurface.BuildNavMesh();

        // Activate all enemies
        foreach (Enemy enemy in enemyList)
        {
            enemy.transform.parent = null;
            enemy.gameObject.SetActive(true);
        }

        MissionManager.instance.StartMission();
    }

    private void GenerateNextLevelPart()
    {
        Transform newPart;

        if (isGenerationComplete)
        {
            // Generate the last part of the level
            newPart = Instantiate(templates.lastLevelPart);
            templates.generatedLevelParts.Add(newPart);

            var levelPartScript = newPart.GetComponent<LevelPart>();
            levelPartScript.FixedSnapTo(nextSnapPoint);

            if (levelPartScript.OverlapDetected())
            {
                InitializeGeneration();
                return;
            }

            nextSnapPoint = levelPartScript.GetExitPoint();
            RemoveNullSnapPoints();
            CompareSnapPoints();

            // Generate blocks at unused snap points
            foreach (Transform snap in availableSnapPoints)
            {
                var snapPoint = snap.GetComponent<SnapPoint>();
                if (snapPoint.snapPointType == SnapPointType.ENTER)
                {
                    continue;
                }
                else
                {
                    int randomIndex = Random.Range(0, blockPrefabs.Length);
                    Instantiate(blockPrefabs[randomIndex], snap.position, snap.rotation);
                }
            }
        }
        else
        {
            // Generate a random level part
            newPart = Instantiate(ChooseRandomPart());
            templates.generatedLevelParts.Add(newPart);

            var levelPartScript = newPart.GetComponent<LevelPart>();
            levelPartScript.FixedSnapTo(nextSnapPoint);

            if (levelPartScript.OverlapDetected())
            {
                InitializeGeneration();
                return;
            }

            nextSnapPoint = levelPartScript.GetExitPoint();
            usedSnapPoints.Add(nextSnapPoint.transform);
            enemyList.AddRange(levelPartScript.MyEnemies());
        }
    }
  

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, templates.activeLevelParts.Count);
        Transform selectedPart = templates.activeLevelParts[randomIndex];

        templates.activeLevelParts.RemoveAt(randomIndex);

        return selectedPart;
    }

    private void RemoveNullSnapPoints()
    {
        // Remove any null entries in available snap points list
        for (int i = 0; i < availableSnapPoints.Count; i++)
        {
            if (availableSnapPoints[i] == null)
            {
                availableSnapPoints.RemoveAt(i);
                i--;
            }
        }
    }

    private void CompareSnapPoints()
    {
        // Remove used snap points from the available list
        for (int i = 0; i < availableSnapPoints.Count; i++)
        {
            for (int j = 0; j < usedSnapPoints.Count; j++)
            {
                if (availableSnapPoints[i] == usedSnapPoints[j])
                {
                    availableSnapPoints.RemoveAt(i);
                    i = 0;
                    j = 0;
                }
            }
        }
    }

    public List<Enemy> GetEnemyList()
    {
        return enemyList;
    }

}
