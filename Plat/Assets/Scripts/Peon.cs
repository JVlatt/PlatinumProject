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

    [Header("Personnalité")]
    private int m_ID;
    public int _ID { 
        get { return m_ID; } 
    }
    static int m_nextID;

    [SerializeField]
    private TYPE m_type;
    public TYPE _type
    {
        get { return m_type; }
    }

    private HEALTHSTATE m_HEALTHSTATE;
    public HEALTHSTATE _HEALTHSTATE
    {
        get { return m_HEALTHSTATE; }
        set { m_HEALTHSTATE = value; }
    }

    [Header("PV")]
    private float m_maxPV;
    [SerializeField]
    private float m_PV;




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
        SIMPLE
    }
    #endregion
    #endregion

    void Start()
    {
        m_ID = m_nextID;
        _mentalHealth = 100;
        GameManager.GetManager()._peonManager._peons.Add(this);        
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
    }

    public void TreatPeon() //CEDRIC
    {
        _HEALTHSTATE = HEALTHSTATE.TREAT;
        //lancer le timer de traitement;
        //mettre a jours la vie 
    }

    public float PVLost() //CEDRIC
    {
        return m_maxPV - m_PV;
    }
}
