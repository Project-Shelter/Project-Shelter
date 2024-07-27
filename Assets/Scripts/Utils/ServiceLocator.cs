using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new();

    // 서비스의 등록과 해제는 BaseScene 파생 클래스에서만 할 수 있도록 제한
    public static void RegisterService<T>(T service)
    {
        services[typeof(T)] = service;
        Debug.Log($"RegisterService: {typeof(T)}");
    }

    public static void UnregisterService<T>()
    {
        services.Remove(typeof(T));
    }

    public static T GetService<T>()
    {
        return (T)services[typeof(T)];
    }
}