using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
