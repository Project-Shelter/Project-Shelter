using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DayNight))]
public class DayNightEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DayNight dayNight = (DayNight)target;
        if (GUILayout.Button("Skip Timer")) 
        {
            dayNight.PassTime(); 
        }
    }
}
