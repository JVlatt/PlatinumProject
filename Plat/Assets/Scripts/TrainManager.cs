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
        if (_startTimer)
        {
            _timerChoice += Time.deltaTime;
            UIManager.Instance.choiceClock.fillAmount = _timerChoice / _cDChoice;
            if (_timerChoice > _cDChoice)
            {
                RecupererWagon(false);
            }
        }
    }


    public void MovePeonToCarriage(Peon p, Carriage carriage, Vector3 actionPosition = new Vector3())
    {
        if (carriage.id == 0 && p._type == Peon.TYPE.MECA)
        {
            TrainManager.Instance.UpdateSpeed(true);
            p._ACTIVITY = Peon.ACTIVITY.DRIVE;
        }
        else if(p._ACTIVITY == Peon.ACTIVITY.DRIVE)
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

    public void UnclipCarriage(int carriageID)
    {
        List<Carriage> toRemove = new List<Carriage>();
        for (int i = carriageID + 1; i < m_carriages.Count; i++)
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
                _speedTarget += ((_maxDamagedWagonSpeedMalus - _wagonSpeedMalus) / 4) * 3;
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

    public void UpdateSpeed(bool addMeca)
    {
        if (addMeca)
            _speedTarget += _driverSpeedBonus;
        else
            _speedTarget -= _driverSpeedBonus;
    }

    public void RecupererWagon(bool oui)
    {
        if (oui)
            UIManager.Instance.fade(UIManager.FADETYPE.ADDCARRIAGE);
        PhaseManager.Instance.NextPhase();
        UIManager.Instance.choicePannel.SetActive(false);
        _timerChoice = 0;
        _startTimer = false;
    }

    public void AddCarriage()
    {
        Vector3 position = _carriages[_carriages.Count-1].transform.parent.position;
        position.x -= 10f;
        GameObject go = Instantiate(_carriageToAdd, position, _carriageToAdd.transform.rotation, transform);
        Carriage carriage = go.GetComponentInChildren<Carriage>();
        m_carriages.Add(carriage);
        AddId(carriage);
        CameraController.Instance.MajCamera(m_carriages);
        if (_withPeon)
            PeonManager.Instance.AddPeon(go.GetComponentInChildren<Peon>());
    }

    public void EventNewCarriage(GameObject carriage, float timer,bool withPeon)
    {
        _withPeon = withPeon;
        _carriageToAdd = carriage;
        _cDChoice = timer;
        _startTimer = true;
        UIManager.Instance.choicePannel.SetActive(true);
    }


    public AttackedCariageDirection CheckAttackedCariageDirection()
    {
        AttackedCariageDirection direction = new AttackedCariageDirection();
        foreach (Carriage item in m_carriages)
        {
            if (item._underAttack)
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

        if(PhaseManager.Instance.activePhase.GetPhaseType() == Phase.PhaseType.SHUTDOWN)
            PhaseManager.Instance.EndCondition(true);
    }

    public class AttackedCariageDirection
    {
        public bool Right;
        public bool Left;
    }
}
