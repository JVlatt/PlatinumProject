using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseActiveAnim : Phase
{

    [SerializeField]
    Animator animator;
    [SerializeField]
    string fieldName;


    public override string BuildGameObjectName()
    {
        return "Active anim " + animator.name+" (" +fieldName+")"; 
    }

    public override void LaunchPhase()
    {
        animator.SetTrigger(fieldName);
    }

}
