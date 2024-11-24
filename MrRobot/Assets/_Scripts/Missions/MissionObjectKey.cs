using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjectKey : MonoBehaviour
{
    private GameObject player;
    public static event Action OnKeyPickUp;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player)
        {
            return;
        }

        OnKeyPickUp?.Invoke();
        Destroy(gameObject);
    }
}
