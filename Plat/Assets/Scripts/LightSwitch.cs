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
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private void OnMouseOver()
    {
        if (!(Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(0))) return;
        if (!PeonManager.Instance._activePeon) return;
        if (PhaseManager.Instance && PhaseManager.Instance.activePhase.freezeControl) return;
        if (!TrainManager.Instance._isShutDown) return;

        Peon p = PeonManager.Instance._activePeon;
        TrainManager.Instance.MovePeonToCarriage(p, _locomotive, transform.position);
        p._isFixing = true;
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
#endif
#if UNITY_ANDROID || UNITY_EDITOR
    public void Touch()
    {
        if (!PeonManager.Instance._activePeon) return;
        if (PhaseManager.Instance && PhaseManager.Instance.activePhase.freezeControl) return;
        if (!TrainManager.Instance._isShutDown) return;

        Peon p = PeonManager.Instance._activePeon;
        TrainManager.Instance.MovePeonToCarriage(p, _locomotive, transform.position);
        p._isFixing = true;
    }
#endif
    

    
}
