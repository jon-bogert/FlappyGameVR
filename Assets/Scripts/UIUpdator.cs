using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdator : MonoBehaviour
{
[SerializeField] TMPro.TextMeshProUGUI scoreText;
    
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
    
}
