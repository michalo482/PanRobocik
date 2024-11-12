using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> levelParts;
    [SerializeField] private SnapPoint nextSnapPoint;

    [ContextMenu("Create new level part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = Instantiate(ChooseRandomPart());
    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, levelParts.Count);

        Transform chosenPart = levelParts[randomIndex];

        levelParts.RemoveAt(randomIndex);

        return chosenPart;
    }
}
