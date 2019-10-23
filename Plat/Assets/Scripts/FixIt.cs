using Assets.Script;
using UnityEngine;

public class FixIt : MonoBehaviour
{
    Carriage _carriage;
    bool m_isOnFix;
    public bool _isOnFix
    {
        get { return m_isOnFix; }
        set { 
            m_isOnFix = value;
            if (!value)
                _activePeon._isFixing = false;
        }
    }
    public Peon _activePeon { get; private set; }

    public void Setup(Carriage carriage)
    {
        _carriage = carriage;
    }

    private void OnMouseDown()
    {
        if (_isOnFix || !GameManager.GetManager()._peonManager._activePeon) return;
        if (!GameManager.GetManager()._peonManager._activePeon.CanFix(_carriage)) return;
        if (_carriage._underAttack || _carriage._willBeAttacked) return;
        _activePeon = GameManager.GetManager()._peonManager._activePeon;
        if (_activePeon._currentCarriage != _carriage)
        {
            GameManager.GetManager()._trainManager.MovePeonToCarriage(_activePeon, _carriage);
        }
        else
        {
            GameManager.GetManager()._peonManager._activePeon = null;
        }
        _isOnFix = true;
        
        _activePeon._isFixing = true;
    }

    private void OnMouseEnter()
    {
        GameManager.GetManager()._UIManager.ChangeCursor("fix");
    }
    private void OnMouseExit()
    {
        GameManager.GetManager()._UIManager.ChangeCursor("default");
    }

}
