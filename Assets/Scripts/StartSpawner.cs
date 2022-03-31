using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpawner : MonoBehaviour
{
    [SerializeField] bool tutorialMode = false;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] int maxHeight = 5;
    [SerializeField] int minHeight = -5;
    void Start()
    {
        int spawnHeight = Random.Range(minHeight, maxHeight);
        float xPos = transform.position.x;
        if (tutorialMode)
        {
            xPos = (FindObjectOfType<TutorialController>().GetTutorialIndex() == 1)
                ? transform.position.x + Random.Range(minHeight, maxHeight)
                : xPos;
        }

        Vector3 spawnPosition = new Vector3 (xPos, transform.position.y + spawnHeight, transform.position.z);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}
