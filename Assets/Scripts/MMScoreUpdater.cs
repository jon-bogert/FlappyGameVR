using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMScoreUpdater : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI highScoreText;
    void Start()
    {
        highScoreText.text = "High Score: " + FindObjectOfType<GameData>().GetHighScore();
    }
}
