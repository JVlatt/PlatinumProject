using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseText : Phase
{
    public enum textType
    {
        CUSTOM,
        ACTOR,
        SPECTATOR,
    }

    [SerializeField]
    private string _text;
    [SerializeField]
    private string _character;
    [SerializeField]
    private textType _textType;
    [SerializeField]
    private bool _isInstant;

    [SerializeField]
    private string _textOni;
    [SerializeField]
    private string _textTaon;
    [SerializeField]
    private string _textButor;
    public override void LaunchPhase()
    {
        controlDuration = false;
        if (PhaseManager.Instance.eventPeon != null)
        {
            switch (_textType)
            {
                case textType.ACTOR:
                    _character = PhaseManager.Instance.eventPeon._peonInfo.name;
                    break;
                case textType.SPECTATOR:
                    if(_character == "")
                    _character = GetSpectator();
                    switch (_character)
                    {
                        case "Oni":
                            _text = _textOni;
                            break;
                        case "Butor":
                            _text = _textButor;
                            break;
                        case "Taon":
                            _text = _textTaon;
                            break;
                    }
                    break;
            }
        }
        if (_textType == textType.CUSTOM)
        {
            if (!PeonManager.Instance._peons.Find(x => x._peonInfo.name == _character))
            {
                PhaseManager.Instance.NextPhase();
            }
        }
        if (_textType == textType.ACTOR && PhaseManager.Instance.eventPeon == null)
        {
            PhaseManager.Instance.NextPhase();
        }
        else
        {
            UIManager.Instance.DisplayText(_text, _character, duration, _isInstant);
            if (_isInstant)
                PhaseManager.Instance.NextPhase();
        }
    }
    public override string BuildGameObjectName()
    {
        return "Text (" + _textType + " " + _character + ")";
    }

    public override bool KillTextBox()
    {
        return false;
    }

    public string GetSpectator()
    {
        List<Peon> spectators = new List<Peon>();
        if (PhaseManager.Instance.eventPeon != null)
        {
            foreach (Peon p in PeonManager.Instance._peons)
            {
                if (p != PhaseManager.Instance.eventPeon)
                    spectators.Add(p);
            }

            if(spectators.Count > 0)
            {
                int rand = Random.Range(0, spectators.Count);
                if (GetNextSpectator() && spectators.Count > 1)
                {
                    GetNextSpectator().SetCharacter((spectators.Find(x => x != spectators[rand])).name);
                }
                return spectators[rand]._peonInfo.name;
            }
            else
            {
                PhaseManager.Instance.NextPhase();
                return null;
            }
        }
        else
        {
            PhaseManager.Instance.NextPhase();
            return null;
        }
    }
    private void Start()
    {
        type = PhaseType.TEXT;
    }
    public textType GetTextType()
    {
        return _textType;
    }

    public PhaseText GetNextSpectator()
    {
        if (PhaseManager.Instance.phaseBuffer.Count > 1)
        {
            if (PhaseManager.Instance.phaseBuffer[1].GetPhaseType() == PhaseType.TEXT && ((PhaseText)PhaseManager.Instance.phaseBuffer[1]).GetTextType() == textType.SPECTATOR)
            {
                return (PhaseText)PhaseManager.Instance.phaseBuffer[1];
            }
            else
                return null;
        }
        else
            return null;
    }

    public void SetCharacter(string text)
    {
        _character = text;
    }

    public void SetType(textType type)
    {
        _textType = type;
    }
}
