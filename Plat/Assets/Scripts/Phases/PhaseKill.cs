using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseKill : Phase
{
    [Header("Kill Phase Parameters")]
    [SerializeField]
    private targetType _targetType = targetType.CUSTOM;
    [SerializeField]
    public string _target = "Name";
    public enum targetType
    {
        CUSTOM,
        ACTOR
    }

    public override void LaunchPhase()
    {
        switch (_targetType)
        {
            case targetType.CUSTOM:
                if(PeonManager.Instance._peons.Find(x => x.name == _target))
                    PeonManager.Instance._peons.Find(x => x.name == _target).Death();
                break;
            case targetType.ACTOR:
                if (PhaseManager.Instance.eventPeon != null)
                    PeonManager.Instance._peons.Find(x => x._peonInfo.name == PhaseManager.Instance.eventPeon).Death();
                break;
        }
    }
    public override string BuildGameObjectName()
    {
        return "Kill";
    }

    private void Start()
    {
        type = PhaseType.KILL;
        controlDuration = true;
    }
}
