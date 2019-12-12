using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseNewCarriage : Phase
{
    [Header("Phase NewCarriage Parameters")]
    [SerializeField]
    GameObject carriagePrefab;
    private void Start()
    {
        type = PhaseType.NEWCARRIAGE;
        controlDuration = true;
    }

    public override string BuildGameObjectName()
    {
        return "newCarriage";
    }

    public override void LaunchPhase()
    {
        TrainManager.Instance.RecupererWagon(carriagePrefab);
    }
}
