using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DespawnPoint : MonoBehaviour
{
    private World world;

    private void Start()
    {
        world = FindObjectOfType <World>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spawnable" || other.gameObject.tag == "Wall")
        {
            world.RemoveObstacle(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
