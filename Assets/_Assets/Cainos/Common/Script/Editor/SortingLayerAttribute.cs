using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cainos
{
    public class SortingLayerAttribute : PropertyAttribute
    {
        [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
        public class SortingLayerDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                string[] sortingLayerNames = new string[SortingLayer.layers.Length];
                for (int a = 0; a < SortingLayer.layers.Length; a++)
                    sortingLayerNames[a] = SortingLayer.layers[a].name;
                if (property.propertyType != SerializedPropertyType.String)
                {
                    EditorGUI.HelpBox(position, property.name + "{0} is not an string but has [SortingLayer].", MessageType.Error);
                }
                else if (sortingLayerNames.Length == 0)
                {
                    EditorGUI.HelpBox(position, "There is no Sorting Layers.", MessageType.Error);
                }
                else if (sortingLayerNames != null)
                {
                    EditorGUI.BeginProperty(position, label, property);

                    string oldName = property.stringValue;

                    int oldLayerIndex = -1;
                    for (int a = 0; a < sortingLayerNames.Length; a++)
                        if (sortingLayerNames[a].Equals(oldName)) oldLayerIndex = a;

                    int newLayerIndex = EditorGUI.Popup(position, label.text, oldLayerIndex, sortingLayerNames);

                    if (newLayerIndex != oldLayerIndex)
                    {
                        property.stringValue = sortingLayerNames[newLayerIndex];
                    }

                    EditorGUI.EndProperty();
                }
            }
        }
    }
}