using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnableObjects;

    private void Start()
    {
        SpawnRandomObject();
    }

    private void SpawnRandomObject()
    {
        if (spawnableObjects.Length == 0)
        {
            Debug.LogWarning("No objects assigned to spawn.");
            return;
        }

        int randomIndex = Random.Range(0, spawnableObjects.Length);
        GameObject instance = Instantiate(spawnableObjects[randomIndex], transform.position, transform.rotation);
        instance.transform.SetParent(transform);
    }
}
