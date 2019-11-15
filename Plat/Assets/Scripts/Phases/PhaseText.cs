using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseText : Phase
{
    public enum textType
    {
        CUSTOM,
        ACTOR,
        SPECTATOR
    }

    [SerializeField]
    private string _text;
    [SerializeField]
    private string _character;
    [SerializeField]
    private textType _textType;
    [SerializeField]
    private bool _isInstant;

    public override void LaunchPhase()
    {
        controlDuration = false;
        if(PhaseManager.Instance.eventPeon != null)
        {
            switch (_textType)
            {
                case textType.ACTOR:
                    _character = PhaseManager.Instance.eventPeon._peonInfo.name;
                    break;
                case textType.SPECTATOR:
                    _character = GetSpectator();
                    break;
            }
        }
        UIManager.Instance.DisplayText(_text, _character, duration, _isInstant);
        if (_isInstant)
            PhaseManager.Instance.NextPhase();
    }
    public override string BuildGameObjectName()
    {
        return "Text (" + _textType +" "+ _character + ")";
    }

    public override bool KillTextBox()
    {
        return false;
    }

    public string GetSpectator()
    {
        List<Peon> spectators = new List<Peon>();
        foreach(Peon p in PeonManager.Instance._peons)
        {
            if (p != PhaseManager.Instance.eventPeon)
                spectators.Add(p);
        }

        int rand = Random.Range(0, 2);
        return spectators[rand]._peonInfo.name;
    }
    private void Start()
    {
        type = PhaseType.TEXT;
    }
}
