using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clip : MonoBehaviour
{

    Carriage _carriage;

    private void Start()
    {
        _carriage = GetComponentInParent<Carriage>();
    }

    private void OnMouseEnter()
    {
        if (PeonManager.Instance._activePeon._type == Peon.TYPE.MECA)
            UIManager.Instance.ChangeCursor("unclip");
    }
    private void OnMouseOver()
    {
        if((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) 
            && PeonManager.Instance._activePeon != null 
            && PeonManager.Instance._activePeon._peonInfo.TYPE == Peon.TYPE.MECA)
        {
            Peon p = PeonManager.Instance._activePeon;
            TrainManager.Instance.MovePeonToCarriage(p, TrainManager.Instance._carriages[TrainManager.Instance._carriages.IndexOf(_carriage) - 1],transform.position);
            Meca meca = (Meca)p;
            meca.IsUncliping = true;
        }
    }

    private void OnMouseExit()
    {
        if (PeonManager.Instance._activePeon._type == Peon.TYPE.MECA)
            UIManager.Instance.ChangeCursor("default");
    }
}
