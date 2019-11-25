using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PopupAtribute))]
public class Popup : PropertyDrawer
{
    int index;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        PopupAtribute att = (PopupAtribute)attribute;
        SerializedProperty liste = property.serializedObject.FindProperty(att.Liste);
        string[] myListe = new string[liste.arraySize];
        for (int i = 0; i < liste.arraySize; i++)
        {
            myListe[i] = liste.GetArrayElementAtIndex(i).stringValue;
        }
        index = EditorGUI.Popup(position, label.text, index, myListe);
        //property.stringValue = index.ToString();
    }

}
