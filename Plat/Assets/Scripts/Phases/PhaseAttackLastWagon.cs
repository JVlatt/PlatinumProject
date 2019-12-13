using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseAttackLastWagon : Phase
{
    [SerializeField]
    private float _timeBeforeAttack;
    [SerializeField]
    private float _timeBeforeNextEvent;
    private bool _launchTimer = false;
    private float _timer = 0f;

    private void Start()
    {
        type = PhaseType.ATTACKLAST;
    }

    public override void LaunchPhase()
    {
        _timer = 0f;
        controlDuration = false;
        if(TrainManager.Instance._carriages.Count <= 3)
        {
            PhaseManager.Instance.NextPhase();
        }
        else
        {
            Carriage c = TrainManager.Instance._carriages[TrainManager.Instance._carriages.Count - 1];
            if (c != null)
            {
                c.autoLoose = true;
                c.Attack(duration, _timeBeforeAttack);
                SoundManager.Instance.Play("attack");
                PeonManager.Instance._activePeon = null;
                _launchTimer = true;
            }
        }
    }


    private void Update()
    {
        if(_launchTimer)
        {
            _timer += Time.deltaTime;
            if (_timer > _timeBeforeNextEvent)
            {
                PhaseManager.Instance.NextPhase();
                _launchTimer = false;
            }
        }
        else
        {
            _timer = 0f;
        }
    }


    public override string BuildGameObjectName()
    {
        return "Attack (Last Wagon)";
    }
}
