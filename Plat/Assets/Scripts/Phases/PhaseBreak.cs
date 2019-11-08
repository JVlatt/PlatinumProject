using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBreak : Phase
{
    public int _carriage;
    [SerializeField]
    private bool _waitForFix;
    [SerializeField]
    private Carriage.DEGATSTATE _damageAmont;

    public override void LaunchPhase()
    {
        if(TrainManager.Instance._carriages.Find(x => x.id == _carriage))
        {
            TrainManager.Instance._carriages.Find(x => x.id == _carriage).Break(_damageAmont);
            if (_waitForFix)
            {
                TrainManager.Instance._carriages.Find(x => x.id == _carriage).fixIt.isAnEvent = _waitForFix;
            }
            else
            {
                controlDuration = true;
            }
            SoundManager.Instance.Play("break");
        }
    }
    public override string BuildGameObjectName()
    {
        return "Break (Wagon " + _carriage + ")";
    }
}
