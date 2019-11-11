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
    private float _speed = 100;
    [SerializeField]
    private float _acceleration = 5;
    [SerializeField]
    private float _deceleration = 10;
    [SerializeField]
    private float _wagonSpeedMalus = 10;
    [SerializeField]
    private float _maxDamagedWagonSpeedMalus = 20;
    [SerializeField]
    private float _attackSpeedMalus = 5;
    private float _speedTarget;

    private GameObject carriageToAdd;
    private bool carriageWithPerso;

    public float Speed
    {
        get { return _speed; }
    }

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
        _speedTarget = _speed;
        foreach (var item in m_carriages)
        {
            _speedTarget -= _wagonSpeedMalus;
        }
        _speed = _speedTarget;
    }

    private void Update()
    {
        float dif = _speed - _speedTarget;
        if(Mathf.Abs(dif)>0.05)
        {
            if (dif > 0)
            {
                //deceleration
                _speed -= _deceleration * Time.deltaTime;
            }
            else
                _speed += _acceleration * Time.deltaTime;
        }
    }


    public void MovePeonToCarriage(Peon p, Carriage carriage, Vector3 actionPosition = new Vector3())
    {
        carriage.ClearPeon(p);
        carriage.GetFreePos(p, actionPosition);
        carriage._peons.Add(p);
        p.MovePeon(carriage);
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

    public void UnclipCarriage(int carriageID)
    {
        List<Carriage> toRemove = new List<Carriage>();
        for (int i = carriageID+1; i < m_carriages.Count; i++)
        {
            m_carriages[i].isDetached = true;
            _speedTarget += _wagonSpeedMalus;
            toRemove.Add(m_carriages[i]);
        }
        foreach (var item in toRemove)
        {
            m_carriages.Remove(item);
            UIManager.Instance.RemoveCarriageName(item);
        }
        CameraController.Instance.MajCamera(m_carriages);
    }

    public void UpdateSpeed(Carriage.DEGATSTATE currentState, Carriage.DEGATSTATE newState)
    {
        switch (currentState)
        {
            case Carriage.DEGATSTATE.DEGAT20:
                _speedTarget += (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 4;
                break;
            case Carriage.DEGATSTATE.DEGAT40:
                _speedTarget += (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 2;
                break;
            case Carriage.DEGATSTATE.DEGAT60:
                _speedTarget += ((_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 4)*3;
                break;
            case Carriage.DEGATSTATE.DEGAT80:
                _speedTarget += (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus);
                break;
        }
        switch (newState)
        {
            case Carriage.DEGATSTATE.DEGAT20:
                _speedTarget -= (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 4;
                break;
            case Carriage.DEGATSTATE.DEGAT40:
                _speedTarget -= (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 2;
                break;
            case Carriage.DEGATSTATE.DEGAT60:
                _speedTarget -= ((_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 4) * 3;
                break;
            case Carriage.DEGATSTATE.DEGAT80:
                _speedTarget -= (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus);
                break;
        }
    }

    public void UpdateSpeed(bool ancienBool, bool newBool)
    {
        if (ancienBool)
            _speedTarget += _attackSpeedMalus;
        if (newBool)
            _speedTarget -= _attackSpeedMalus;
    }

    public void RecupererWagon(bool oui)
    {
         
    }

}
