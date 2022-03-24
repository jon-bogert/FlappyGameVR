using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdator : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI scoreText;
    [SerializeField] TMPro.TextMeshProUGUI scoreTextPause;
    [SerializeField] TMPro.TextMeshProUGUI highScoreTextPause;

    [Space]
    [SerializeField] GameObject flapToBegin;

    bool startTextActive = true;

    public void UpdateScore(int score, int highScore)
    {
        scoreText.text = "Score: " + score;
        scoreTextPause.text = "Score: " + score;
        highScoreTextPause.text = "High Score: " + highScore;
    }

    public void ToggleBeginMsg()
    {
        if (startTextActive)
        {
            flapToBegin.SetActive(!flapToBegin.activeSelf);
        }
    }
    public void DisableBeginMsg()
    {
        flapToBegin.SetActive(false);
        startTextActive = false;
    }

    
    
}
