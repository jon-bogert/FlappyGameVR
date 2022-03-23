using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject rightHandController;
    [SerializeField] GameObject leftHandController;
    [SerializeField] GameObject hudCanvas;
    
    bool gamePaused = false;

    void Start()
    {
        gamePaused = false;
        Time.timeScale = 1f;
    }

    public void ToggleMenu()
    {
        
        if (gamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Resume()
    {
        hudCanvas.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        SetControllerProperties(false);
    }

    void Pause()
    {
        hudCanvas.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        SetControllerProperties(true);
    }

    void SetControllerProperties(bool isEnabled)
    {
        //Debug.Log("Enable: " + isEnabled + " | Time Scale: " + Time.timeScale);
        rightHandController.GetComponent<LineRenderer>().enabled = isEnabled;
        rightHandController.GetComponent<XRInteractorLineVisual>().enabled = isEnabled;
        leftHandController.GetComponent<LineRenderer>().enabled = isEnabled;
        leftHandController.GetComponent<XRInteractorLineVisual>().enabled = isEnabled;
        Debug.Log("HUD UI " + hudCanvas.activeSelf);
    }

    public bool GetGamePaused()
    {
        return gamePaused;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        //FindObjectOfType<SceneLoader>().MainMenu();
    }
    
}
