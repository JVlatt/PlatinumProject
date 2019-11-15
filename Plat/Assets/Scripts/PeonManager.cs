using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonManager : MonoBehaviour
{
    #region Singleton
    private static PeonManager _instance = null;
    public static PeonManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    #region Variables


    [SerializeField]
    private Material _base;
    [SerializeField]
    public Material _outline;

    private List<Peon> m_peons = new List<Peon>();
    public List<Peon> _peons
    {
        get { return m_peons; }
        set { m_peons = value; }
    }

    private Peon m_activePeon;
    public Peon _activePeon
    {
        get { return m_activePeon; }
        set
        {
            if(m_activePeon != null)
            {
                m_activePeon.SwitchMaterial(_base);
                UIManager.Instance.ActiveUIPerso(false, m_activePeon._ID);
            }
            if(value != null)
            {
                value.SwitchMaterial(_outline);
                UIManager.Instance.ActiveUIPerso(true,value._ID);
            }
            m_activePeon = value;
        }
    }

    
    #endregion
    
    public void AddPeon(Peon peonToAdd)
    {
        _peons.Add(peonToAdd);
        UIManager.Instance.AddUIPeon(peonToAdd);
    }
}
