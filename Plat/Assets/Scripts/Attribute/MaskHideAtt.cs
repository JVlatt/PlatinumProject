using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class MaskHideAtt : PropertyAttribute
{

    public string MaskField;
    public int MaskIndex;
    public string BoolField;
    public bool IsHeader;
    public bool InvertBoolField;

    public MaskHideAtt(string maskField, int maskIndex, string boolField,bool invertBoolField, bool isHeader)
    {
        MaskField = maskField;
        MaskIndex = maskIndex;
        BoolField = boolField;
        IsHeader = isHeader;
        InvertBoolField = invertBoolField;
    }

}
