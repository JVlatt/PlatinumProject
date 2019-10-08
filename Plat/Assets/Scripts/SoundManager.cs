using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer; 
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        GameManager.GetManager()._soundManager = instance;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound" + name + "not found");
            return;
        }
        s.source.Play();
    }
}
