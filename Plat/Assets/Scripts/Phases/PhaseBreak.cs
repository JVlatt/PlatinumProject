using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBreak : Phase
{
    [Header("Break Phase Parameters")]
    public int _carriage;
    [SerializeField]
    private Carriage.DEGATSTATE _damageAmount;

    public override void LaunchPhase()
    {
        if(TrainManager.Instance._carriages.Find(x => x.id == _carriage))
        {
            TrainManager.Instance._carriages.Find(x => x.id == _carriage).Break(_damageAmount);
            TrainManager.Instance._carriages.Find(x => x.id == _carriage).isAnEvent = true;
            SoundManager.Instance.Play("break");
        }
    }
    public override string BuildGameObjectName()
    {
        return "Break (Wagon " + _carriage + ")";
    }

    private void Start()
    {
        type = PhaseType.BREAK;
    }
}
