using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseDetach : Phase
{
    [Header("Detach Phase Parameters")]
    [SerializeField]
    [Range(0,100)]
    private int _luck;

    private void Start()
    {
        type = PhaseType.DETACH;
    }
    public override void LaunchPhase()
    {
        controlDuration = false;
        int rand = Random.Range(0, 101);
        Debug.Log("Random = " + rand);
        if(rand <= _luck)
        {
            TrainManager.Instance.UnclipCarriage(TrainManager.Instance._carriages.Count - 2);
            SoundManager.Instance.Play("attack");
        }
        PhaseManager.Instance.NextPhase();
    }
    public override string BuildGameObjectName()
    {
        return "Detach Last Wagon";
    }
}
