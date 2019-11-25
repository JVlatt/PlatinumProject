using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MaskAtt))]
public class Mask : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MaskAtt att = (MaskAtt)attribute;
        property.intValue = EditorGUI.MaskField(position,property.intValue,att.fieldList);
    }

}
