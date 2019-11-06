using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBreak : Phase
{
    [SerializeField]
    private int _carriage;
    [SerializeField]
    private bool _waitForFix;

    public override void LaunchPhase()
    {
        TrainManager.Instance._carriages.Find(x => x.id == _carriage)._isBroke = true;
        TrainManager.Instance._carriages.Find(x => x.id == _carriage).fixIt.isAnEvent = _waitForFix;
    }
    public override string BuildGameObjectName()
    {
        return "Break (Wagon " + _carriage + ")";
    }
}
