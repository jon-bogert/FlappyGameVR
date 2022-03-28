
using UnityEngine;
using UnityEngine.UI;

public class MusicOptionsButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Toggle>().isOn = FindObjectOfType<GameData>().musicOn;
    }

    public void OptionsMusic(bool musicOn)
    {
        if (musicOn) FindObjectOfType<MusicManager>().OptionsStart();
        else FindObjectOfType<MusicManager>().OptionsStop();
    }
    
}
