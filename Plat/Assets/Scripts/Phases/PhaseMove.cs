using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMove : Phase
{
    [Header("Kill Phase Parameters")]
    [SerializeField]
    private targetType _targetType = targetType.CUSTOM;
    [SerializeField]
    private string _target = "Name";
    [SerializeField]
    private int _carriage = 1;

    public enum targetType
    {
        CUSTOM,
        ACTOR
    }

    public override void LaunchPhase()
    {
        if (_carriage >= TrainManager.Instance._carriages.Count)
            _carriage = TrainManager.Instance._carriages.Count - 1;

        switch (_targetType)
        {
            case targetType.CUSTOM:
                if (PeonManager.Instance._peons.Find(x => x.name == _target) && TrainManager.Instance._carriages.Find(x => x.id == _carriage))
                    TrainManager.Instance.MovePeonToCarriage(PeonManager.Instance._peons.Find(x => x.name == _target),TrainManager.Instance._carriages.Find(x => x.id == _carriage));
                break;
            case targetType.ACTOR:
                if (PhaseManager.Instance.eventPeon != null && TrainManager.Instance._carriages.Find(x => x.id == _carriage))
                    TrainManager.Instance.MovePeonToCarriage(PeonManager.Instance._peons.Find(x => x.name == PhaseManager.Instance.eventPeon), TrainManager.Instance._carriages.Find(x => x.id == _carriage));
                break;
        }
    }
    public override string BuildGameObjectName()
    {
        return "MovePeon (Carriage " + _carriage+")";
    }

    private void Start()
    {
        type = PhaseType.MOVE;
        controlDuration = true;
    }
}
