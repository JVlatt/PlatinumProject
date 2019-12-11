using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCutSon : Phase
{
    [SerializeField]
    string soundName;

    public override string BuildGameObjectName()
    {
        return "Cut son (" + soundName + ")";
    }

    public override void LaunchPhase()
    {
            SoundManager.Instance.StopSound(soundName);
    }

    // Start is called before the first frame update
    void Start()
    {
        controlDuration = true;
    }

}
