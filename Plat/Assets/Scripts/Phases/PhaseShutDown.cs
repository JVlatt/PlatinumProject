﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseShutDown : Phase
{
    public override void LaunchPhase()
    {
        controlDuration = true;
        SoundManager.Instance.Play("shutdown");
        TrainManager.Instance.ShutDown(duration);
    }
    public override string BuildGameObjectName()
    {
        return "ShutDown";
    }
    private void Start()
    {
        type = PhaseType.SHUTDOWN;
    }
}
