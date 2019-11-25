using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class MaskAtt : PropertyAttribute
{

    public string[] fieldList = { "Touche", "Slide", "Pinche", "Drag&Drop" };

    public MaskAtt() { }

}
