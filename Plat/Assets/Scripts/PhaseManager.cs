using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Phase[] _phases;
    public Phase[] phases
    {
        get { return _phases; }
        set { _phases = value; }
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
        _activePhase.LaunchPhase();
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

    public Phase GetNextPhase()
    {
        if (_phaseId + 1 > _phases.Length - 1) return null;
        return _phases[_phaseId + 1];
    }

    public void GetPeon(Peon p)
    {
        _eventPeon = p;
    }
}
