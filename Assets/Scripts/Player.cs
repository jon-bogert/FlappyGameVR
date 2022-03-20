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
    [SerializeField] Collider sheildCollider;
    [SerializeField] Collider playerCollider;

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
        else if (collision.gameObject.tag == "Deadly" && sheildActive)
        {
            //Debug.Log("Collision Enter");
            world.StopMovement();
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Deadly" && !isDead && world.GetCurrentMovement() == 0f)
        {
            //Debug.Log("Collision Exit");
            world.StartMovement();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint" && !isDead)
        {
            gameData.AddToScore();
        }

        else if (other.gameObject.tag == "Item Glide" && !isDead)
        {
            StartCoroutine(GlideModeCoroutine());
        }
        else if (other.gameObject.tag == "Item Shield" && !isDead)
        {
            StartCoroutine(ShieldModeCoroutine());
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

    IEnumerator GlideModeCoroutine()
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

    IEnumerator ShieldModeCoroutine()
    {
        sheildActive = true;
        UpdateShieldMesh();
        yield return new WaitForSeconds(15);
        sheildActive = false;
        UpdateShieldMesh();
    }

    void UpdateShieldMesh()
    {
        shieldMesh.enabled = (sheildActive);
        sheildCollider.enabled = (sheildActive);
        playerCollider.enabled = (!sheildActive);
    }
}
