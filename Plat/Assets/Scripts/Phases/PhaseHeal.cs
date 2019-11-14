using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseHeal : Phase
{
    public Peon peonToHeal;
    public bool waitTime;
    public override string BuildGameObjectName()
    {
        return "Heal (" + peonToHeal + ")";
    }

    public override void LaunchPhase()
    {
        controlDuration = waitTime;
    }
    private void Start()
    {
        type = PhaseType.HEAL;
    }

    private void Update()
    {
        if(PhaseManager.Instance.activePhase == this && peonToHeal._HEALTHSTATE == Peon.HEALTHSTATE.TREAT)
        {
            PhaseManager.Instance.EndCondition(true);
        }
    }
}
