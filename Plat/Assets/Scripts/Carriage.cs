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
    private List<Peon> m_peons = new List<Peon>(3);
    public List<Peon> _peons
    {
        get { return m_peons; }
        set { m_peons = value; }
    }

    [SerializeField]
    private Transform m_mainDestination;

    private void OnMouseDown()
    {
        if(m_peons.Count < m_capacity)
        {
            GameManager.GetManager()._peonManager._activePeon._destination = m_mainDestination.position;
            m_peons.Add(GameManager.GetManager()._peonManager._activePeon);
            AddPeonToSpecialCarriage(GameManager.GetManager()._peonManager._activePeon);
            GameManager.GetManager()._peonManager._activePeon._subDestination = m_subDestinations[m_peons.IndexOf(GameManager.GetManager()._peonManager._activePeon)].position;
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
        //Ajouter les tags des différents rôles
        if(other.tag == "Peon")
        {
            if(m_peons.Contains(other.GetComponent<Peon>()))
            {
                RemovePeon(other.GetComponent<Peon>());
            }
            other.transform.parent = null;
        }
    }

    public virtual void AddPeonToSpecialCarriage(Peon peon)
    {

    }

    public virtual void RemovePeon(Peon peon)
    {
        m_peons.Remove(peon);
    }
}
