using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCamera : Phase
{
    [Header("Camera Phase Parameters")]
    [SerializeField]
    private int _carriage;

    public override void LaunchPhase()
    {
        freezeControl = true;
        controlDuration = false;
        if (_carriage >= TrainManager.Instance._carriages.Count)
            _carriage = TrainManager.Instance._carriages.Count - 1;
        CameraController.Instance.MoveToCarriage(TrainManager.Instance._carriages.Find(x => x.id == _carriage));
    }
    public override string BuildGameObjectName()
    {
        return "Camera (Wagon " + _carriage + ")";
    }

    private void Start()
    {
        type = PhaseType.CAMERA;
    }
}
