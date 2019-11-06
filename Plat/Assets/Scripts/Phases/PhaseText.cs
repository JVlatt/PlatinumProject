using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseText : Phase
{
    [SerializeField]
    private bool _autoName;

    [SerializeField]
    private string _text;
    [SerializeField]
    private string _character;

    public override void LaunchPhase()
    {
        controlDuration = false;
        UIManager.Instance.autoName = _autoName;
        UIManager.Instance.DisplayText(_text, _character, duration);
    }
    public override string BuildGameObjectName()
    {
        return "Text (" + _character + ")";
    }

    public override bool KillTextBox()
    {
        return false;
    }
}
