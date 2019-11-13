using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    private Carriage _locomotive;

    private void Start()
    {
        _locomotive = TrainManager.Instance._carriages[0];
    }
    private void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (!PeonManager.Instance._activePeon) return;
        if (PhaseManager.Instance && PhaseManager.Instance.activePhase.freezeControl) return;
        if (!TrainManager.Instance._isShutDown) return;

        PeonManager.Instance._activePeon._isFixing = true;
        TrainManager.Instance.MovePeonToCarriage(PeonManager.Instance._activePeon, _locomotive, transform.position);
    }

    private void Update()
    {
        if(_locomotive._activePeons.Count > 0 && _locomotive._activePeons[0]._isFixing && TrainManager.Instance._isShutDown)
        {
            _locomotive._activePeons[0]._isFixing = false;
            TrainManager.Instance.RepairLights();
        }
    }

    private void OnMouseEnter()
    {
        if (PeonManager.Instance._activePeon != null && !PhaseManager.Instance.activePhase.freezeControl && TrainManager.Instance._isShutDown)
            UIManager.Instance.ChangeCursor("fix");
    }
    private void OnMouseExit()
    {
        UIManager.Instance.ChangeCursor("default");
    }
}
