using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
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
        set { m_underAttack = value; }
    }
    [SerializeField]
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
    private float _fightDuration = 10f;
    private float _fightTimer = 0f;
    #endregion

    private void OnMouseDown()
    {
        if (GameManager.GetManager()._peonManager._activePeon != null && m_peons.Count < m_capacity && !m_peons.Contains(GameManager.GetManager()._peonManager._activePeon))
        {
            GameManager.GetManager()._trainManager.MovePeonToCarriage(GameManager.GetManager()._peonManager._activePeon,this);
        }
    }

    #region List Management

    
    private void OnTriggerEnter(Collider other) //Faudrait passer en TriggerStay ça parce que si tu "annule" ton déplacement il est quand même plus dans la liste de peons
    {
        if (other.tag == "Peon")
        {
            other.transform.parent = this.transform;
            if (m_peons.Find(gameObject => other.gameObject))
            {
                Peon peonToAdd = other.GetComponent<Peon>();
                m_activePeons.Add(peonToAdd);
                AddPeonToSpecialCarriage(peonToAdd);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Ajouter les tags des différents rôles
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
    }
    #endregion

    #region Position Management
    public void ClearPeon(Peon currentPeon)
    {
        if (currentPeon._currentCarriage == null) return;
        positions lastPos = currentPeon._currentCarriage.m_subDestinations.Find(x => x.peonOnPos == currentPeon);
        lastPos.isAvailable = true;
        lastPos.peonOnPos = null;
        currentPeon._currentCarriage.RemovePeon(currentPeon);
    }
    public void GetFreePos(Peon currentPeon)
    {
        currentPeon._destination = m_mainDestination.position;
        positions freepos = m_subDestinations.Find(x => x.isAvailable == true);
        currentPeon._destination = m_mainDestination.position;
        currentPeon._subDestination = freepos.position.position;
        freepos.isAvailable = false;
        freepos.peonOnPos = currentPeon;
    }
    #endregion

    #region Attack Management


    private void CheckFight()
    {
        if (m_willBeAttacked)
        {
            _timerBeforeAttack += Time.deltaTime;
            if (_timerBeforeAttack >= _timeBeforeAttack)
            {
                m_underAttack = true;
                m_willBeAttacked = false;
                _attackTimer = 0f;
            }
        }
        if (m_underAttack)
        {
            _attackTimer += Time.deltaTime;
            if (m_activePeons.Count >= 1)
            {
                _fightTimer += Time.deltaTime;
                if (_fightTimer >= _fightDuration)
                {
                    Fight();
                    _fightTimer = 0f;
                }
            }
            if (_attackTimer >= _attackDuration)
            {
                //Mettre le résultat
                DestructCarriage();
                _timeBeforeAttack = 0f;
                m_underAttack = false;
            }
        }
    }
    private void Fight()
    {
        /* Apparemment ça saute du GD genre si jamais tu veux sacrifier un gars vu que c'est celui qui prend le focus qui perd le plus d'hp demande à clement jpense il se souviendra
        m_activePeons.Sort(delegate (Peon a, Peon b)
        {
            return a._power.CompareTo(b._power);
        });*/
        int totalpower = 0;

        switch(m_activePeons[0]._type)
        {
            case Peon.TYPE.FIGHTER:
                totalpower = 70;
                break;
            case Peon.TYPE.SIMPLE:
                totalpower = 40;
                break;
            case Peon.TYPE.MECANO:
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
                case Peon.TYPE.MECANO:
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
        if(rand <= totalpower)
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

    }

    private void Victory()
    {
        m_underAttack = false;
    }

    private void Defeat()
    {
        int count = m_activePeons.Count;
        for(int i = 0; i < count; i++)
        {
            m_activePeons[0].HP -= 1;
            GameManager.GetManager()._trainManager.MovePeonToCarriage(m_activePeons[0], GameManager.GetManager()._trainManager._carriages.Find((x => x.m_capacity > x.m_activePeons.Count && x != this)));
        }

    }

    private void DestructCarriage()
    {
        Debug.Log("Le wagon a été détruit");
    }
    #endregion

    private void Update()
    {
        CheckFight();
    }
}
