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
        StartCoroutine(LoadScene(1));
        FindObjectOfType<MusicManager>().UpdateMusic(true);
        //FindObjectOfType<GameData>().Reset();
    }
    
    public void MainMenu()
    {
        //FindObjectOfType<PauseMenu>().ToggleMenu(); //reset from pause menu
        StartCoroutine(LoadScene(0));
        FindObjectOfType<MusicManager>().UpdateMusic(false);
    }

    IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(index);
        FindObjectOfType<MusicManager>().UpdateMusic(index != 0);
    }
}
