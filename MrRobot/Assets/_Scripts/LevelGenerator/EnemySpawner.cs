using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{    
    public GameObject[] enemy;

    // Start is called before the first frame update
    // void Start()
    // {
    //        Invoke("Spawn", 3f);  
    // }

    public void Spawn(){
        int rand = Random.Range(0, enemy.Length);
        GameObject instance = (GameObject)Instantiate(enemy[rand], transform.position, transform.rotation * Quaternion.Euler (0f, 180f, 0f));
        //instance.transform.parent = transform.parent;
        // Instantiate(enemy[rand], transform.position, transform.rotation * Quaternion.Euler (0f, 180f, 0f));   
    }
}
