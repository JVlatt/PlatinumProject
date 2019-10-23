using System.Collections;
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
        BLANK
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
    private bool _isCompleted;
    public bool isCompleted
    {
        get { return _isCompleted; }
        set { isCompleted = value; }
    }
}
