using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
public class Carriage : MonoBehaviour
{
    [SerializeField]
    private int m_capacity = 3;
    [SerializeField]
    private List<Transform> m_subDestinations = new List<Transform>(3);
    [SerializeField]
    private List<Peon> m_Peons = new List<Peon>(3);
    [SerializeField]
    private Transform _mainDestination;

    private void OnMouseDown()
    {
        if(m_Peons.Count <= m_capacity)
        {
            GameManager.GetManager()._peonManager._activePeon._destination = _mainDestination.position;
            m_Peons.Add(GameManager.GetManager()._peonManager._activePeon);
            GameManager.GetManager()._peonManager._activePeon._subDestination = m_subDestinations[m_Peons.IndexOf(GameManager.GetManager()._peonManager._activePeon)].position;
            GameManager.GetManager()._peonManager._activePeon._canMove = true;
            GameManager.GetManager()._peonManager._activePeon = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Peon")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Peon")
        {
            other.transform.parent = null;
        }
    }
}
