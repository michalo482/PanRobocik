using System.Collections;
using UnityEngine;

public class ZoneLimitation : MonoBehaviour
{
    private ParticleSystem[] particleLines;
    private BoxCollider zoneCollider;

    private void Start()
    {
        InitializeComponents();
        ActivateLimitZone(false);
    }

    private void InitializeComponents()
    {
        GetComponent<MeshRenderer>().enabled = false;
        zoneCollider = GetComponent<BoxCollider>();
        particleLines = GetComponentsInChildren<ParticleSystem>();
    }

    private void ActivateLimitZone(bool isActive)
    {
        foreach (var line in particleLines)
        {
            if (isActive)
            {
                line.Play();
            }
            else
            {
                line.Stop();
            }
        }

        zoneCollider.isTrigger = !isActive;
    }

    private IEnumerator LimitActivationCoroutine()
    {
        ActivateLimitZone(true);
        yield return new WaitForSeconds(1);
        ActivateLimitZone(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LimitActivationCoroutine());
        Debug.Log("I'm picking up unusual sensor readings. Access is not recommended—there is a high risk of danger!!");
    }
}
