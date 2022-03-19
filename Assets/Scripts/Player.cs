using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    [SerializeField] MeshRenderer deathColor;
    [SerializeField] PhysicsHand phLeft;
    [SerializeField] PhysicsHand phRight;
    [SerializeField] ActionBasedController leftController;
    [SerializeField] ActionBasedController rightController;

    [Space]
    [Header("Glide Mode")]
    [SerializeField] bool glideMode = false;
    [SerializeField] float glideModeDuration = 15f;
    [SerializeField] MeshRenderer engineMeshLeft;
    [SerializeField] MeshRenderer engineMeshRight;

    [Space]
    [Header("Shield")]
    [SerializeField] bool sheildActive = false;
    [SerializeField] MeshRenderer shieldMesh;

    GameData gameData;
    SceneLoader sceneLoader;
    World world;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        world = FindObjectOfType<World>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (leftController.selectAction.action.ReadValue<float>() > 0.01f)
        shieldMesh.enabled = (sheildActive); // TODO - Remove from Update
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit");
        if (collision.gameObject.tag == "Deadly" && !sheildActive)
        {
            world.StopMovement();
            deathColor.enabled = true;
            phLeft.DisableFlight();
            phRight.DisableFlight();
            isDead = true;
            //Debug.Log("Dead");
            StartCoroutine(sceneLoader.ResetGame());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint" && !isDead)
        {
            gameData.AddToScore();
        }
        if (other.gameObject.tag == "Item" && !isDead)
        {
            StartCoroutine(GlideModeCoroutine()); // TODO - DO Better Check than "Item" tag
            //Debug.Log("Item Triggered");
        }
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public bool GetGlideMode()
    {
        return glideMode;
    }

    public bool GetSheildActive()
    {
        return sheildActive;
    }

    public IEnumerator GlideModeCoroutine()
    {
        glideMode = true;
        UpdateEngineMesh();
        yield return new WaitForSeconds(15);
        glideMode = false;
        UpdateEngineMesh();
    }

    void UpdateEngineMesh()
    {
        engineMeshLeft.enabled = glideMode;
        engineMeshRight.enabled = glideMode;
    }
}
