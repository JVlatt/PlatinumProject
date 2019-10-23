﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : MonoBehaviour
{
    public enum TYPE
    {
        ATTACK,
        TEXT,
        FIX,
        HEAL,
        CAMERA,
        SOUND,
        BLANK,
        BREAK
    }

    [SerializeField]
    private TYPE _type;
    public TYPE type
    {
        get { return _type; }
        set { _type = value; }
    }

    [SerializeField]
    private float _duration;
    public float duration
    {
        get { return _duration; }
        set { _duration = value; }
    }

    [SerializeField]
    private string _text;
    public string text
    {
        get { return _text; }
        set { _text = value; }
    }

    [SerializeField]
    private Carriage _carriage;
    public Carriage carriage
    {
        get { return _carriage; }
        set { _carriage = value; }
    }

    [SerializeField]
    private bool _specialEvent;
    public bool specialEvent
    {
        get { return _specialEvent; }
        set { _specialEvent = value; }
    }
    [SerializeField]
    private string _sound;
    public string sound
    {
        get { return _sound; }
        set { _sound = value; }
    }
    [SerializeField]
    private bool _controlDuration = false;
    public bool controlDuration
    {
        get { return _controlDuration; }
        set { _controlDuration = value; }
    }

    [SerializeField]
    private bool _freezeControl = false;
    public bool freezeControl
    {
        get { return _freezeControl; }
        set { _freezeControl = value; }
    }

}
