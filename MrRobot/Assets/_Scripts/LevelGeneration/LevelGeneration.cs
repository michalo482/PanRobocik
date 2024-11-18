using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject start;
    public Transform[] startingPositions;
    public GameObject[] desert;
    public Transform[] desertPositions;

    public GameObject desertBag;
    public GameObject generationBag;
    
    private Rigidbody rbGO;
    

    //int worldSizeX = 7;
    //int worldSizeZ = 7;
    //int gridOffset = 50;

    // Start is called before the first frame update
    void Start()
    {
        
        int startPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[startPos].position;
        GameObject instance = (GameObject)Instantiate(start, transform.position, Quaternion.identity);
        instance.transform.parent = generationBag.transform;        

        rbGO = startingPositions[startPos].GetComponent<Rigidbody>();
        rbGO.Sleep();


        Invoke("Fill", 1.5f);
        
    }

    void Fill()
    {
        for(int x = 0; x < desertPositions.Length; x++){
            if(desertPositions[x] == null){    
                
            } else {
                transform.position = desertPositions[x].position;
                int rand = Random.Range(0, desert.Length);
                GameObject instance = (GameObject)Instantiate(desert[rand], transform.position, Quaternion.identity);
                instance.transform.parent = desertBag.transform;

                rbGO = desertPositions[x].GetComponent<Rigidbody>();
                rbGO.Sleep();
            }
            
        }
        
        /*for(int x = 0; x < worldSizeX; x++){
            for(int z = 0; z < worldSizeZ; z++){
                Vector3 pos = new Vector3(x*gridOffset, 0, z*gridOffset);
                Instantiate(desert[rand], pos, Quaternion.identity);                
            }            
        }*/
    }    
}
