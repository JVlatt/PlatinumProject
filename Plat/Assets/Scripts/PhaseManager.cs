using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class PhaseManager : MonoBehaviour
{
    [SerializeField]
    private Phase[] _phases;
    public Phase[] phases
    {
        get { return _phases; }
        set { _phases = value; }
    }

    private Phase _activePhase;
    private int _phaseId;
    private float _phaseTimer;

    private bool _freezeControl;
    public bool freezeControl
    {
        get { return _freezeControl; }
        set { _freezeControl = value; }
    }
    private Peon _eventPeon;
    public Peon eventPeon
    {
        get { return _eventPeon; }
        set { _eventPeon = value; }
    }

    private void Awake()
    {
        GameManager.GetManager().phaseManager = this;
    }
    private void Start()
    {
        _phases = GetComponentsInChildren<Phase>();
        _phaseId = 0;
        _activePhase = _phases[_phaseId];
        StartPhase();
    }

    private void Update()
    {
        GameLoop();
    }

    public void GameLoop()
    {
        if(_activePhase.controlDuration)
        {
            _phaseTimer += Time.deltaTime;
            if(_phaseTimer >= _activePhase.duration)
            {
                NextPhase();
            }
        }
        else
        {
            _phaseTimer = 0f;
        }
    }

    public void StartPhase()
    {
        _freezeControl = _activePhase.freezeControl;
        switch (_activePhase.type)
        {
            case Phase.TYPE.ATTACK:
                _activePhase.carriage.Attack(_activePhase.duration,_activePhase.subDuration);
                _activePhase.carriage.isAnEvent = _activePhase.specialEvent;
                _activePhase.carriage.autoWin = _activePhase.win;
                break;
            case Phase.TYPE.TEXT:
                GameManager.GetManager()._UIManager.isAnEvent = _activePhase.specialEvent;
                GameManager.GetManager()._UIManager.DisplayText(_activePhase.text, _activePhase.character, _activePhase.duration);
                break;
            case Phase.TYPE.CAMERA:
                GameManager.GetManager().cameraController.MoveToCarriage(_activePhase.carriage);
                break;
            case Phase.TYPE.BLANK:
                break;
            case Phase.TYPE.SOUND:
                GameManager.GetManager()._soundManager.Play(_activePhase.sound);
                break;
            case Phase.TYPE.BREAK:
                _activePhase.carriage._isBroke = true;
                _activePhase.carriage.fixIt.isAnEvent = _activePhase.specialEvent;
                break;
            case Phase.TYPE.MOVE:
                GameManager.GetManager()._trainManager.MovePeonToCarriage(_activePhase.peon, _activePhase.carriage);
                break;
            case Phase.TYPE.RESETCAMERA:
                GameManager.GetManager().cameraController.ResetCamera();
                break;
            case Phase.TYPE.FADE:
                GameManager.GetManager()._UIManager.fade();
                break;
        }
    }

    public void NextPhase()
    {
        _phaseTimer = 0;
        if (_phaseId < _phases.Length - 1)
        {
            _phaseId++;
        }
        _activePhase = _phases[_phaseId];
        StartPhase();
    }

    public Phase.TYPE GetNextPhaseType()
    {
        if (_phaseId + 1 > _phases.Length - 1) return Phase.TYPE.BLANK;
        return _phases[_phaseId + 1].type;
    }

    public void GetPeon(Peon p)
    {
        _eventPeon = p;
    }
}
