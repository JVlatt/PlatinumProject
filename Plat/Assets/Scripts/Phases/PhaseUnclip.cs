﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUnclip : Phase
{
    [Header("Unclip Phase Parameters")]

    [SerializeField]
    private int _carriage = 1;
    [SerializeField]
    private bool _unclipLast = false;

    public override void LaunchPhase()
    {
        if (_unclipLast)
        {
            if(TrainManager.Instance._carriages.Count > 4)
            TrainManager.Instance.UnclipCarriage(TrainManager.Instance._carriages.Count - 2);
        }

        else if(TrainManager.Instance._carriages.Find(x => x.id == _carriage))
            TrainManager.Instance.UnclipCarriage(_carriage - 1);
    }
    public override string BuildGameObjectName()
    {
        return "Unclip (Carriage " + _carriage + ")";
    }

    private void Start()
    {
        type = PhaseType.UNCLIP;
        controlDuration = true;
    }
}
