using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseResetCamera : Phase
{
    public override void LaunchPhase()
    {
        freezeControl = true;
        CameraController.Instance.ResetCamera();
    }
    public override string BuildGameObjectName()
    {
        return "ResetCamera";
    }
    private void Start()
    {
        type = PhaseType.RESETCAMERA;
    }
}
