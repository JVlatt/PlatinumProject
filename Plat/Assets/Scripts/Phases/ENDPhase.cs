using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENDPhase : Phase
{

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
        UIManager.Instance.Fade(UIManager.FADETYPE.END);
    }
}
