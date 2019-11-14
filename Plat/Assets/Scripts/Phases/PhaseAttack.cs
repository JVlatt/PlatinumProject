using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseAttack : Phase
{
    [Header("Attack Phase Parameters")]
    public int _carriage;
    [SerializeField]
    private float _timeBeforeAttack;
    private bool _getTankName = true;

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
