using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] MeshRenderer deathColor;
    [SerializeField] PhysicsHand phLeft;
    [SerializeField] PhysicsHand phRight;
    [SerializeField] ActionBasedController leftController;
    [SerializeField] ActionBasedController rightController;
    [SerializeField] InputActionReference pauseAction;

    [Space]
    [Header("Glide Mode")]
    [SerializeField] bool glideMode = false;
    [SerializeField] float glideModeDuration = 15f;
    [SerializeField] MeshRenderer engineMeshLeft;
    [SerializeField] MeshRenderer engineMeshRight;
    [SerializeField] Slider glideSlider;
    
    [Space]
    [Header("Shield")]
    [SerializeField] bool shieldActive = false;
    [SerializeField] MeshRenderer shieldMesh;
    [SerializeField] Collider shieldCollider;
    [SerializeField] Collider playerCollider;
    [SerializeField] InputActionReference activateShieldAction;
    [SerializeField] int shieldMax = 15;
    [SerializeField] int shieldMin = 5;
    [SerializeField] Slider shieldSlider;

    GameData gameData;
    SceneLoader sceneLoader;
    World world;
    bool isDead = false;
    int shieldValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        world = FindObjectOfType<World>();
        activateShieldAction.action.performed += ActivateShield;
        pauseAction.action.performed += PauseGame;
    }

    void OnDestroy()
    {
        activateShieldAction.action.performed -= ActivateShield;
        pauseAction.action.performed -= PauseGame;
    }

    // Update is called once per frame
    void Update()
    {
        //if (leftController.selectAction.action.ReadValue<float>() > 0.01f)

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit");
        if (collision.gameObject.tag == "Deadly" && !shieldActive)
        {
            world.StopMovement();
            deathColor.enabled = true;
            phLeft.DisableFlight();
            phRight.DisableFlight();
            isDead = true;
            //Debug.Log("Dead");
            StartCoroutine(ResetGame());
        }
        else if (collision.gameObject.tag == "Deadly" && shieldActive)
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
            if (shieldValue < shieldMax && !shieldActive) shieldValue++;
            UpdateShieldSlider();
        }

        else if (other.gameObject.tag == "Item Glide" && !isDead)
        {
            StartCoroutine(GlideModeCoroutine());
        }
        else if (other.gameObject.tag == "Item Shield" && !isDead)
        {
            shieldValue = shieldMax;
            UpdateShieldSlider();
        }
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(5);
        //Destroy(FindObjectOfType<GameData>());
        FindObjectOfType<GameData>().Reset();
        SceneManager.LoadScene(1);
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
        return shieldActive;
    }

    IEnumerator GlideModeCoroutine()
    {
        glideMode = true;
        UpdateEngineMesh();
        int duration = (int)glideModeDuration;
        glideSlider.value = duration;
        while (duration > 0)
        {
            yield return new WaitForSeconds(1);
            duration--;
            glideSlider.value = duration;
        }

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
        shieldActive = true;
        UpdateShieldMesh();
        while (shieldValue > 0)
        {
            yield return new WaitForSeconds(1);
            shieldValue--;
            UpdateShieldSlider();
        }
        shieldActive = false;
        UpdateShieldMesh();
    }

    void UpdateShieldMesh()
    {
        shieldMesh.enabled = (shieldActive);
        shieldCollider.enabled = (shieldActive);
        playerCollider.enabled = (!shieldActive);
    }

    void ActivateShield(InputAction.CallbackContext obj)
    {
        if (shieldActive || isDead || shieldValue < shieldMin) return;
        StartCoroutine(ShieldModeCoroutine());
    }

    void UpdateShieldSlider()
    {
        shieldSlider.value = shieldValue;
    }

    void PauseGame(InputAction.CallbackContext obj)
    {
        FindObjectOfType<PauseMenu>().ToggleMenu();
        //sceneLoader.MainMenu();
    }
    
}
