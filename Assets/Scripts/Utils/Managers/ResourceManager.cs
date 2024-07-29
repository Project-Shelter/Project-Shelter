using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        T[] arr = Resources.LoadAll<T>(path);
        if(arr == null || arr.Length == 0)
        {
            return null;
        }

        return arr;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject obj = GameObject.Instantiate(original, parent);
        obj.name = original.name;

        return obj;
    }

    public void Destroy(GameObject _go, float _time = 0.0f) {
        if (_go == null)
        {
            Debug.Log($"Object you want to Destroy is Null : {_go}");
            return;
        }
        GameObject.Destroy(_go, _time);
    }
}
