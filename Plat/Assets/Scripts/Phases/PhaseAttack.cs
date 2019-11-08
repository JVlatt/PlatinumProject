using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseAttack : Phase
{
    [SerializeField]
    private int _carriage;
    [SerializeField]
    private bool _win = false;
    [SerializeField]
    private float _timeBeforeAttack;

    public override void LaunchPhase()
    {
        controlDuration = false;
        if(TrainManager.Instance._carriages.Find(x => x.id == _carriage))
        {
            TrainManager.Instance._carriages.Find(x => x.id == _carriage).Attack(duration, _timeBeforeAttack);
            TrainManager.Instance._carriages.Find(x => x.id == _carriage).autoWin = _win;
            SoundManager.Instance.Play("attack");
        }
        
    }
    public override string BuildGameObjectName()
    {
        return "Attack (Wagon " + _carriage +")";
    }
}
