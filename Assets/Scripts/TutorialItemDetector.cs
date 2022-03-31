
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialItemDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item Glide")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            FindObjectOfType<TutorialController>().ResetAttributes();
        }
        
    }
}
