using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public abstract class Phase : MonoBehaviour
{
    public enum PhaseMode
    {
        MAIN,
        SUB,
        CONDITION,
        GROUP
    }
    [Header("Common Phase Parameters")]
    public PhaseMode mode;

    public enum PhaseType
    {
        ATTACK,
        BLANK,
        BREAK,
        CAMERA,
        RESETCAMERA,
        SOUND,
        TEXT,
        DETACH
    }

    protected PhaseType type;
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

    [HideInInspector]
    public List<Phase> subPhases;

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

    private void Awake()
    {
        subPhases = HierarchyUtils.GetComponentInDirectChildren<Phase>(this.transform);
    }

    public PhaseType GetPhaseType()
    {
        return type;
    }
}
