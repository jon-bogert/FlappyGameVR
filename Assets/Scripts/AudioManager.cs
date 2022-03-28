using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    [SerializeField] Sound[] sounds;
    
    
    // Start is called before the first frame update
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

    public void Pause()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Pause();
        }
    }

    public void UnPause()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.UnPause();
        }
    }
}
