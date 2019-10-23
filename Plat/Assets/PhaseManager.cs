using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class PhaseManager : MonoBehaviour
{
    [SerializeField]
    private List<Phase> _phases;
    public List<Phase> phases
    {
        get { return _phases; }
        set { _phases = value; }
    }

    private Phase _activePhase;
    private int _phaseId;
    private float _phaseTimer;

    private void Awake()
    {
        GameManager.GetManager().phaseManager = this;
    }
    private void Start()
    {
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
        
    }

    public void StartPhase()
    {
        switch (_activePhase.type)
        {
            case Phase.TYPE.ATTACK:
                _activePhase.carriage.Attack(_activePhase.duration);
                break;
            case Phase.TYPE.TEXT:
                GameManager.GetManager()._UIManager.DisplayText(_activePhase.text,_activePhase.duration);
                break;
            case Phase.TYPE.CAMERA:
                GameManager.GetManager().cameraController.MoveToCarriage(_activePhase.carriage);
                break;
            case Phase.TYPE.BLANK:
                break;
            default:
                break;
        }
    }

    public void NextPhase()
    {
        _phaseTimer = 0;
        if (_phaseId < _phases.Count - 1)
        {
            _phaseId++;
        }
        _activePhase = _phases[_phaseId];
        StartPhase();
    }

    public Phase.TYPE GetNextPhaseType()
    {
        if (_phaseId + 1 > _phases.Count - 1) return Phase.TYPE.BLANK;
        return _phases[_phaseId + 1].type;
    }
}
