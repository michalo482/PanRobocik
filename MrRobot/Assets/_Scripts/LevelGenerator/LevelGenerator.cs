using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshs;
    private LevelPartTemplates templates;
    public List<GameObject> enemySpawnerList;
    public List<GameObject> enemyList;

    private Activation playerActivation;

    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;

    [SerializeField] private int levelSize;


    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;


    public List<Transform> snapUsed;
    public List<Transform> snapAll;
    private SnapPoint snapPoint;

    public GameObject[] block;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("LevelParts").GetComponent<LevelPartTemplates>();         
        enemyList = new List<GameObject>(); 
        
        playerActivation = GameObject.FindGameObjectWithTag("Player").GetComponent<Activation>();        
        
        defaultSnapPoint = nextSnapPoint;

        snapUsed.Add(nextSnapPoint.transform);

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

    private void InitializeGeneration()
    {
        snapUsed = new List<Transform>();
        nextSnapPoint = defaultSnapPoint;
        snapUsed.Add(nextSnapPoint.transform);
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

        foreach (GameObject enemySpawner in enemySpawnerList)
        {
            Destroy(enemySpawner.gameObject);
        }

        templates.generatedLevelParts = new List<Transform>();
        enemySpawnerList = new List<GameObject>();
    }    

    private void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();

        // foreach (Enemy enemy in enemyList){
        //     enemy.transform.parent = null;
        //     enemy.gameObject.SetActive(true);
        // }
    }

    private void GenerateNextLevelPart()
    {
        Transform newPart = null;

        if (generationOver)
        {
            newPart = Instantiate(templates.lastLevelPart);
            
            newPart.rotation = nextSnapPoint.transform.rotation;
            newPart.rotation = Quaternion.Euler(newPart.rotation.eulerAngles.x, newPart.rotation.eulerAngles.y + 180f, newPart.rotation.eulerAngles.z);
            
            newPart.position = nextSnapPoint.transform.position;
            newPart.position += newPart.transform.forward * -5f;

            templates.generatedLevelParts.Add(newPart);

            LevelPart levelPartScript = newPart.GetComponent<LevelPart>();
            levelPartScript.FixedSnapTo(nextSnapPoint);

            if (levelPartScript.OverlapDetected())
            {
                InitializeGeneration();
                return;
            }

            nextSnapPoint = levelPartScript.GetExitPoint();
            //snapUsed.Add(nextSnapPoint.transform);
            
            StartCoroutine(RebuildNavMeshAsync());
            
            foreach (GameObject enemySpawner in enemySpawnerList){
                if (enemySpawner == null){

                } else {
                    EnemySpawner spawn = enemySpawner.GetComponent<EnemySpawner>();
                    spawn.Spawn();

                }
            }
            playerActivation.enabled = true;

            RemoveNullSnap();            

            SnapCompare();

            foreach (Transform snap in snapAll){
                snapPoint = snap.GetComponent<SnapPoint>();

                if(snapPoint.pointType == 0){
                    
                } else {
                    int rand = Random.Range(0, block.Length);
                    Instantiate(block[rand], snap.position, snap.rotation);
                }
                            
                
                
                
            }         
        }
        else
        {
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
            snapUsed.Add(nextSnapPoint.transform);
        }
      
    }

    private IEnumerator RebuildNavMeshAsync()
    {
        if (navMeshs.navMeshData == null)
        {
            navMeshs.navMeshData = new NavMeshData();
            NavMesh.AddNavMeshData(navMeshs.navMeshData);
        }

        var operation = navMeshs.UpdateNavMesh(navMeshs.navMeshData);

        while (!operation.isDone)
        {
            yield return null; 
        }
    }



    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, templates.currentLevelParts.Count);

        Transform choosenPart = templates.currentLevelParts[randomIndex];

        templates.currentLevelParts.RemoveAt(randomIndex);

        return choosenPart;
    }

    private void RemoveNullSnap()
    {
        for(int x = 0; x<snapAll.Count; x++){          
            
            if (snapAll[x] == null){
                snapAll.RemoveAt(x);
                x--;
            }

        }         
    }

    private void SnapCompare()
    {
        for(int x = 0; x < snapAll.Count; x++){
                for(int y = 0; y < snapUsed.Count; y++){
                    if(snapAll[x] == snapUsed[y]){
                        snapAll.RemoveAt(x);
                        x=0;
                        y=0;
                    }
                }
            }
    }

}
