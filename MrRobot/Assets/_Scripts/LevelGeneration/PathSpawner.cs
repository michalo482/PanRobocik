using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{

    public int pathDirection;

    // private float minX = 50;
    // private float maxX = 250;
    // private float minZ = 50;
    // private float maxZ = 250;

    private PathTemplates templates;
    private int rand;
    public bool spawned = false;

    //public GameObject pathPieces;
    


    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Paths").GetComponent<PathTemplates>();        

        Invoke("Spawn", 0.4f);
    }

    // Update is called once per frame
    void Spawn()
    {
        
        if(spawned == false && templates.mapParts.Count < 10){
            if(pathDirection == 1){
                rand = Random.Range(0, templates.bottomPath.Length); 
                GameObject instance = (GameObject)Instantiate(templates.bottomPath[rand], transform.position, Quaternion.identity);
                instance.transform.parent = transform.parent;          
            } else if(pathDirection == 2){
                rand = Random.Range(0, templates.topPath.Length);
                GameObject instance = (GameObject)Instantiate(templates.topPath[rand], transform.position, Quaternion.identity);
                instance.transform.parent = transform.parent;           
            } else if(pathDirection == 3){
                rand = Random.Range(0, templates.leftPath.Length);
                GameObject instance = (GameObject)Instantiate(templates.leftPath[rand], transform.position, Quaternion.identity);
                instance.transform.parent = transform.parent;           
            } else if(pathDirection == 4){
                rand = Random.Range(0, templates.rightPath.Length);
                GameObject instance = (GameObject)Instantiate(templates.rightPath[rand], transform.position, Quaternion.identity);
                instance.transform.parent = transform.parent;           
            }
              
        }
        spawned = true;        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("SpawnPoint")){
            if(other.GetComponent<PathSpawner>().spawned == false && spawned == false && templates.mapParts.Count < 10){
                GameObject instance = (GameObject)Instantiate(templates.closingPath, transform.position, Quaternion.identity);
                instance.transform.parent = transform.parent;
                
            }
            spawned = true;
        }        
    }
}
