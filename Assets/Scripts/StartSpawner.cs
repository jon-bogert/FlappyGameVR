using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] int maxHeight = 5;
    [SerializeField] int minHeight = -5;
    void Start()
    {
        //FindObjectOfType<SpawnPoint>().NewObstacle(new Vector3 (transform.position.x, transform.position.y, transform.position.z));
        int spawnHeight = Random.Range(minHeight, maxHeight);
        Vector3 spawnPosition = new Vector3 (transform.position.x, transform.position.y + spawnHeight, transform.position.z);
        GameObject newObstacle  = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        //world.AddObstacle(newObstacle);
    }
}
