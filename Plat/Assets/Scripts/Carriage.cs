using System.Collections;
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
        set
        {
            TrainManager.Instance.UpdateSpeed(_underAttack, value);
            m_underAttack = value;
            _monster.gameObject.SetActive(value);
            UIManager.Instance._isOnAttack = value;
            _light.SetBool("isAttack", value);
        }
    }
    [SerializeField]
    private bool m_willBeAttacked = false;
    public bool _willBeAttacked
    {
        get { return m_willBeAttacked; }
        set
        {
            m_willBeAttacked = value;
            _light.SetBool("willBeAttack", value);
            UIManager.Instance._isOnAttack = true;
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

    private bool _fighting = false;
    private QTEScript _qteFight;
    [HideInInspector]
    public QTERepair _qteFix;
    private ParticleSystem _particle;
    private FixIt _fixItCarriage;
    private FixIt _fixItLight;
    [SerializeField]
    private bool m_isBroke;
    [SerializeField]
    private Transform _monster;


    public bool _isBroke
    {
        private get { return m_isBroke; }
        set
        {
            m_isBroke = value;
            _fixItCarriage.gameObject.SetActive(value);
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

    public Animator _battleUi;
    private bool _isAnEvent = false;
    public bool isAnEvent
    {
        get { return _isAnEvent; }
        set { _isAnEvent = value; }
    }
    public bool autoLoose = false;
    #region Varible pour separation

    private bool _isDetached;

    public bool isDetached
    {
        set { _isDetached = value; }
        get { return _isDetached; }
    }

    [Header("Varible separation")]
    [SerializeField]
    private float _speed;

    #endregion
    #region Dégat Carriage
    public enum DEGATSTATE
    {
        GOOD,
        DEGAT33,
        DEGAT66,
    }
    private DEGATSTATE _degatState = DEGATSTATE.GOOD;
    public DEGATSTATE DegatState
    {
        set
        {
            TrainManager.Instance.UpdateSpeed(_degatState, value);
            _degatState = value;

            switch (_degatState)
            {
                case DEGATSTATE.GOOD:
                    _isBroke = false;
                    meshRenderer.material = damageStates[0].mat;
                    meshFilter.mesh = damageStates[0].mesh;
                    break;
                case DEGATSTATE.DEGAT33:
                    meshRenderer.material = damageStates[1].mat;
                    meshFilter.mesh = damageStates[1].mesh;
                    _isBroke = true;
                    break;
                case DEGATSTATE.DEGAT66:
                    meshRenderer.material = damageStates[2].mat;
                    meshFilter.mesh = damageStates[2].mesh;
                    _isBroke = true;
                    break;
            }
        }
        get { return _degatState; }
    }

    [Header("damageWagon")]
    [SerializeField]
    private List<DamageState> damageStates = new List<DamageState>();

    [System.Serializable]
    public class DamageState
    {
        public Material mat;
        public Mesh mesh;
    }

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    #endregion

    #endregion

    private void Start()
    {
        meshRenderer = transform.parent.GetComponent<MeshRenderer>();
        meshFilter = transform.parent.GetComponent<MeshFilter>();
        _qteFight = GetComponentInChildren<QTEScript>();
        _qteFix = GetComponentInChildren<QTERepair>();
        FixIt[] allFixIt = GetComponentsInChildren<FixIt>(true);
        _fixItCarriage = allFixIt[0];
        _fixItLight = allFixIt[1];
        _light = transform.parent.GetComponentInChildren<Light>().GetComponent<Animator>();
        if (_fixItCarriage) _fixItCarriage.Setup(this);
        if (_fixItLight) _fixItLight.Setup(this);
        _particle = transform.parent.GetComponentInChildren<ParticleSystem>();
        _particle.Stop();
    }

#if UNITY_ANDROID
    public void Touch()
    {
        if (PhaseManager.Instance.activePhase.freezeControl) return;
        if (PeonManager.Instance._activePeon != null && m_peons.Count < m_capacity && !m_peons.Contains(PeonManager.Instance._activePeon))
        {
            PhaseManager.Instance.eventPeon = PeonManager.Instance._activePeon._peonInfo.name;
            TrainManager.Instance.MovePeonToCarriage(PeonManager.Instance._activePeon, this);
        }
    }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private void OnMouseOver()
    {
        if (!(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) return;
        if (PhaseManager.Instance.activePhase.freezeControl) return;
        if (PeonManager.Instance._activePeon != null && m_peons.Count < m_capacity && !m_peons.Contains(PeonManager.Instance._activePeon))
        {
            SoundManager.Instance.Play("plop");
            PhaseManager.Instance.eventPeon = PeonManager.Instance._activePeon._peonInfo.name;
            TrainManager.Instance.MovePeonToCarriage(PeonManager.Instance._activePeon, this);
        }
    }
    private void OnMouseEnter()
    {
        if (PhaseManager.Instance.activePhase.freezeControl) return;
        if (_underAttack)
        {
            UIManager.Instance.ChangeCursor("attack");
        }
        if (!_underAttack)
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
#endif
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
        if (_fixItCarriage._activePeon && _fixItCarriage._activePeon._ID == peon._ID)
            _fixItCarriage._isOnFix = false;
        if (id == 0 && peon._type == Peon.TYPE.MECA)
            TrainManager.Instance.UpdateSpeed(false);
    }
    #endregion

    #region Position Management
    public void ClearPeon(Peon currentPeon)
    {
        if (currentPeon._currentCarriage == null) return;
        if (currentPeon._isFixing)
            currentPeon._isFixing = false;
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
        FXManager.CallDelegate("MovePeon", freepos.position.position);
        freepos.isAvailable = false;
        freepos.peonOnPos = currentPeon;
    }
    #endregion

    #region Attack Management

    public void Attack(float duration, float subduration)
    {
        _timeBeforeAttack = subduration;
        _attackDuration = duration;
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
            if (m_activePeons.Count >= 1 && !_fighting)
            {
                _fighting = true;
                if (!SoundManager.Instance.isPlaying("fight"))
                {
                    SoundManager.Instance.Play("fight");
                }
                _battleUi.gameObject.SetActive(true);
                switch (m_activePeons[0].name)
                {
                    case "Oni":
                        _qteFight.Launch(20, 15, 1, 1);
                        break;
                    case "Naru":
                        _qteFight.Launch(20, 15, 1, 1);
                        break;
                    case "Butor":
                        _qteFight.Launch(10, 5, 1, 2);
                        break;
                    case "Taon":
                        _qteFight.Launch(15, 7, 1, 1.5f);
                        break;
                }
            }
            if (_attackTimer >= _attackDuration)
            {
                DamageCarriage();
                _timeBeforeAttack = 0f;
                _attackTimer = 0f;
            }
        }
    }

    public void Victory()
    {
        _timerBeforeAttack = 0f;
        if (m_activePeons.Count != 0)
        {
            PhaseManager.Instance.GetPeon(m_activePeons[0]._peonInfo.name);
            _underAttack = false;
            _particle.Stop();
            m_activePeons[0].SetDamage(2);
            foreach (Peon item in m_activePeons)
            {
                item.BattleAnim(true);
            }
            if (isCarriageAttackedByEvent())
                PhaseManager.Instance.EndCondition(true);
        }
        else
        {
            if (isCarriageAttackedByEvent())
                PhaseManager.Instance.EndCondition(true);
        }
        _fighting = false;
        _battleUi.SetBool("isWin", true);
        _battleUi.SetTrigger("EndFight");
        Invoke("DesactiveBattleUi", 2);
    }

    public void Defeat()
    {
        _battleUi.SetBool("isWin", false);
        _battleUi.SetTrigger("EndFight");
        Invoke("DesactiveBattleUi", 2);
        _fighting = false;
        _timerBeforeAttack = 0f;
        PhaseManager.Instance.GetPeon(m_activePeons[0]._peonInfo.name);
        m_activePeons[0].SetDamage(5);
        m_activePeons[0]._HEALTHSTATE = Peon.HEALTHSTATE.HURT;
        m_activePeons[0].BattleAnim(false);

        if (_nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count))
        {
            TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], _nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count));
        }
        else
        {
            TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], TrainManager.Instance._carriages.Find((x => x.m_capacity > x.m_activePeons.Count && x != this)));
        }
        int count = m_activePeons.Count;
        for (int i = 0; i < count; i++)
        {
            m_activePeons[0]._HP -= 10;
            if (m_activePeons[0]._HP < 1)
                m_activePeons[0]._HP = 1;
            m_activePeons[0]._HEALTHSTATE = Peon.HEALTHSTATE.HURT;
            m_activePeons[0].BattleAnim(false);

            if (_nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count))
            {
                TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], _nextCarriages.Find(x => x.m_capacity > x.m_activePeons.Count));
            }
            else
            {
                TrainManager.Instance.MovePeonToCarriage(m_activePeons[0], TrainManager.Instance._carriages.Find((x => x.m_capacity > x.m_activePeons.Count && x != this)));
            }
        }
        if (isCarriageAttackedByEvent())
            PhaseManager.Instance.EndCondition(false);
    }

    private void DamageCarriage()
    {
        if (_isDetached) return;
        if (DegatState == DEGATSTATE.DEGAT66)
        {
            TrainManager.Instance.UnclipCarriage(this.id - 1);
        }
        else
        {
            DegatState = _degatState + 1;
            _isBroke = true;
        }
        Debug.Log("Le wagon endomagé");
    }
    #endregion

    public void Break(Carriage.DEGATSTATE amount)
    {
        DegatState = amount;
    }

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

    public void SwitchLights(bool on, bool needRepair = false)
    {
        _light.SetBool("lightOn", on);
        if (needRepair)
            _fixItLight.gameObject.SetActive(true);
    }

    public bool isCarriageAttackedByEvent()
    {
        if (PhaseManager.Instance.activePhase.GetPhaseType() == Phase.PhaseType.ATTACK)
        {
            if (((PhaseAttack)(PhaseManager.Instance.activePhase))._carriage == id)
            {
                return true;
            }
        }
        return false;
    }

    public void DesactiveBattleUi()
    {
        _battleUi.SetTrigger("Reset");
        _battleUi.gameObject.SetActive(false);
    }
}
