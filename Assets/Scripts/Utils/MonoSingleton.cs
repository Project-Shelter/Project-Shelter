using System;
using UnityEngine;

//MonoBehaviour를 상속받는 유니티 싱글톤

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance { get { Init(); return _instance; } }

    private void Awake()
    {
        if(Init())
            Destroy(gameObject);
    }

    static bool Init()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();
            GameObject obj;
            
            if (_instance == null)
            {
                obj = new GameObject { name = typeof(T).ToString() + "(MonoSingleton)" };
                _instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
                return true;
            }

            obj = _instance.gameObject;
            DontDestroyOnLoad(obj);
            return false;
        }

        return true;
    }
}
