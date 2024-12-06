using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSnap : MonoBehaviour
{
    private LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        levelGenerator.snapAll.Add(this.transform);
    }
}
