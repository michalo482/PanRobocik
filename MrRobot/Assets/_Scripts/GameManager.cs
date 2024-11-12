using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Settings")]
    public bool friendlyFire;

    private void Awake()
    {
        Instance = this;
    }
}
