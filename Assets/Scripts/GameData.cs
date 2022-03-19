using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    [Space]
    [Header("References")]
    [SerializeField] TMPro.TextMeshProUGUI scoreText;
    int currentScore;

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
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void AddToScore(int value = 1)
    {
        currentScore += value;
        //Debug.Log(currentScore);
    }

    public int GetScore()
    {
        return currentScore;
    }
}
