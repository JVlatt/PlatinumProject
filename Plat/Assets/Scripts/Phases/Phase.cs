using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public abstract class Phase : MonoBehaviour
{
    public enum PhaseType
    {
        MAIN,
        SUB,
        CONDITION,
        GROUP
    }

    public PhaseType type;

    [SerializeField]
    private float _duration;
    public float duration
    {
        get { return _duration; }
        set { _duration = value; }
    }
    
    private bool _controlDuration = false;
    public bool controlDuration
    {
        get { return _controlDuration; }
        set { _controlDuration = value; }
    }

    [SerializeField]
    private bool _freezeControl = false;
    public bool freezeControl
    {
        get { return _freezeControl; }
        set { _freezeControl = value; }
    }

    public List<Phase> subPhases;

    public bool conditionPhase = false;

    public abstract void LaunchPhase();

    public abstract string BuildGameObjectName();
    private void OnValidate()
    {
        gameObject.name = BuildGameObjectName();
    }

    public virtual bool KillTextBox()
    {
        return true;
    }

    private void Start()
    {
        subPhases = HierarchyUtils.GetComponentInDirectChildren<Phase>(this.transform);
        if (subPhases.Count == 2)
            conditionPhase = true;
    }
}
