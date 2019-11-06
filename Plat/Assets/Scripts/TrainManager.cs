using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    #region Singleton
    private static TrainManager _instance = null;

    public static TrainManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private List<Carriage> m_carriages = new List<Carriage>();
    public List<Carriage> _carriages
    {
        get { return m_carriages; }
        set { m_carriages = value; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            _instance = this;
    }
    private void Start()
    {
        UpdateId();
    }
    public void MovePeonToCarriage(Peon p,Carriage carriage)
    {
        carriage.ClearPeon(p);
        carriage.GetFreePos(p);
        carriage._peons.Add(p);
        p._currentCarriage = carriage;
        p._canMove = true;
        PeonManager.Instance._activePeon = null;
    }

    public void UpdateId()
    {
        foreach (Carriage c in m_carriages)
        {
            c.id = m_carriages.IndexOf(c);
            UIManager.Instance.AddCarriageName(c);
        }
    }
}
