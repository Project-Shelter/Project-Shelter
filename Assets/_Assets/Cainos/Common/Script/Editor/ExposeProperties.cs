//https://wiki.unity3d.com/index.php/ExposePropertiesInInspector_SetOnlyWhenChanged

using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Cainos
{
    public static class ExposeProperties
    {
        public static void Expose(PropertyField[] properties)
        {
            var emptyOptions = new GUILayoutOption[0];
            EditorGUILayout.BeginVertical(emptyOptions);
            foreach (PropertyField p in properties)
            {
                Expose(p);
            }
            EditorGUILayout.EndVertical();
        }

        public static void Expose(PropertyField property, Type type = null)
        {
            var emptyOptions = new GUILayoutOption[0];
            EditorGUILayout.BeginHorizontal(emptyOptions);

            if (property.Type == SerializedPropertyType.Integer)
            {
                var oldValue = (int)property.GetValue();
                var newValue = EditorGUILayout.IntField(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }
            else if (property.Type == SerializedPropertyType.Float)
            {
                var oldValue = (float)property.GetValue();
                var newValue = EditorGUILayout.FloatField(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }
            else if (property.Type == SerializedPropertyType.Boolean)
            {
                var oldValue = (bool)property.GetValue();
                var newValue = EditorGUILayout.Toggle(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }
            else if (property.Type == SerializedPropertyType.String)
            {
                var oldValue = (string)property.GetValue();
                var newValue = EditorGUILayout.TextField(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }
            else if (property.Type == SerializedPropertyType.Vector2)
            {
                var oldValue = (Vector2)property.GetValue();
                var newValue = EditorGUILayout.Vector2Field(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }
            else if (property.Type == SerializedPropertyType.Vector3)
            {
                var oldValue = (Vector3)property.GetValue();
                var newValue = EditorGUILayout.Vector3Field(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }
            else if (property.Type == SerializedPropertyType.Enum)
            {
                var oldValue = (Enum)property.GetValue();
                var newValue = EditorGUILayout.EnumPopup(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }
            else if (property.Type == SerializedPropertyType.Color)
            {
                var oldValue = (Color)property.GetValue();
                var newValue = EditorGUILayout.ColorField(property.Name, oldValue, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }

            else if (property.Type == SerializedPropertyType.ObjectReference)
            {
                var oldValue = (UnityEngine.Object)property.GetValue();
                var newValue = EditorGUILayout.ObjectField(property.Name, oldValue, type, false, emptyOptions);
                if (oldValue != newValue)
                    property.SetValue(newValue);
            }

            EditorGUILayout.EndHorizontal();
        }

        public static PropertyField[] GetProperties(object obj)
        {
            var fields = new List<PropertyField>();

            PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo info in infos)
            {
                if (!(info.CanRead && info.CanWrite))
                    continue;

                object[] attributes = info.GetCustomAttributes(true);

                bool isExposed = false;
                foreach (object o in attributes)
                    if (o.GetType() == typeof(ExposePropertyAttribute))
                    {
                        isExposed = true;
                        break;
                    }
                if (!isExposed)
                    continue;

                var type = SerializedPropertyType.Integer;
                if (PropertyField.GetPropertyType(info, out type))
                {
                    var field = new PropertyField(obj, info, type);
                    fields.Add(field);
                }
            }

            return fields.ToArray();
        }

        public static PropertyField GetProperty(string name, object obj)
        {
            PropertyField field = null;

            PropertyInfo info = obj.GetType().GetProperty(name);

            if (!(info.CanRead && info.CanWrite))
                return null;

            object[] attributes = info.GetCustomAttributes(true);

            bool isExposed = false;
            foreach (object o in attributes)
                if (o.GetType() == typeof(ExposePropertyAttribute))
                {
                    isExposed = true;
                    break;
                }
            if (!isExposed)
                return null;

            var type = SerializedPropertyType.Integer;
            if (PropertyField.GetPropertyType(info, out type))
            {
                field = new PropertyField(obj, info, type);
            }

            return field;
        }
    }

    public class PropertyField
    {
        public object obj;

        PropertyInfo info;
        SerializedPropertyType type;

        MethodInfo getter;
        MethodInfo setter;

        public SerializedPropertyType Type
        {
            get { return type; }
        }

        public String Name
        {
            get { return ObjectNames.NicifyVariableName(info.Name); }
        }

        public PropertyField(object obj, PropertyInfo info, SerializedPropertyType type)
        {
            this.obj = obj;
            this.info = info;
            this.type = type;

            getter = this.info.GetGetMethod();
            setter = this.info.GetSetMethod();
        }

        public object GetValue() { return getter.Invoke(obj, null); }
        public void SetValue(object value) { setter.Invoke(obj, new[] { value }); }

        public static bool GetPropertyType(PropertyInfo info, out SerializedPropertyType propertyType)
        {
            Type type = info.PropertyType;
            propertyType = SerializedPropertyType.Generic;
            if (type == typeof(int))
                propertyType = SerializedPropertyType.Integer;
            else if (type == typeof(float))
                propertyType = SerializedPropertyType.Float;
            else if (type == typeof(bool))
                propertyType = SerializedPropertyType.Boolean;
            else if (type == typeof(string))
                propertyType = SerializedPropertyType.String;
            else if (type == typeof(Vector2))
                propertyType = SerializedPropertyType.Vector2;
            else if (type == typeof(Vector3))
                propertyType = SerializedPropertyType.Vector3;
            else if (type == typeof(Color))
                propertyType = SerializedPropertyType.Color;
            else if (type.IsEnum)
                propertyType = SerializedPropertyType.Enum;
            else if (type == typeof(Material))
                propertyType = SerializedPropertyType.ObjectReference;
            return propertyType != SerializedPropertyType.Generic;
        }
    }
}