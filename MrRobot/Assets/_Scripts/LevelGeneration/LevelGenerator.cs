using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;


    private List<Enemy> enemyList;

    [SerializeField] private NavMeshSurface navMeshSurface;
    
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private List<Transform> levelParts;
    private List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();

    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;

    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        enemyList = new List<Enemy>();
        defaultSnapPoint = nextSnapPoint;
        //generatedLevelParts = new List<Transform>();
        //currentLevelParts = new List<Transform>(levelParts);
        //InitializeGeneration();
    }

    private void Update()
    {
        if (generationOver)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            if(currentLevelParts.Count > 0)
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
    public void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);
        DestroyOldLevelParts();
    }

    private void DestroyOldLevelParts()
    {

        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }

        foreach (Transform t in generatedLevelParts)
        {
            Destroy(t.gameObject);
        }

        //generatedLevelParts.Clear();
        generatedLevelParts = new List<Transform>();
        enemyList = new List<Enemy>();
    }

    private void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();

        navMeshSurface.BuildNavMesh();
        foreach(Enemy enemy in enemyList)
        {
            enemy.transform.parent = null;
            enemy.gameObject.SetActive(true);
        }

        MissionManager.instance.StartMission();
    }

    [ContextMenu("Create new level part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = null;
        if ((generationOver))
        {
            newPart = Instantiate(lastLevelPart);
        }
        else
        {
            newPart = Instantiate(ChooseRandomPart());
        }

        generatedLevelParts.Add(newPart);

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        if(levelPartScript.IntersectionDetected())
        {
            Debug.LogWarning("Sie nalozyly");
            InitializeGeneration();
            return;
        }
        nextSnapPoint = levelPartScript.GetExitPoint();
        enemyList.AddRange(levelPartScript.MyEnemies());
    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);

        Transform chosenPart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);

        return chosenPart;
    }

    public List<Enemy> GetEnemyList()
    {
        return enemyList;
    }
}
