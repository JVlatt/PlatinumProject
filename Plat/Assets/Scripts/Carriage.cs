using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
public class Carriage : MonoBehaviour
{
    private void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit))
        {
            if(hit.transform == this.transform && GameManager.GetManager()._peonManager._activePeon != null)
            {
                GameManager.GetManager()._peonManager._activePeon._destination = hit.transform.position;
                GameManager.GetManager()._peonManager._activePeon._canMove = true;
                GameManager.GetManager()._peonManager._activePeon = null;
            }
        }
    }
}
