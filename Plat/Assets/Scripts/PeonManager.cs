using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class PeonManager : MonoBehaviour
{
    #region Variables
    
    [SerializeField]
    private Material _base;
    [SerializeField]
    private Material _outline;

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
                GameManager.GetManager()._UIManager.UpdateUIPeon(null);
            }
            if(value != null)
            {
                value.SwitchMaterial(_outline);
                GameManager.GetManager()._UIManager.UpdateUIPeon(value._peonInfo);
            }
            m_activePeon = value;
        }
    }

    
    #endregion

    private void Awake()
    {
        GameManager.GetManager()._peonManager = this;
    }

    public void AddPeon(Peon peonToAdd)
    {
        _peons.Add(peonToAdd);
        GameManager.GetManager()._UIManager.AddUIPeon(peonToAdd);
    }
}
