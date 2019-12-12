using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseUnclip : Phase
{
    [Header("Unclip Phase Parameters")]

    [SerializeField]
    private int _carriage = 1;
    [SerializeField]
    private bool _unclipLast = false;
    private bool isLaunched = false;

    public override void LaunchPhase()
    {
        isLaunched = true;
        if (_unclipLast)
            _carriage = TrainManager.Instance._carriages.Count - 1;
    }
    public override string BuildGameObjectName()
    {
        return "Unclip (Carriage " + _carriage + ")";
    }

    private void Start()
    {
        type = PhaseType.UNCLIP;
        controlDuration = true;
    }

    private void Update()
    {
        if (isLaunched)
        {
            if (!TrainManager.Instance._carriages.Find(x => x.id == _carriage))
                PhaseManager.Instance.EndCondition(true);

            if (PhaseManager.Instance.activePhase != this)
                isLaunched = false;
        }
    }
}
