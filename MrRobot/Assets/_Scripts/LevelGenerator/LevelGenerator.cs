using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private LevelPartTemplates templates;

    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;

    [SerializeField] private int levelSize;


    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("LevelParts").GetComponent<LevelPartTemplates>();
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
        // templates.currentLevelParts = new List<Transform>(templates.levelParts);

        DestroyOldLevelParts();
    }

    private void DestroyOldLevelParts()
    {
        foreach (Transform t in templates.generatedLevelParts)
        {
            Destroy(t.gameObject);
        }

        templates.generatedLevelParts = new List<Transform>();
    }

    private void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();
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
    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, templates.levelParts.Count);

        Transform choosenPart = templates.levelParts[randomIndex];

        // templates.currentLevelParts.RemoveAt(randomIndex);

        return choosenPart;
    }
}
