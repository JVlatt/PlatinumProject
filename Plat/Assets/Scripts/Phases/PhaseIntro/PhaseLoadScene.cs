using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseLoadScene : Phase
{

    public override string BuildGameObjectName()
    {
        return "LoadScene";
    }

    public override void LaunchPhase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        controlDuration = true;
    }

}
