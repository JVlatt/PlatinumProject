using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSound : Phase
{
    [Header("Sound Phase Parameters")]
    [SerializeField]
    private string _sound;

    public override void LaunchPhase()
    {
        SoundManager.Instance.Play(_sound);
    }
    public override string BuildGameObjectName()
    {
        return "Sound (" + _sound + ")";
    }

    private void Start()
    {
        type = PhaseType.SOUND;
    }
}
