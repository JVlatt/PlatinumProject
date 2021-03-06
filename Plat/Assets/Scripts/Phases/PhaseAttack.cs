﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseAttack : Phase
{
    [Header("Attack Phase Parameters")]
    public int _carriage;
    [SerializeField]
    private float _timeBeforeAttack;
    private bool _getTankName = true;
    [SerializeField]
    bool autoLoose = false;
    private void Start()
    {
        type = PhaseType.ATTACK;
    }
    public override void LaunchPhase()
    {
        controlDuration = false;
        if(_carriage > TrainManager.Instance._carriages.Count - 1)
        {
            _carriage = TrainManager.Instance._carriages.Count - 1;
        }
        Carriage c = TrainManager.Instance._carriages.Find(x => x.id == _carriage);
        if (c != null)
        {
            c.Attack(duration, _timeBeforeAttack);
            c.isAnEvent = _getTankName;
            c.autoLoose = autoLoose;
            SoundManager.Instance.Play("attack");
            PeonManager.Instance._activePeon = null;
            if (mode != PhaseMode.CONDITION)
                PhaseManager.Instance.NextPhase();
        }
        
    }
    public override string BuildGameObjectName()
    {
        return "Attack (Wagon " + _carriage +")";
    }
}
