using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    int currentScore = 0;
    int highScore = 0;

    public bool musicOn = true;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameData>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ResetScore(); // for dubug
        ResetGameData();
    }

    void ResetScore()
    {
        PlayerPrefs.SetInt("High Score", 0);
    }
    

    public void ResetGameData()
    {
        Load();
        currentScore = 0;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            FindObjectOfType<UIUpdator>().UpdateScore(currentScore, highScore);
        }
    }

    public void AddToScore(int value = 1)
    {
        currentScore += value;
        UpdateScore();
    }

    public int GetScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        Load();
        return highScore;
    }

    public void CheckHighScore()
    {
        if (currentScore > highScore)
        {
            FindObjectOfType<AudioManager>().Play("High Score");
            highScore = currentScore;
            Save();
            UpdateScore();
        }
    }

    void Save()
    {
        PlayerPrefs.SetInt("High Score", highScore);
    }

    void Load()
    {
        highScore = PlayerPrefs.GetInt("High Score", 0);
    }
}
