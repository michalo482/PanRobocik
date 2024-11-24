using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEndTrigger : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != player)
        {
            return;
        }
        if(MissionManager.instance.MissionCompleted())
            Debug.Log("Level completed");
    }
}
