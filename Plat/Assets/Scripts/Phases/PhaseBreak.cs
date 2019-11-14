using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseBreak : Phase
{
    [Header("Break Phase Parameters")]
    public int carriage;
    [SerializeField]
    private Carriage.DEGATSTATE _damageAmount;

    public override void LaunchPhase()
    {
        Carriage c = TrainManager.Instance._carriages.Find(x => x.id == carriage);
        if (c != null)
        {
            c.Break(_damageAmount);
            c.isAnEvent = true;
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
