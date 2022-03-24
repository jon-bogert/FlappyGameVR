using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(5);
        //Destroy(FindObjectOfType<GameData>());
        //FindObjectOfType<GameData>().Reset();
        SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        //FindObjectOfType<GameData>().Reset();
    }
    
    public void MainMenu()
    {
        //FindObjectOfType<PauseMenu>().ToggleMenu(); //reset from pause menu
        SceneManager.LoadScene(0);
    }
}
