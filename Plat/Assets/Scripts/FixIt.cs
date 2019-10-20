using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class FixIt : MonoBehaviour
{
    Carriage _carriage;

    public void Setup(Carriage carriage)
    {
        _carriage = carriage;
    }

    private void OnMouseDown()
    {
        if (GameManager.GetManager()._peonManager._activePeon)
        {
            GameManager.GetManager()._trainManager.MovePeonToCarriage(GameManager.GetManager()._peonManager._activePeon, _carriage);
            GameManager.GetManager()._peonManager._activePeon._isFixing = true;
        }
    }

}
