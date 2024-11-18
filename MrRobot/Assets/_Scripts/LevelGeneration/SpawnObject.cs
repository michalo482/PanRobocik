using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    //public NavMeshSurface navSurface;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, transform.rotation);
        instance.transform.parent = transform;

        //navSurface.BuildNavMesh();
    }
    
}
