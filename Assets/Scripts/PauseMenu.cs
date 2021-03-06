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
    [SerializeField] GameObject rightTorch;
    [SerializeField] GameObject leftTorch;
    
    bool gamePaused = false;
    List<AudioSource> pausedSounds;

    void Start()
    {
        gamePaused = false;
        Time.timeScale = 1f;
    }

    public void ToggleMenu()
    {
        FindObjectOfType<UIUpdator>().ToggleBeginMsg();
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
        FindObjectOfType<AudioManager>().UnPause();
        hudCanvas.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        SetControllerProperties(false);
    }

    void Pause()
    {
        FindObjectOfType<AudioManager>().Pause();
        hudCanvas.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        SetControllerProperties(true);
    }

    void SetControllerProperties(bool isEnabled)
    {
        rightHandController.GetComponent<LineRenderer>().enabled = isEnabled;
        rightHandController.GetComponent<XRInteractorLineVisual>().enabled = isEnabled;
        leftHandController.GetComponent<LineRenderer>().enabled = isEnabled;
        leftHandController.GetComponent<XRInteractorLineVisual>().enabled = isEnabled;
        rightTorch.SetActive(isEnabled);
        leftTorch.SetActive(isEnabled);
    }

    public bool GetGamePaused()
    {
        return gamePaused;
    }

    public void MainMenu()
    {
        Resume();
        
        if (FindObjectOfType<TutorialController>() != null)
            Destroy(FindObjectOfType<TutorialController>().gameObject);
        
        StartCoroutine(MenuDelay());
    }

    IEnumerator MenuDelay()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(0);
        FindObjectOfType<MusicManager>().UpdateMusic(false);
    }
    
}
