using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCamera : Phase
{
    [SerializeField]
    private int _carriage;

    public override void LaunchPhase()
    {
        freezeControl = true;
        controlDuration = false;
        CameraController.Instance.MoveToCarriage(TrainManager.Instance._carriages.Find(x => x.id == _carriage));
    }
    public override string BuildGameObjectName()
    {
        return "Camera (Wagon " + _carriage + ")";
    }
}
