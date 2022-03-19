using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<SceneLoader>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(5);
        Destroy(FindObjectOfType<GameData>());
        SceneManager.LoadScene(0);
    }
}
