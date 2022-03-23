using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    int currentScore;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameData>().Length;
        if (numGameSessions > 1)
        {
            FindObjectOfType<GameData>().Reset();
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
        currentScore = 0;
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateScore();
    }

    public void Reset()
    {
        Start();
    }

    private void UpdateScore()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            FindObjectOfType<UIUpdator>().UpdateScore(currentScore);
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
}
