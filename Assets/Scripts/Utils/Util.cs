using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : UnityEngine.Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
        {
            component = obj.AddComponent<T>();
            Debug.Log($"@Warning@ Component is missing(So I Add It in Runtime) > AddComponent in {obj}");
        }
        return component;
    }
    
    public static GameObject FindChild(GameObject parent, string name = null, bool recursive = false)
    {
        Transform temp = FindChild<Transform>(parent, name, recursive);
        if (temp != null)
            return temp.gameObject;
        return null;
    }
    public static T FindChild<T>(GameObject parent, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (parent == null)
            return null;

        if(recursive == false)
        {
            for(int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in parent.GetComponentsInChildren<T>()) 
            { 
                if (string.IsNullOrEmpty(name) || component.name == name) 
                    return component; 
            }
        }

        return null;
    }
}
