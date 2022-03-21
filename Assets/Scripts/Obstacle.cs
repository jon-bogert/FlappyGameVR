using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 3f;

    [Header("Power Ups")]
    [SerializeField] GameObject glidePrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] int horizontalRange = 5;
    
    Player player;
    World world;

    Vector3 forward;

    void Start()
    {
        player = FindObjectOfType<Player>();
        world = FindObjectOfType<World>();
        //StartMovement();
        CheckPowerup();
    }

    // private void OnDestroy()
    // {
    //     world.RemoveObstacle(this.gameObject);
    // }

    void FixedUpdate()
    {
        // if (player.GetIsDead())
        // {
        //     StopMovement();
        // }
        transform.position += world.GetForward() * Time.fixedDeltaTime;
        // if (transform.position.z < -10)
        // {
        //     Destroy(this.gameObject);
        // }
        // else
        // {
        //     transform.position += forward * Time.fixedDeltaTime;
        // }
    }

    public void StopMovement()
    {
        forward = Vector3.zero;
    }

    public void StartMovement()
    {
        forward.Set(0f, 0f, -forwardSpeed);
    }

    void CheckPowerup()
    {
        int dice = Random.Range(1, 100);
        bool spawnItem = false;
        GameObject spawnPrefab = glidePrefab;
        if (!world.GetIfGlideInPlay() && dice >= 70)
        {
            spawnItem = true;
            spawnPrefab = glidePrefab;
            world.GlideInPlay();
        }
        else if (!world.getIfShieldInPlay() && dice <= 30)
        {
            spawnItem = true;
            spawnPrefab = shieldPrefab;
            world.ShieldInPlay();
        }
        if (spawnItem)
        {
            float hDisp = Random.Range(-horizontalRange, horizontalRange) * 1.75f;
            Vector3 spawnPos = new Vector3(transform.position.x + hDisp, transform.position.y, transform.position.z);
            //Debug.Log("Spawn Position X: " + spawnPos.x);
            GameObject newPowerup = Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
            newPowerup.transform.parent = gameObject.transform;
        }
    }
    
}
