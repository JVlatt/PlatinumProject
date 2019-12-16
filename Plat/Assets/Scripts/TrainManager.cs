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
    [SerializeField]
    private float _driverSpeedBonus = 10;
    private float _speedTarget;

    private GameObject _carriageToAdd;
    private bool _withPeon;
    private float _timerChoice;
    private float _cDChoice;
    private bool _startTimer;

    public bool _isShutDown = false;
    private float _shutDownTimer;
    private float _shutDownDuration;

    private List<Peon> peonToKill = new List<Peon>();

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
        foreach (var item in m_carriages)
        {
            AddId(item);
        }
        AddNeighbor();
        if (CameraController.Instance != null)
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
        if (Mathf.Abs(dif) > 0.05)
        {
            if (dif > 0)
            {
                //deceleration
                _speed -= _deceleration * Time.deltaTime;
            }
            else
                _speed += _acceleration * Time.deltaTime;
        }
        if (_carriages[0]._activePeons.Count > 0 && TrainManager.Instance._isShutDown)
        {
            TrainManager.Instance.RepairLights();
            SoundManager.Instance.Play("energy");
        }
    }


    public void MovePeonToCarriage(Peon p, Carriage carriage, Vector3 actionPosition = new Vector3())
    {
        if (carriage.id == 0 && p._type == Peon.TYPE.MECA)
        {
            TrainManager.Instance.UpdateSpeed(true);
            p._ACTIVITY = Peon.ACTIVITY.DRIVE;
        }
        else if (p._ACTIVITY == Peon.ACTIVITY.DRIVE)
        {
            TrainManager.Instance.UpdateSpeed(false);
            p._ACTIVITY = Peon.ACTIVITY.NONE;
        }
        carriage.ClearPeon(p);
        carriage.GetFreePos(p, actionPosition);
        carriage._peons.Add(p);
        p.MovePeon(carriage);
        PeonManager.Instance._activePeon = null;
    }

    public void AddId(Carriage c)
    {

        c.id = m_carriages.IndexOf(c);
        UIManager.Instance.AddCarriageName(c);

    }

    public void AddNeighbor()
    {
        foreach (var item in m_carriages)
        {
            if (item.id == 0)
            {
                item.nextCarriage.Clear();
            }
            else if (item.id == 1)
            {
                item.nextCarriage.Clear();
                if (m_carriages.Count > 2)
                    item.nextCarriage.Add(m_carriages[2]);
            }
            else if(item.id == m_carriages.Count - 1)
            {
                item.nextCarriage.Clear();
                item.nextCarriage.Add(m_carriages[m_carriages.Count - 2]);
            }
            else
            {
                item.nextCarriage.Clear();
                item.nextCarriage.Add(m_carriages[item.id - 1]);
                item.nextCarriage.Add(m_carriages[item.id + 1]);
            }
        }
    }

    public void UnclipCarriage(int carriageID)
    {
        List<Carriage> toRemove = new List<Carriage>();
        for (int i = carriageID + 1; i < m_carriages.Count; i++)
        {
            if (m_carriages[i]._underAttack)
                m_carriages[i].Victory();
            m_carriages[i].isDetached = true;
            _speedTarget += _wagonSpeedMalus;
            toRemove.Add(m_carriages[i]);
            foreach (var item in m_carriages[i]._activePeons)
            {
                item.transform.parent = item._currentCarriage.transform;
                item.enabled = false;
                peonToKill.Add(item);
                Invoke("KillPeon", 3);
            }
            foreach (var item in m_carriages[i]._peons)
            {
                item._canMove = false;
            }
        }
        foreach (var item in toRemove)
        {
            m_carriages.Remove(item);
            UIManager.Instance.RemoveCarriageName(item);
            item.GetComponent<BoxCollider>().enabled = false;
        }
        CameraController.Instance.MajCamera(m_carriages);
    }

    private void KillPeon()
    {
        if (peonToKill != null)
        {
            peonToKill[0].Death();
            peonToKill.RemoveAt(0);
        }
    }

    public void UpdateSpeed(Carriage.DEGATSTATE currentState, Carriage.DEGATSTATE newState)
    {
        switch (currentState)
        {
            case Carriage.DEGATSTATE.DEGAT33:
                _speedTarget += (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 3;
                break;
            case Carriage.DEGATSTATE.DEGAT66:
                _speedTarget += ((_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 3) * 2;
                break;
        }
        switch (newState)
        {
            case Carriage.DEGATSTATE.DEGAT33:
                _speedTarget -= (_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 4;
                break;
            case Carriage.DEGATSTATE.DEGAT66:
                _speedTarget -= ((_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 3) * 2;
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

    public void UpdateSpeed(bool addMeca)
    {
        if (addMeca)
            _speedTarget += _driverSpeedBonus;
        else
            _speedTarget -= _driverSpeedBonus;
    }

    public void RecupererWagon(GameObject carriage)
    {
        _carriageToAdd = carriage;
        UIManager.Instance.Fade(UIManager.FADETYPE.ADDCARRIAGE);
        UIManager.Instance.choicePannel.SetActive(false);
    }

    public void AddCarriage()
    {
        Vector3 position = _carriages[_carriages.Count - 1].transform.parent.position;
        position.x -= 10f;
        GameObject go = Instantiate(_carriageToAdd, position, _carriageToAdd.transform.rotation, transform);
        Carriage carriage = go.GetComponentInChildren<Carriage>();
        carriage.transform.parent.name = "Stockage Room";
        m_carriages.Add(carriage);
        AddId(carriage);
        AddNeighbor();
        CameraController.Instance.MajCamera(m_carriages);
    }

    public AttackedCariageDirection CheckAttackedCariageDirection()
    {
        AttackedCariageDirection direction = new AttackedCariageDirection();
        foreach (Carriage item in m_carriages)
        {
            if (item._underAttack || item._willBeAttacked)
            {
                if (Camera.main.transform.position.x - item.transform.position.x < 0)
                    direction.Right = true;
                else
                    direction.Left = true;
            }
        }
        return direction;
    }

    public void ShutDown(float duration)
    {
        _shutDownDuration = duration;
        _shutDownTimer = 0;

        foreach (Carriage c in _carriages)
        {
            if (c.id != 0)
                c.SwitchLights(false);
        }

        _isShutDown = true;
    }

    public void RepairLights()
    {
        _isShutDown = false;
        _shutDownTimer = 0;
        foreach (Carriage c in _carriages)
        {
            if (c.id != 0)
                c.SwitchLights(true);
        }

        if (PhaseManager.Instance.activePhase.GetPhaseType() == Phase.PhaseType.SHUTDOWN)
        {
            if (PhaseManager.Instance.activePhase.mode == Phase.PhaseMode.CONDITION)
                PhaseManager.Instance.EndCondition(true);
        }
    }

    public class AttackedCariageDirection
    {
        public bool Right;
        public bool Left;
    }
}
