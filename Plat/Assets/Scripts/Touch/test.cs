using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class test : MonoBehaviour
{
    [MaskAtt()]
    public int mask;

    [MaskHideAtt("mask",0,"",false,true)]
    public string touchField;

    [MaskHideAtt("mask", 1,"",false,true)]
    public string slideField;

    [MaskHideAtt("mask", 2, "",false, true)]
    public string pinchField;

    [MaskHideAtt("mask", 3, "",false, true)]
    public string dragField;

    public bool bool1;
    public bool bool2;

    [MaskHideAtt("mask", 0, "",false, false)]
    [ConditionalHide("bool2")]
    public bool bool3;


    public enum type{
        bill,
        bob
    }

    [Serializable]
    public class toto
    {
        public List<bool> tete;
    }


}
