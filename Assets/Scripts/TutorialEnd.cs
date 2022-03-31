using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialEnd : MonoBehaviour
{
    [SerializeField] GameObject endScreenObject;
    [SerializeField] InputActionReference pauseAction;

    bool hasEnded = false;

    void Start()
    {
        pauseAction.action.performed += EndThis;
    }

    void OnDestroy()
    {
        pauseAction.action.performed -= EndThis;
    }

    void EndThis(InputAction.CallbackContext obj)
    {
        if (hasEnded)
        {
            Destroy(gameObject);
        }
    }

    public void HasEnded()
    {
        hasEnded = true;
        endScreenObject.SetActive(true);
        Time.timeScale = 0f;
    }
}