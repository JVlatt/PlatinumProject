using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        set
        {
            m_canMove = value;
            m_animator.SetBool("isWalking", value);
            if (_isFixing && !value)
            {
                SoundManager.Instance.Play("fix");
                _currentCarriage._qteFix.Launch(this);
                transform.forward = Vector3.back;
                m_fix.SetActive(true);
            }
            else
            {
                m_animator.SetBool("Healing", false);
                m_fix.SetActive(false);
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
            UIManager.Instance.UpdateMentalBar();
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
    public GameObject _masque;
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
            if (PeonManager.Instance._activePeon == this)
                UIManager.Instance.UpdateUIPeon(_peonInfo,m_ID);
            _peonInfo.HP = value;
            UIManager.Instance.UpdateUIPeon(_peonInfo, _ID);
        }
    }


    public HEALTHSTATE _HEALTHSTATE
    {
        get { return _peonInfo.HEALTHSTATE; }
        set { 
            _peonInfo.HEALTHSTATE = value;
            UIManager.Instance.UpdateUIPeon(_peonInfo, _ID);
        }
    }

    private float _hpToRecover;
    private float _recoverTimer;
    #endregion
    #region Fix
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
            if (_isFixing && !value)
            {
                _ACTIVITY = ACTIVITY.NONE;
                m_currentCarriage._qteFix.Reset();
            }
            else if (value)
            {
                _ACTIVITY = ACTIVITY.REPAIR;

            }
            m_animator.SetBool("isFixing", value);
            m_isFixing = value;
            _fixTimer = 0;
            if (!_canMove)
            {
                if(_type == TYPE.HEALER)
                    m_animator.SetBool("Healing", value);
                if(value)
                    m_fix.SetActive(value);
            }
        }
    }
    private float _fixTimer;
    protected GameObject m_fix;
    public GameObject _fix
    {
        set
        {
            m_fix = value;
            _fixAnimator = value.GetComponent<Animator>();
        }
    }
    private Animator _fixAnimator;

    private bool _fixEnded;
    #endregion
    #region Enum & Struct
    [System.Serializable]
    public class PeonInfo
    {
        public string name;
        public Sprite visual;
        [HideInInspector]
        public HEALTHSTATE HEALTHSTATE;
        public TYPE TYPE;
        public ACTIVITY ACTIVITY;
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

    public enum ACTIVITY
    {
        NONE,
        HEAL,
        WAIT,
        FIGHT,
        REPAIR,
        DRIVE,
        UNCLIP
    }
    #endregion

    public ACTIVITY _ACTIVITY
    {
        get { return _peonInfo.ACTIVITY; }
        set
        {
            _peonInfo.ACTIVITY = value;
            UIManager.Instance.UpdateUIPeon(_peonInfo, _ID);
        }
    }

    public delegate void Del(bool b);
    public Del onFixEndedDelegate;

    private bool m_actionBeforIdle;
    public bool _actionBeforeIdle
    {
        set { m_actionBeforIdle = value; }
    }

    protected Animator m_animator;
    public bool isTalking
    {
        set { m_animator.SetBool("isTalking", value); }
    }

    private SkinnedMeshRenderer[] _meshRenderers;
    #endregion

    void Start()
    {
        m_ID = _nextID;
        PeonManager.Instance.AddPeon(this,m_ID);
        _meshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        _mentalHealth = 100;
        _HEALTHSTATE = HEALTHSTATE.GOOD;
        _peonInfo.HPMax = _HP;
        SpecialStart();
        m_animator = GetComponentInChildren<Animator>();
        if(_peonInfo.name == "Butor")
        {
            SetDamage(10);
        }
    }

    public virtual void SpecialStart() { }

#if UNITY_ANDROID
    public void Touch()
    {
        if (_currentCarriage._underAttack) return;
        if (!PhaseManager.Instance.activePhase.freezeControl)
        {
            PeonManager.Instance._activePeon = this;
        }
    }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private void OnMouseOver()
    {
        if (!(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))return;
        if (_currentCarriage._underAttack) return;
        if (!PhaseManager.Instance.activePhase.freezeControl)
        {
            PeonManager.Instance._activePeon = this;
        }
    }
    private void OnMouseEnter()
    {
        if (_currentCarriage._underAttack) return;
        if (PhaseManager.Instance.activePhase.freezeControl) return;
        if (_currentCarriage._underAttack) return;
        UIManager.Instance.ChangeCursor("peon");
    }

    private void OnMouseExit()
    {
        UIManager.Instance.ChangeCursor("default");
    }
#endif

    private void Update()
    {
        if (_HP <= 0)
            Death();
        if (_canMove)
        {
            if (Vector3.Distance(transform.position, m_destination) <= 0.1f)
            {
                m_destination = m_subDestination;
                if (m_actionBeforIdle)
                {
                    _canMove = false;
                    m_actionBeforIdle = false;
                }
                if (Vector3.Distance(transform.position, m_subDestination) <= 0.1f)
                {
                    _canMove = false;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, m_destination, Time.deltaTime * m_speed);
            transform.forward = transform.position-m_destination;
        }
        else if (m_currentCarriage != null && !m_currentCarriage._activePeons.Contains(this) && m_currentCarriage._peons.Contains(this))
        {
            if (m_currentCarriage._underAttack)
                _ACTIVITY = ACTIVITY.FIGHT;
            m_currentCarriage._activePeons.Add(this);
            m_currentCarriage.AddPeonToSpecialCarriage(this);
        }

        Fix();
        Recover();
        SpecialUpdate();
    }

    public void Death()
    {
        _currentCarriage.RemovePeon(this);
        GameObject masque = Instantiate(_masque);
        masque.transform.parent = transform.parent;
        masque.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        masque.transform.eulerAngles = new Vector3(0, 90, -90);
        Destroy(gameObject);
        m_nextID = m_ID;
    }

    public virtual void SpecialUpdate() { }

    private void Fix()
    {
        if (!_isFixing && !_fixEnded || m_canMove) return;
        _fixTimer += Time.deltaTime;
        if (!_fixEnded)
            transform.forward = Vector3.back;
        else
            transform.forward = Vector3.forward;
        if (_fixTimer > _fixCD)
        {
            EndFix(false);
        }
        else if (_fixEnded)
        {
            if (_fixTimer > 0.8f)
            {
                _fixEnded = false;
                _isFixing = false;
                _canMove = true;
            }

        }
    }

    public void EndFix(bool win)
    {
        float random = Random.Range(0, 100);
        if (win)
        {
            //c'est reparé
            if (null != onFixEndedDelegate)
                onFixEndedDelegate(true);
            _fixAnimator.SetTrigger("Win");
            m_animator.SetBool("FixingWin", true);
        }
        else
        {
            if (onFixEndedDelegate != null)
                onFixEndedDelegate(false);
            _fixAnimator.SetTrigger("Fail");
            m_animator.SetBool("FixingWin", false);
        }
        _fixEnded = true;
        m_animator.SetBool("isFixing", false);
        m_animator.SetTrigger("EndFix");
        if (_currentCarriage.isAnEvent)
        {
            PhaseManager.Instance.GetPeon(this);
        }
        _fixTimer = 0;
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

    public virtual void MovePeon(Carriage carriage)
    {
        _currentCarriage = carriage;
        _canMove = true;
    }

    public float HPLost() //CEDRIC
    {
        return _peonInfo.HPMax - _HP;
    }

    public void SetDamage(int damages)
    {
        _HEALTHSTATE = HEALTHSTATE.HURT;
        _HP -= damages;
    }

    public void SwitchMaterial(Material mat)
    {
        foreach (var item in _meshRenderers)
        {
            if (item)
            {
                Material[] newMaterial = new Material[2];
                newMaterial[0] = item.materials[0];
                newMaterial[1] = mat;
                item.materials = newMaterial;
            }
        } 
    }

    public virtual bool CanFix(Carriage carriage)
    {
        return true;
    }

    public bool CanBeSelect()
    {
        return !m_currentCarriage._underAttack && !m_currentCarriage.isDetached;
    }
}
