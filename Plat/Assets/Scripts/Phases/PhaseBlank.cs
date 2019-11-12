using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBlank : Phase
{
    [Header("Blank Phase Parameters")]
    [SerializeField]
    private bool waitNextEvent;
    public override string BuildGameObjectName()
    {
        return "Blank(" + duration +"s)";
    }

    public override void LaunchPhase()
    {
        controlDuration = !waitNextEvent;
    }

    private void Start()
    {
        type = PhaseType.BLANK;
    }
}
