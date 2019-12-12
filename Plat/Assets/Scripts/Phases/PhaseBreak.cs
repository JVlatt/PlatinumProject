using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBreak : Phase
{
    [Header("Break Phase Parameters")]
    public int carriage;
    [SerializeField]
    private Carriage.DEGATSTATE _damageAmount;
    [SerializeField]
    private bool playSound = true;
    [SerializeField]
    private bool _waitFix = true;
    public override void LaunchPhase()
    {
        controlDuration = !_waitFix;
        if (carriage > TrainManager.Instance._carriages.Count - 1)
        {
            carriage = TrainManager.Instance._carriages.Count - 1;
        }
        Carriage c = TrainManager.Instance._carriages.Find(x => x.id == carriage);
        
        if (c != null)
        {
            c.Break(_damageAmount);
            c.isAnEvent = true;
            if(playSound)
            SoundManager.Instance.Play("break");
        }
    }
    public override string BuildGameObjectName()
    {
        return "Break (Wagon " + carriage + ")";
    }

    private void Start()
    {
        type = PhaseType.BREAK;
    }
}
