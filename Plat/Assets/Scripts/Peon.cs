using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using TMPro;
public class Peon : MonoBehaviour
{
    #region Variables
    #region Deplacement
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
        set { 
            m_canMove = value;
            if (_isFixing && !value)
            {
                _animator.SetBool("Healing", true); 
                _fix.SetActive(true);
            }
            else
            {
                _animator.SetBool("Healing", false);
                _fix.SetActive(false);
            }
        }
    }
    #endregion

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
    #region Personnalité
    [Header("Personnalité")]

    [SerializeField]
    private PeonInfo m_peonInfo;
    public PeonInfo _peonInfo { get { return m_peonInfo; } }
    public TYPE _type
    {
        get { return _peonInfo.TYPE; }
    }

    private int m_ID;
    public int _ID
    {
        get { return m_ID; }
    }
    static int m_nextID;
    public int _nextID
    {
        get { return m_nextID++; }
    }

    #endregion
    

    #region Gestion HP
    [Header("PV")]
    [SerializeField]
    [Range(0, 100)]
    private float m_percentHpRecoverAfterTreat;
    private float _percentHpRecoverAfterTreat
    {
        get { return m_percentHpRecoverAfterTreat / 100; }
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
    public float _HP
    {
        get { return _peonInfo.HP; }
        set
        {
            _peonInfo.HP = value;
            GameManager.GetManager()._UIManager.UpdateHealthBar((_peonInfo.HPMax - HPLost()) / _peonInfo.HPMax, _ID);
        }
    }


    public HEALTHSTATE _HEALTHSTATE
    {
        get { return _peonInfo.HEALTHSTATE; }
        set { _peonInfo.HEALTHSTATE = value; }
    }

    private float _hpToRecover;
    private float _recoverTimer;
    #endregion

    [Header("Fix State")]
    [SerializeField]
    [Range(0, 100)]
    private float _fixLuck;
    [SerializeField]
    private float _fixCD;
    private bool m_isFixing;
    public bool _isFixing
    {
        get { return m_isFixing; }
        set
        {
            m_isFixing = value;
            _fixTimer = 0;
            if (!_canMove)
            {
                _animator.SetBool("Healing", value);
                _fix.SetActive(value);
            }
        }
    }
    private float _fixTimer;

    #region Enum & Struct
    [System.Serializable]
    public class PeonInfo
    {
        public string name;
        public Sprite visual;
        [HideInInspector]
        public HEALTHSTATE HEALTHSTATE;
        public TYPE TYPE;
        public float HP;
        [HideInInspector]
        public float HPMax;

    }
    public enum HEALTHSTATE
    {
        HURT,
        TREAT,
        GOOD
    }

    public enum TYPE
    {
        HEALER,
        MECA,
        SIMPLE,
        FIGHTER
    }
    #endregion

    private GameObject m_over;
    public GameObject _over
    {
        get { return m_over; }
        set { m_over = value; }
    }

    private GameObject m_fix;
    public GameObject _fix
    {
        get { return m_fix; }
        set { m_fix = value; }
    }

    private Animator m_animator;
    public Animator _animator
    {
        get { return m_animator; }
    }

    private MeshRenderer _meshRenderer;
    #endregion

    void Start()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        m_ID = _nextID;
        _mentalHealth = 100;
        GameManager.GetManager()._peonManager.AddPeon(this);
        _peonInfo.HPMax = _HP;
        SetDamage();
        SpecialStart();
        _over.SetActive(false);
        m_animator = GetComponentInChildren<Animator>();
    }

    public virtual void SpecialStart() { }

    private void OnMouseDown()
    {
        if (GameManager.GetManager().phaseManager.freezeControl) return;
        GameManager.GetManager()._peonManager._activePeon = this;
    }
    private void OnMouseEnter()
    {
        GameManager.GetManager()._UIManager.ChangeCursor("peon");
    }
    private void OnMouseOver()
    {
        _over.SetActive(true);
    }
    private void OnMouseExit()
    {
        _over.SetActive(false);
        GameManager.GetManager()._UIManager.ChangeCursor("default");
    }
    private void Update()
    {
        if (_canMove)
        {
            if (Vector3.Distance(transform.position, m_destination) <= 0.1f)
            {
                m_destination = m_subDestination;
                if (Vector3.Distance(transform.position, m_subDestination) <= 0.1f)
                {
                    _canMove = false;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, m_destination, Time.deltaTime * m_speed);
        }
        else if (m_currentCarriage!= null && !m_currentCarriage._activePeons.Contains(this) && m_currentCarriage._peons.Contains(this))
        {
            m_currentCarriage._activePeons.Add(this);
            m_currentCarriage.AddPeonToSpecialCarriage(this);
        }

        Fix();
        Recover();
        SpecialUpdate();
    }

    public virtual void SpecialUpdate() { }

    private void Fix()
    {
        if (!_isFixing || m_canMove) return;
        _fixTimer += Time.deltaTime;
        if (_fixTimer > _fixCD)
        {
            float random = Random.Range(0, 100);
            if (random > _fixLuck)
            {
                //c'est reparé
                _currentCarriage._isBroke = false;
            }
            else
            {
                _currentCarriage._isBroke = true;

            }
            _fixTimer = 0;
            _isFixing = false;
            
        }
    }

    private void Recover()
    {
        if (_HEALTHSTATE != HEALTHSTATE.TREAT) return;
        _recoverTimer += Time.deltaTime;
        if (_recoverTimer > 1)
        {
            _HP += _hpToRecover * _percentHpRecoverPerSecond;
            _recoverTimer -= 1;
            if (_HP >= _peonInfo.HPMax)
            {
                _recoverTimer = 0;
                _HEALTHSTATE = HEALTHSTATE.GOOD;
                _HP = _peonInfo.HPMax;
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
        return _peonInfo.HPMax - _HP;
    }

    public void SetDamage()
    {
        _HEALTHSTATE = HEALTHSTATE.HURT;
        _HP -= 10;
    }

    public void SwitchMaterial(Material mat)
    {
        //_meshRenderer.material = mat;
    }

    public virtual bool CanFix(Carriage carriage)
    {
        return true;
    }
}
