using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENDPhase : Phase
{
    [SerializeField]
    private bool isGenerique;
    private void Start()
    {
        controlDuration = false;
    }
    public override string BuildGameObjectName()
    {
        return "End Phase";
    }

    public override void LaunchPhase()
    {
        if (isGenerique)
            ;
        else
            UIManager.Instance.Fade(UIManager.FADETYPE.END);
    }
}
