using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class PeonManager : MonoBehaviour
{
    #region Variables
    
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
        set { m_activePeon = value; }
    }

    
    #endregion

    private void Awake()
    {
        GameManager.GetManager()._peonManager = this;
    }

}
