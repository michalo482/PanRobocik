using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneLimit : MonoBehaviour
{
    private ParticleSystem[] lines;
    private BoxCollider zone;

    private void Start(){
        GetComponent<MeshRenderer>().enabled = false;
        zone = GetComponent<BoxCollider>();
        lines = GetComponentsInChildren<ParticleSystem>();
        ActivateLimitZone(false);
    }

    private void ActivateLimitZone(bool activate){
        foreach(var line in lines){
            if (activate){
                line.Play();
            } else {
                line.Stop();
            }
        }

        zone.isTrigger = !activate;
    }

    IEnumerator LimitActivationCo(){
        ActivateLimitZone(true);
        yield return new WaitForSeconds(1);
        ActivateLimitZone(false);
    }

    private void OnTriggerEnter(Collider other){
        StartCoroutine(LimitActivationCo());
        Debug.Log("My sensors are going crazy, I think it's dangerous!");
    }
}
