using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] SpawnPoint.SpawnType spawnType = SpawnPoint.SpawnType.Wall;
    
    World world;
    Vector3 forward;
    
    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += world.GetForward() * Time.fixedDeltaTime;
    }

    public SpawnPoint.SpawnType GetSpawnType()
    {
        return spawnType;
    }
}
