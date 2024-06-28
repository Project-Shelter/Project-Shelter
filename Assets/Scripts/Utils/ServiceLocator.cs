using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new();

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