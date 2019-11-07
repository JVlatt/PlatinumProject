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
        CameraController.Instance.MajCamera(m_carriages);
    }
    public void MovePeonToCarriage(Peon p, Carriage carriage, Vector3 actionPosition = new Vector3())
    {
        carriage.ClearPeon(p);
        carriage.GetFreePos(p, actionPosition);
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

    public void UnclipCarriage(Carriage carriage)
    {
        List<Carriage> toRemove = new List<Carriage>();
        for (int i = carriage.id; i < m_carriages.Count; i++)
        {
            m_carriages[i].isDetached = true;
            toRemove.Add(m_carriages[i]);
        }
        foreach (var item in toRemove)
        {
            m_carriages.Remove(item);
            UIManager.Instance.RemoveCarriageName(item);
        }
        CameraController.Instance.MajCamera(m_carriages);
    }
}
