using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MaskHideAtt))]
public class MaskHide : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MaskHideAtt att = (MaskHideAtt)attribute;



        bool isEnabled = getEnabledBool(att, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = isEnabled;

        if (isEnabled)
        {
            if (att.IsHeader)
            {
                switch (att.MaskIndex)
                {
                    case 0:
                        
                        EditorGUI.LabelField(position, "Touche",EditorStyles.boldLabel);
                        break;
                    case 1:
                        EditorGUI.LabelField(position, "Slide", EditorStyles.boldLabel);
                        break;
                    case 2:
                        EditorGUI.LabelField(position, "Pinche", EditorStyles.boldLabel);
                        break;
                    case 3:
                        EditorGUI.LabelField(position, "Drag", EditorStyles.boldLabel);
                        break;
                    default:
                        break;
                }
                position.y += EditorGUI.GetPropertyHeight(property, label);
            }
            EditorGUI.PropertyField(position, property, label, true);
        }


    }

    private bool getEnabledBool(MaskHideAtt att, SerializedProperty property)
    {
        bool isActive = false;
        SerializedProperty maskProperty = property.serializedObject.FindProperty(att.MaskField);
        bool b;
        if (att.BoolField == "")
            b = true;
        else
        {
            SerializedProperty boolField = property.serializedObject.FindProperty(att.BoolField);
            b = boolField.boolValue;
        }
        if (att.InvertBoolField)
            b = !b;
        if ((maskProperty.intValue & 1 << att.MaskIndex) != 0 && b)
            isActive = true;
        return isActive;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        MaskHideAtt att = (MaskHideAtt)attribute;
        bool enabled = getEnabledBool(att, property);

        if (enabled)
        {
            if(att.IsHeader)
                return EditorGUI.GetPropertyHeight(property, label)*2;
            else
                return EditorGUI.GetPropertyHeight(property, label);
        }
        else
            return -EditorGUIUtility.standardVerticalSpacing;
    }

}
