using Assets.Script;
using UnityEngine;

public class FixIt : MonoBehaviour
{
    Carriage _carriage;
    bool m_isOnFix;
    public bool _isOnFix
    {
        get { return m_isOnFix; }
        set
        {
            m_isOnFix = value;
        }
    }
    public Peon _activePeon { get; private set; }

    public void Setup(Carriage carriage)
    {
        _carriage = carriage;
    }

    private bool _isAnEvent = false;
    public bool isAnEvent
    {
        get { return _isAnEvent; }
        set { _isAnEvent = value; }
    }

    private void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (_isOnFix || !GameManager.GetManager()._peonManager._activePeon) return;
        if (!GameManager.GetManager()._peonManager._activePeon.CanFix(_carriage)) return;
        if (_carriage._underAttack || _carriage._willBeAttacked) return;
        if (GameManager.GetManager().phaseManager && GameManager.GetManager().phaseManager.freezeControl) return;
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
        if (_isAnEvent)
        {
            GameManager.GetManager().phaseManager.GetPeon(_activePeon);
            GameManager.GetManager().phaseManager.NextPhase();
            _isAnEvent = false;
        }
    }

    private void OnMouseEnter()
    {
        if (GameManager.GetManager()._peonManager._activePeon != null && !GameManager.GetManager().phaseManager.freezeControl)
            GameManager.GetManager()._UIManager.ChangeCursor("fix");
    }
    private void OnMouseExit()
    {
        GameManager.GetManager()._UIManager.ChangeCursor("default");
    }

}
