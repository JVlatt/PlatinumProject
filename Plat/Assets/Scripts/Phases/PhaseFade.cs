using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseFade : Phase
{
    [Header("Fade Parameter")]
    [SerializeField]
    private bool ActiveFade;
    private void Start()
    {
        controlDuration = false;
    }
    public override string BuildGameObjectName()
    {
        if (ActiveFade)
            return "Active Fade";
        else
            return "Desactive Fade";
    }

    public override void LaunchPhase()
    {
        if (ActiveFade)
            UIManager.Instance.Fade(UIManager.FADETYPE.FADEEVENT);
        else
            UIManager.Instance.Fade(UIManager.FADETYPE.ENDFADEEVENT);
    }
}
