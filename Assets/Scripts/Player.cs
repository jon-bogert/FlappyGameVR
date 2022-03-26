using System;
using System.Collections;
using System.IO.Enumeration;
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
    [SerializeField] InputActionReference homeAction;

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
    AudioManager audioManager;
    bool isDead = false;
    int shieldValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        world = FindObjectOfType<World>();
        audioManager = FindObjectOfType<AudioManager>();
        activateShieldAction.action.performed += ActivateShield;
        pauseAction.action.performed += PauseGame;
        homeAction.action.performed += HomeButtonPress;
        gameData.ResetGameData();
        //FindObjectOfType<UIUpdator>().UpdateScore(); // Debug

        GetComponent<Rigidbody>().useGravity = false;
    }

    void OnDestroy()
    {
        activateShieldAction.action.performed -= ActivateShield;
        pauseAction.action.performed -= PauseGame;
        homeAction.action.performed -= HomeButtonPress;
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
            if (!isDead) audioManager.Play("Player Death");
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
            audioManager.Play("Checkpoint");
            gameData.AddToScore();
            //FindObjectOfType<UIUpdator>().UpdateScore(); // Debug
            if (shieldValue < shieldMax && !shieldActive)
            {
                shieldValue++;
                if (shieldValue == 5) audioManager.Play("Shield Ready");
            }
            UpdateShieldSlider();
        }

        else if (other.gameObject.tag == "Item Glide" && !isDead)
        {
            //audioManager.Play("Shield Powerup");
            Destroy(other.gameObject);
            StartCoroutine(GlideModeCoroutine());
        }
        else if (other.gameObject.tag == "Item Shield" && !isDead)
        {
            audioManager.Play("Shield Powerup");
            Destroy(other.gameObject);
            shieldValue = shieldMax;
            UpdateShieldSlider();
        }
    }

    IEnumerator ResetGame()
    {
        Time.timeScale = 0.5f;
        gameData.CheckHighScore();
        yield return new WaitForSeconds(5 * Time.timeScale);
        //Destroy(FindObjectOfType<GameData>());
        FindObjectOfType<GameData>().ResetGameData();
        Time.timeScale = 1f;
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
        audioManager.Play("Engine");
        glideMode = true;
        UpdateEngineMesh();
        int duration = (int)glideModeDuration;
        glideSlider.value = duration;
        while (duration > 0)
        {
            yield return new WaitForSeconds(0.9f);
            duration--;
            glideSlider.value = duration;
        }

        glideMode = false;
        UpdateEngineMesh();
        if (world.GetCurrentMovement() == 0)  world.StartMovement();
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
        
        audioManager.Play("Shield Start");
        yield return new WaitForSeconds(1);
        shieldValue--;
        UpdateShieldSlider();
        
        while (shieldValue > 1)
        {
            audioManager.Play("Shield Loop");
            yield return new WaitForSeconds(1);
            shieldValue--;
            UpdateShieldSlider();
        }
        audioManager.SetLoop("Shield Loop", false);
        audioManager.Play("Shield End");
        yield return new WaitForSeconds(1);
        shieldValue--;
        UpdateShieldSlider();
        
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
        audioManager.Play("Button Click");
    }

    void HomeButtonPress(InputAction.CallbackContext obj)
    {
        if (!FindObjectOfType<PauseMenu>().GetGamePaused())
        {
            FindObjectOfType<PauseMenu>().ToggleMenu();
        }
    }
    
}
