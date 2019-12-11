using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseLightOff : Phase
{
    public int carriage;
    public override void LaunchPhase()
    {
        controlDuration = true;
        Carriage c = TrainManager.Instance._carriages.Find(x => x.id == carriage);
        if (c != null)
        {
            if(PhaseManager.Instance.eventPeon == null)
            {
                PhaseManager.Instance.eventPeon = "Taon";
            }
            c.isAnEvent = true;
            c.SwitchLights(false, true);
            SoundManager.Instance.Play("lightBreak");
            PeonManager.Instance._activePeon = null;
        }
    }
    public override string BuildGameObjectName()
    {
        return "LightOff(" + carriage + ")";
    }
    private void Start()
    {
        type = PhaseType.LIGHTOFF;
    }
}
