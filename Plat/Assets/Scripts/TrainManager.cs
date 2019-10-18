using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class TrainManager : MonoBehaviour
{
    [SerializeField]
    private List<Carriage> m_carriages = new List<Carriage>();
    public List<Carriage> _carriages
    {
        get { return m_carriages; }
        set { m_carriages = value; }
    }

    private void Awake()
    {
        GameManager.GetManager()._trainManager = this; 
        UpdateId();
    }
    public void MovePeonToCarriage(Peon p,Carriage carriage)
    {
        carriage.ClearPeon(p);
        carriage.GetFreePos(p);
        carriage._peons.Add(p);
        p._canMove = true;
        p._currentCarriage = carriage;
        GameManager.GetManager()._peonManager._activePeon = null;
    }

    public void UpdateId()
    {
        foreach (Carriage c in m_carriages)
            c.id = m_carriages.IndexOf(c);
    }
}
