
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MusicManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;
    Sound melody;
    
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }
    public Sound FindSound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Cannot find sound with name " + name);
        }
        return sound;
    }
    
    public void SetLoop(string name, bool setValue)
    {
        Sound sound = FindSound(name);
        if (sound == null) return;
        sound.source.loop = setValue;
    }
    
    public void Play(string name)
    {
        Sound sound = FindSound(name);
        if (sound == null) return;
        sound.source.Play();
    }

    public Sound[] GetSounds()
    {
        return sounds;
    }
    
    void Start()
    {
        FindObjectOfType<GameData>().musicOn = true;
        melody = FindSound("Melody");
        Play("Background");
        UpdateMusic((SceneManager.GetActiveScene().buildIndex != 0));
        Play("Melody");
    }

    public void UpdateMusic(bool melodyEnabled)
    {
        melody.source.volume = (melodyEnabled) ? melody.volume : 0f;
    }

    public void OptionsStop()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
            FindObjectOfType<GameData>().musicOn = false;
        }
    }

    public void OptionsStart()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Play();
            FindObjectOfType<GameData>().musicOn = true;
        }
    }
    
}
