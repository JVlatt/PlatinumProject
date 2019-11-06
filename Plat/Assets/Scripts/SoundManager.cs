using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] fx;
    public Sound[] ambient;
    public Sound music;
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Sound s in fx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in ambient)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        music.source = gameObject.AddComponent<AudioSource>();
        music.source.clip = music.clip;
        music.source.outputAudioMixerGroup = music.mixer;
        music.source.volume = music.volume;
        music.source.pitch = music.pitch;
        music.source.loop = music.loop;
    }
    private void Start()
    {
        foreach (Sound s in ambient)
        {
            s.source.Play();
        }
        music.source.Play();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(fx, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound" + name + "not found");
            return;
        }
        s.source.Play();
    }
}
