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
        if (_isOnFix || !PeonManager.Instance._activePeon) return;
        if (!PeonManager.Instance._activePeon.CanFix(_carriage)) return;
        if (_carriage._underAttack || _carriage._willBeAttacked) return;
        if (PhaseManager.Instance && PhaseManager.Instance.activePhase.freezeControl) return;
        _activePeon = PeonManager.Instance._activePeon;
        TrainManager.Instance.MovePeonToCarriage(_activePeon, _carriage, transform.position);
        _isOnFix = true;

        _activePeon._isFixing = true;

        // JSP
        if (_isAnEvent)
        {
            PhaseManager.Instance.GetPeon(_activePeon);
            PhaseManager.Instance.NextPhase();
            _isAnEvent = false;
        }
    }

    private void OnMouseEnter()
    {
        if (PeonManager.Instance._activePeon != null && !PhaseManager.Instance.activePhase.freezeControl && !_carriage._underAttack)
            UIManager.Instance.ChangeCursor("fix");
    }
    private void OnMouseExit()
    {
        UIManager.Instance.ChangeCursor("default");
    }

}
