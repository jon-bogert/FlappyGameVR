using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] Transform worldTransform;
    [SerializeField] int maxHeight = 10;
    [SerializeField] int minHeight = -10;

    Player player;
    World world;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        world = FindObjectOfType<World>();
        
        NewObstacle();
        //StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void NewObstacle()
    {
        int spawnHeight = Random.Range(minHeight, maxHeight);
        Vector3 spawnPosition = new Vector3 (transform.position.x, transform.position.y + spawnHeight, transform.position.z);
        GameObject newObstacle  = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        world.AddObstacle(newObstacle);
        //world.ListAllObjects();
    }
    
    // IEnumerator SpawnTimer()
    // {
    //     //Declare a yield instruction.
    //     WaitForSeconds wait = new WaitForSeconds(3);
    //     while (!player.GetIsDead())
    //     {
    //         NewObstacle();
    //         Debug.Log("BAM");
    //         yield return wait;
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spawnable")
        {
            NewObstacle(); //TODO check type and spawn correct GameObject
        }
    }
}
