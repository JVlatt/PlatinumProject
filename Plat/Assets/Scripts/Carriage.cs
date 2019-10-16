using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
public class Carriage : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int m_capacity = 3;

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
        set { m_underAttack = value; }
    }
    private bool m_willBeAttacked = false;
    public bool _willBeAttacked
    {
        get { return m_willBeAttacked; }
        set { m_willBeAttacked = value; }
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
    private float _fightDuration = 30f;
    private float _fightTimer = 0f;
    #endregion

    #region List Management
    private void OnMouseDown()
    {
        if(GameManager.GetManager()._peonManager._activePeon != null && m_peons.Count < m_capacity && !m_peons.Contains(GameManager.GetManager()._peonManager._activePeon))
        {

            Peon currentPeon = GameManager.GetManager()._peonManager._activePeon;
            if (currentPeon._currentCarriage != null)
            {
                positions lastPos = currentPeon._currentCarriage.m_subDestinations.Find(x => x.peonOnPos == currentPeon);
                lastPos.isAvailable = true;
                lastPos.peonOnPos = null;
                currentPeon._currentCarriage.RemovePeon(currentPeon);
                
            }
            currentPeon._destination = m_mainDestination.position;
            m_peons.Add(currentPeon);
            AddPeonToSpecialCarriage(currentPeon);
            positions freepos = m_subDestinations.Find(x => x.isAvailable == true);
            currentPeon._destination = m_mainDestination.position;
            currentPeon._subDestination = freepos.position.position;
            freepos.isAvailable = false;
            freepos.peonOnPos = currentPeon;
            currentPeon._canMove = true;
            
            
            currentPeon._currentCarriage = this;

            GameManager.GetManager()._peonManager._activePeon = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Peon")
        {
            other.transform.parent = this.transform;
            if(m_peons.Find(gameObject => other.gameObject))
            {
                m_activePeons.Add(other.GetComponent<Peon>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Ajouter les tags des différents rôles
        if(other.tag == "Peon")
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
        if(m_activePeons.Contains(peon))
        {
            m_activePeons.Remove(peon);
        }
    }
    #endregion

    #region Attack Management



    #endregion

    private void Update()
    {
        if(m_willBeAttacked)
        {
            _timerBeforeAttack += Time.deltaTime;
            if(_timerBeforeAttack >= _timeBeforeAttack)
            {
                m_underAttack = true;
                m_willBeAttacked = false;
                _attackTimer = 0f;
            }
        }
        if(m_underAttack)
        {
            _attackTimer += Time.deltaTime;

            if(_attackTimer >= _attackDuration)
            {
                _timeBeforeAttack = 0f;
                m_underAttack = false;
            }
        }

    }
}
