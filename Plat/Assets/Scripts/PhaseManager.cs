﻿using System.Collections;
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

    private string _eventPeon;
    public string eventPeon
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
        _phases = HierarchyUtils.GetComponentsInDirectChildren<Phase>(this.transform,true);
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
            if(_activePhase.mode == Phase.PhaseMode.CONDITION)
            {
                EndCondition(false);
            }
            else
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
        switch (_activePhase.mode)
        {
            case Phase.PhaseMode.MAIN:
                if (_phaseId < _phases.Count - 1)
                {
                    _phaseId++;
                }
                _phaseBuffer.Add(_phases[_phaseId]);
                break;
            case Phase.PhaseMode.GROUP:
                _phaseBuffer.InsertRange(0,_activePhase.subPhases);
                break;
            case Phase.PhaseMode.SUB:
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
            _phaseBuffer.Insert(0,_activePhase.subPhases[0]);
        }
        else
        {
            _phaseBuffer.Insert(0, _activePhase.subPhases[1]);
        }
        _activePhase = _phaseBuffer[0];
        _phaseTimer = 0;
        StartPhase();
    }

    public Phase GetNextPhase()
    {
        if (_phaseId+1<_phases.Count && _phases[_phaseId + 1] != null)
            return _phases[_phaseId + 1];
        else
            return _phaseBuffer[1];
    }

    public void GetPeon(string p)
    {
        _eventPeon = p;
    }
}
