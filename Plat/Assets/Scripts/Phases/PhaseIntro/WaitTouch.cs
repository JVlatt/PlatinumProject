using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitTouch : Phase
{

    [SerializeField]
    Image image;

    public override string BuildGameObjectName()
    {
        return "Wait for Touch";
    }

    public override void LaunchPhase()
    {
        image.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        controlDuration = false;
        type = PhaseType.WAITTOUCH;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
