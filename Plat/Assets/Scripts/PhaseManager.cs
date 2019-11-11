using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class PhaseManager : MonoBehaviour
{
    #region Singleton
    private static PhaseManager _instance = null;

    public static PhaseManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion


    [SerializeField]
    private List<Phase> _phases;
    public List<Phase> phases
    {
        get { return _phases; }
        set { _phases = value; }
    }
    [SerializeField]
    private List<Phase> _phaseBuffer;
    public List<Phase> phaseBuffer
    {
        get { return _phaseBuffer; }
        set { _phaseBuffer = value; }
    }
    private Phase _activePhase;
    public Phase activePhase
    {
        get { return _activePhase; }
        set { _activePhase = value; }
    }
    private int _phaseId;
    private float _phaseTimer;

    private Peon _eventPeon;
    public Peon eventPeon
    {
        get { return _eventPeon; }
        set { _eventPeon = value; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            _instance = this;
    }
    private void Start()
    {
        _phases = HierarchyUtils.GetComponentInDirectChildren<Phase>(this.transform);
        _phaseId = 0;
        _activePhase = _phases[_phaseId];
        _phaseBuffer.Add(_activePhase);
        StartPhase();
    }

    private void Update()
    {
        GameLoop();
    }

    public void GameLoop()
    {
        _phaseTimer += Time.deltaTime;
        if (_phaseTimer >= _activePhase.duration && _activePhase.controlDuration)
        {
            NextPhase();
        }
    }

    public void StartPhase()
    {
        _activePhase.LaunchPhase();
    }

    public void NextPhase()
    {
        _phaseBuffer.Remove(_activePhase);
        switch (_activePhase.type)
        {
            case Phase.PhaseType.MAIN:
                if (_phaseId < _phases.Count - 1)
                {
                    _phaseId++;
                }
                _phaseBuffer.Add(_phases[_phaseId]);
                break;
            case Phase.PhaseType.GROUP:
                _phaseBuffer = _activePhase.subPhases;
                break;
            case Phase.PhaseType.SUB:
                if(_phaseBuffer.Count < 1)
                {
                    if (_phaseId < _phases.Count - 1)
                    {
                        _phaseId++;
                    }
                    _phaseBuffer.Add(_phases[_phaseId]);
                }
                break;
        }
        _activePhase = _phaseBuffer[0];
        _phaseTimer = 0;
        StartPhase();
    }

    public void EndCondition(bool isWin)
    {
        _phaseBuffer.Remove(_activePhase);
        if (isWin)
        {
            _phaseBuffer.Add(_activePhase.subPhases[0]);
        }
        else
        {
            _phaseBuffer.Add(_activePhase.subPhases[1]);
        }
        _activePhase = _phaseBuffer[0];
        _phaseTimer = 0;
        StartPhase();
    }

    public Phase GetNextPhase()
    {
        if (_phaseId + 1 > _phases.Count - 1) return null;
        return _phases[_phaseId + 1];
    }

    public void GetPeon(Peon p)
    {
        _eventPeon = p;
    }
}
