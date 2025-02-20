using System.Collections.Generic;
using UnityEngine;

public class LevelPartTemplates : MonoBehaviour
{
    [Header("Level Part Settings")]
    [Tooltip("Finish parts of the level.")]
    public List<Transform> lastLevelPart = new List<Transform>();

    [Tooltip("All available level parts for spawning.")]
    public List<Transform> availableLevelParts = new List<Transform>();

    [Tooltip("Parts available for this generation.")]
    public List<Transform> activeLevelParts = new List<Transform>();

    [Tooltip("Generated level parts during gameplay.")]
    public List<Transform> generatedLevelParts = new List<Transform>();

    [Tooltip("Special mission parts")]
    public List<Transform> specialLevelParts = new List<Transform>();
}
