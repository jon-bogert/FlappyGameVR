using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField] int tutorialIndex = 1;
    int points;

    public void ResetAttributes()
    {
        points = 0;
        //FindObjectOfType<UIUpdator>().TutorialUpdateScore(points);
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetAttributes();
    }

    public void AddPoints(int value = 1)
    {
        points += value;
        FindObjectOfType<UIUpdator>().TutorialUpdateScore(points);

        if (tutorialIndex == 1 && points >= 5)
        {
            StartCoroutine(NextPart());
        }

        if (tutorialIndex == 3 && points >= 6)
        {
            StartCoroutine(Finish());
        }
    }

    public IEnumerator NextPart()
    {
        yield return new WaitForSeconds(2);
        tutorialIndex++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ResetAttributes();
    }

    IEnumerator Finish()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<AudioManager>().Play("High Score");
        FindObjectOfType<TutorialEnd>().HasEnded();
    }

    public int GetTutorialIndex()
    {
        return tutorialIndex;
    }
}
