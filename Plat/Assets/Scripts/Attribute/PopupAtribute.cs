using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class PopupAtribute : PropertyAttribute
{

    public string Liste;

    public PopupAtribute(string liste)
    {
        Liste = liste;
    }

}
