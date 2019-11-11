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
    [SerializeField]
    private bool _getTankName;

    private void Start()
    {
        type = PhaseType.ATTACK;
    }
    public override void LaunchPhase()
    {
        controlDuration = false;
        if(TrainManager.Instance._carriages.Find(x => x.id == _carriage))
        {
            TrainManager.Instance._carriages.Find(x => x.id == _carriage).Attack(duration, _timeBeforeAttack);
            TrainManager.Instance._carriages.Find(x => x.id == _carriage).isAnEvent = _getTankName;
            SoundManager.Instance.Play("attack");
        }
        
    }
    public override string BuildGameObjectName()
    {
        return "Attack (Wagon " + _carriage +")";
    }
}
