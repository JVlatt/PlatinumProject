using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class Peon : MonoBehaviour
{
    #region Variables

    [Header("Déplacement")]
    [SerializeField]
    private float m_speed = 1.0f;
    public float _speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

    [SerializeField]
    private int m_power = 1;
    public int _power
    {
        get { return m_power; }
        set { m_power = value; }
    }
    private Vector3 m_destination;
    public Vector3 _destination
    {
        get { return m_destination; }
        set { m_destination = value; }
    }

    private Vector3 m_subDestination;
    public Vector3 _subDestination
    {
        get { return m_subDestination; }
        set { m_subDestination = value; }
    }

    private bool m_canMove = false;
    public bool _canMove
    {
        get { return m_canMove; }
        set { m_canMove = value; }
    }

    private float m_mentalHealth;
    public float _mentalHealth
    {
        get { return m_mentalHealth; }
        set
        {
            m_mentalHealth = value;
            GameManager.GetManager()._UIManager.UpdateMentalBar();
        }
    }

    [SerializeField]
    private Carriage m_currentCarriage;
    public Carriage _currentCarriage
    {
        get { return m_currentCarriage; }
        set { m_currentCarriage = value; }
    }

    [Header("Personnalité")]
    [SerializeField]
    private TYPE m_type;
    public TYPE _type
    {
        get { return m_type; }
    }

    private int m_ID;
    public int _ID { 
        get { return m_ID; } 
    }
    static int m_nextID;
    public int _nextID
    {
        get { return m_nextID++; }
    }

    private HEALTHSTATE m_HEALTHSTATE = HEALTHSTATE.GOOD;
    public HEALTHSTATE _HEALTHSTATE
    {
        get { return m_HEALTHSTATE; }
        set { m_HEALTHSTATE = value; }
    }

    #region Gestion HP
    [Header("PV")]
    [SerializeField]
    private float _HP;
    public float HP
    {
        get { return _HP; }
        set { _HP = value; }
    }
    private float _maxHP;

    [SerializeField]
    [Range(0, 100)]
    private float m_percentHpRecoverAfterTreat;
    private float _percentHpRecoverAfterTreat
    {
        get { return m_percentHpRecoverAfterTreat/100; }
        set { m_percentHpRecoverAfterTreat = value; }
    }
    [SerializeField]

    [Range(0, 100)]
    private float m_percentHpRecoverPerSecond;
    private float _percentHpRecoverPerSecond
    {
        get { return m_percentHpRecoverPerSecond / 100; }
        set { m_percentHpRecoverPerSecond = value; }
    }

    private float _hpToRecover;
    private float _recoverTimer;
    #endregion

    #region Enum
    public enum HEALTHSTATE
    {
        HURT,
        TREAT,
        GOOD
    }

    public enum TYPE
    {
        HEALER,
        MECANO,
        SIMPLE,
        FIGHTER
    }
    #endregion
    #endregion

    void Start()
    {
        m_ID = _nextID;
        _mentalHealth = 100;
        GameManager.GetManager()._peonManager._peons.Add(this);
        _maxHP = _HP;
    }

    private void OnMouseDown()
    {
        GameManager.GetManager()._peonManager._activePeon = this;
    }

    private void Update()
    {
        if(m_canMove)
        {
            if(Vector3.Distance(transform.position,m_destination) <= 0.1f)
            {
                m_destination = m_subDestination;
                if (Vector3.Distance(transform.position, m_subDestination) <= 0.1f)
                {
                    m_canMove = false;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, m_destination, Time.deltaTime * m_speed);
        }
        Recover();
        SpecialUpdate();
    }

    public void Recover()
    {
        if (_HEALTHSTATE != HEALTHSTATE.TREAT) return;
        _recoverTimer += Time.deltaTime;
        if (_recoverTimer > 1)
        {
            _HP += _hpToRecover * _percentHpRecoverPerSecond;
            _recoverTimer -= 1;
            if(_HP>=_maxHP)
            {
                _recoverTimer = 0;
                _HEALTHSTATE = HEALTHSTATE.GOOD;
                _HP = _maxHP;
            }
        }
    }

    public void TreatPeon() //CEDRIC
    {
        _HEALTHSTATE = HEALTHSTATE.TREAT;
        _HP += HPLost() * _percentHpRecoverAfterTreat;
        _hpToRecover = HPLost();
    }

    public float HPLost() //CEDRIC
    {
        return _maxHP - _HP;
    }

    public virtual void SpecialUpdate() { }

}
