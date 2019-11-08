﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meca : Peon
{
    [SerializeField]
    private float _unclipCD;
    private float _unclipTimer;
    private bool _isUncliping;
    private bool _feedbackIsActive;

    public bool IsUncliping
    {
        set
        {
            _isUncliping = value;
            _unclipTimer = 0;
        }
    }

    public override void SpecialUpdate()
    {
        if (_canMove || _isFixing) return;
        if(_isUncliping)
        {
            ActiveFeedback(true);
            _unclipTimer += Time.deltaTime;
            if(_unclipTimer>= _unclipCD)
            {
                ActiveFeedback(false);
                IsUncliping = false;
                _canMove = true;
                TrainManager.Instance.UnclipCarriage(_currentCarriage.id);
            }
        }
    }

    public void ActiveFeedback(bool active)
    {
        if (active == _feedbackIsActive) return;
        m_fix.SetActive(active);
        m_animator.SetBool("Healing", active);
        _feedbackIsActive = active;
    }

    public override void MovePeon(Carriage carriage)
    {
        base.MovePeon(carriage);
        IsUncliping = false;
    }
}
