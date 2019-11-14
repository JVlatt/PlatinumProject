using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseNewCarriage : Phase
{
    [Header("Phase NewCarriage Parameters")]
    [SerializeField]
    bool withPeon;
    [SerializeField]
    GameObject carriagePrefab;
    [SerializeField]
    float choiceTimer;
    [SerializeField]
    private string _text;
    private void Start()
    {
        type = PhaseType.NEWCARRIAGE;
    }

    public override string BuildGameObjectName()
    {
        return "newCarriage";
    }

    public override void LaunchPhase()
    {
        TrainManager.Instance.EventNewCarriage(carriagePrefab, choiceTimer,withPeon);
        UIManager.Instance.choiceText = _text;
    }
}
