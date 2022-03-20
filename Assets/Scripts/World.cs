using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 1f;
    [SerializeField] int glideRespawnTime = 25;

    Vector3 forward;
    List<GameObject> obstacles = new List<GameObject>();
    bool glidePowerupInPlay = false;
    bool sheildPowerUpInPlay = false;

    void Start()
    {
        StartMovement();
    }
    void FixedUpdate()
    {
        transform.position += forward * Time.fixedDeltaTime;
    }

    public void StopMovement()
    {
        forward = Vector3.zero;
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<Obstacle>().StopMovement();
        }
    }
    public void StartMovement()
    {
        forward.Set(0f, 0f, -forwardSpeed);
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<Obstacle>().StartMovement();
        }
    }

    public Vector3 GetForward()
    {
        return Vector3.forward;
    }

    public float GetCurrentMovement()
    {
        return -forward.z;
    }
    
    public void AddObstacle(GameObject gameObject)
    {
        obstacles.Add(gameObject);
    }

    public void RemoveObstacle(GameObject gameObject)
    {
        obstacles.Remove(gameObject);
    }

    public void ListAllObjects() // For Debug
    {
        Debug.Log(obstacles.Count + " Obstacles Found");
    }

    public bool GetIfGlideInPlay()
    {
        return glidePowerupInPlay;
    }
    
    public void GlideInPlay()
    {
        glidePowerupInPlay = true;
        StartCoroutine(GlideRespawnTimer());
    }

    IEnumerator GlideRespawnTimer()
    {
        yield return new WaitForSeconds(glideRespawnTime);
        glidePowerupInPlay = false;
    }
}
