﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Carriage : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int m_capacity = 3;
    private int m_id;
    public int id
    {
        get { return m_id; }
        set { m_id = value; }
    }

    [System.Serializable]
    public class positions
    {
        public Transform position;
        public bool isAvailable;
        public Peon peonOnPos;
    }

    [SerializeField]
    private List<positions> m_subDestinations = new List<positions>();

    [SerializeField]
    private List<Peon> m_peons = new List<Peon>(3);
    public List<Peon> _peons
    {
        get { return m_peons; }
        set { m_peons = value; }
    }
    [SerializeField]
    private List<Peon> m_activePeons = new List<Peon>(3);
    public List<Peon> _activePeons
    {
        get { return m_activePeons; }
        set { m_activePeons = value; }
    }

    private bool m_underAttack = false;
    public bool _underAttack
    {
        get { return m_underAttack; }
        set {
            m_underAttack = value;
            UIManager.Instance._isOnAttack = value;
            _light.SetBool("isAttack", value);
        }
    }
    [SerializeField]
    private bool m_willBeAttacked = false;
    public bool _willBeAttacked
    {
        get { return m_willBeAttacked; }
        set {
            m_willBeAttacked = value;
            _light.SetBool("willBeAttack", value);
            UIManager.Instance._isOnAttack = value;
        }
    }

    [SerializeField]
    private Transform m_mainDestination;

    [Header("Attack Timers")]
    [SerializeField]
    private float _timeBeforeAttack = 5f;
    private float _timerBeforeAttack = 0f;

    [SerializeField]
    private float _attackDuration = 30f;
    private float _attackTimer = 0f;

    [SerializeField]
    private float _fightDuration = 10f;
    private float _fightTimer = 0f;

    private ParticleSystem _particle;
    private FixIt _fixIt;
    public FixIt fixIt
    {
        get { return _fixIt; }
        set { _fixIt = value; }
    }
    [SerializeField]
    private bool m_isBroke;
    public bool _isBroke
    {
        private get { return m_isBroke; }
        set {
            m_isBroke = value;
            _fixIt.gameObject.SetActive(value);
            _fixIt._isOnFix = false;
        }
    }

    private TextMeshProUGUI _nameTag;
    public TextMeshProUGUI nameTag
    {
        get { return _nameTag; }
        set { _nameTag = value; }
    }

    [SerializeField]
    private List<Carriage> _nextCarriages;
    public List<Carriage> nextCarriage
    {
        get { return _nextCarriages; }
        set { _nextCarriages = value; }
    }

    private Animator _light;

    public GameObject _battleUi;
    private bool _isAnEvent = false;
    public bool isAnEvent
    {
        get { return _isAnEvent; }
        set { _isAnEvent = value; }
    }
    [SerializeField]
    private bool _autoWin = false;
    public bool autoWin
    {
        get { return _autoWin; }
        set { _autoWin = value; }
    }
    #region Varible pour separation

    private bool _isDetached;

    public bool isDetached
    {
        set { _isDetached = value; }
    }

    [Header("Varible separation")]
    [SerializeField]
    private float _speed;

    #endregion
    #region Dégat Carriage
    public enum DEGATSTATE
    {
        GOOD,
        DEGAT20,
        DEGAT40,
        DEGAT60,
        DEGAT80
    }
    private DEGATSTATE _degatState = DEGATSTATE.GOOD;
    public DEGATSTATE DegatState
    {
        private set
        {
            TrainManager.Instance.UpdateSpeed(_degatState, value);
            _degatState = value;

        }
        get { return _degatState; }
    }
    #endregion
    #endregion

    private void Start()
    {
        _fixIt = GetComponentInChildren<FixIt>(true);
        _light = transform.parent.GetComponentInChildren<Light>().GetComponent<Animator>();
        if(_fixIt) _fixIt.Setup(this);
        _particle = transform.parent.GetComponentInChildren<ParticleSystem>();
        _particle.Stop();
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (PhaseManager.Instance.activePhase.freezeControl) return;
        if (PeonManager.Instance._activePeon != null && m_peons.Count < m_capacity && !m_peons.Contains(PeonManager.Instance._activePeon))
        {
            TrainManager.Instance.MovePeonToCarriage(PeonManager.Instance._activePeon,this);
        }
    }

    private void OnMouseEnter()
    {
        if (PhaseManager.Instance.activePhase.freezeControl) return;
        if(_underAttack)
        {
            UIManager.Instance.ChangeCursor("attack");
        }
        if(!_underAttack)
        {
            _nameTag.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (_underAttack)
        {
            UIManager.Instance.ChangeCursor("default");
        }
        _nameTag.gameObject.SetActive(false);
    }

    #region List Management


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Peon")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Peon")
        {
            other.transform.parent = null;
        }
    }

    public virtual void AddPeonToSpecialCarriage(Peon peon)
    {

    }

    public virtual void RemovePeon(Peon peon)
    {
        m_peons.Remove(peon);
        if (m_activePeons.Contains(peon))
        {
            m_activePeons.Remove(peon);
        }
        if (_fixIt._activePeon &&_fixIt._activePeon._ID == peon._ID)
            _fixIt._isOnFix = false;
    }
    #endregion

    #region Position Management
    public void ClearPeon(Peon currentPeon)
    {
        if (currentPeon._currentCarriage == null) return;
        positions lastPos = currentPeon._currentCarriage.m_subDestinations.Find(x => x.peonOnPos != null && x.peonOnPos._ID == currentPeon._ID);
        lastPos.isAvailable = true;
        lastPos.peonOnPos = null;
        currentPeon._currentCarriage.RemovePeon(currentPeon);
    }
    public void GetFreePos(Peon currentPeon, Vector3 actionPositon = new Vector3())
    {
        currentPeon._destination = m_mainDestination.position;
        positions freepos = m_subDestinations.Find(x => x.isAvailable == true);
        if (actionPositon == new Vector3())
            currentPeon._destination = m_mainDestination.position;
        else
        {
            currentPeon._destination = actionPositon;
            currentPeon._actionBeforeIdle = true;
        }
        currentPeon._subDestination = freepos.position.position;
        freepos.isAvailable = false;
        freepos.peonOnPos = currentPeon;
    }
    #endregion

    #region Attack Management

    public void Attack(float duration,float subduration)
    {
        _timeBeforeAttack = duration;
        _attackDuration = subduration;
        _willBeAttacked = true;
    }

    private void CheckFight()
    {
        if (_willBeAttacked)
        {
            _timerBeforeAttack += Time.deltaTime;
            if (_timerBeforeAttack >= _timeBeforeAttack)
            {
                _particle.Play();
                _willBeAttacked = false;
                _underAttack = true;
                _attackTimer = 0f;
            }
        }
        if (m_underAttack)
        {
            _attackTimer += Time.deltaTime;
            if (m_activePeons.Count >= 1)
            {
                _battleUi.SetActive(true);
                _fightTimer += Time.deltaTime;
                if (_fightTimer >= _fightDuration)
                {
                    Fight();
                    _fightTimer = 0f;
                }
            }
            if (_attackTimer >= _attackDuration)
            {
                PhaseManager.Instance.NextPhase();
                _particle.Stop();
                DestructCarriage();
                _timeBeforeAttack = 0f;
                _underAttack = false;
            }
        }
    }
    private void Fight()
    {
        if(_isAnEvent)
        {
            if(autoWin)
            {
                Victory();
            }
            if(!autoWin)
            {
                PhaseManager.Instance.GetPeon(m_activePeons[0]);
                _underAttack = false;
                _particle.Stop();
                m_activePeons[0]._HP = 0;
                PhaseManager.Instance.NextPhase();
            }
            _battleUi.SetActive(false);
        }
        else
        {
            int totalpower = 0;

            switch (m_activePeons[0]._type)
            {
                case Peon.TYPE.FIGHTER:
                    totalpower = 70;
                    break;
                case Peon.TYPE.SIMPLE:
                    totalpower = 40;
                    break;
                case Peon.TYPE.MECA:
                    totalpower = 35;
                    break;
                case Peon.TYPE.HEALER:
                    totalpower = 20;
                    break;
            }
            for (int i = 1; i < m_activePeons.Count; i++)
            {
                switch (m_activePeons[i]._type)
                {
                    case Peon.TYPE.FIGHTER:
                        totalpower += 20;
                        break;
                    case Peon.TYPE.SIMPLE:
                        totalpower += 15;
                        break;
                    case Peon.TYPE.MECA:
                        totalpower += 10;
                        break;
                    case Peon.TYPE.HEALER:
                        totalpower += 5;
                        break;
                }
            }
            int rand = Random.Range(0, 100);
            Debug.Log("Puissance Totale = " + totalpower);
            Debug.Log("Jet de Dés = " + rand);
            if (rand <= totalpower)
            {
                Debug.Log("Victoire ! ");
                Victory();
            }
            else
            {
                Debug.Log("Défaite :(");
                Defeat();
            }
            _timerBeforeAttack = 0f;
            _battleUi.SetActive(false);
        }
    }

    private void Victory()
    {
        PhaseManager.Instance.GetPeon(m_activePeons[0]);
        _underAttack = false;
        _particle.Stop();
        m_activePeons[0]._HP -= 10;
        m_activePeons[0]._HEALTHSTATE = Peon.HEALTHSTATE.HURT;
        PhaseManager.Instance.NextPhase();
    }

    private void Defeat()
    {
        m_activePeons[0]._HP -= 15;
        m_activePeons[0]._HEALTHSTATE = Peon.HEALTHSTATE.HURT;

        if (_nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count))
        {
            TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], _nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count));
        }
        else
        {
            TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], TrainManager.Instance._carriages.Find((x => x.m_capacity > x.m_activePeons.Count && x != this)));
        }
        int count = m_activePeons.Count;
        for(int i = 1; i < count; i++)
        {
            m_activePeons[0]._HP -= 10;
            m_activePeons[0]._HEALTHSTATE = Peon.HEALTHSTATE.HURT;

            if (_nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count))
            {
                TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], _nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count));
            }
            else
            {
                TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], TrainManager.Instance._carriages.Find((x => x.m_capacity > x.m_activePeons.Count && x != this)));
            }
        }

    }

    private void DestructCarriage()
    {
        PhaseManager.Instance.NextPhase();
        Debug.Log("Le wagon a été détruit");
    }
    #endregion

    private void Update()
    {
        CheckFight();
        RunAway();
    }

    private void RunAway()
    {
        if (!_isDetached) return;
        transform.parent.transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
