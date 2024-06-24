using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoSingleton<Managers>
{
    private ResourceManager _resources = new ResourceManager();
    private UIManager _ui = new UIManager();
    private TableManager _data = new TableManager();
    private SceneManagerEX _sceneManager = new SceneManagerEX();

    public static ResourceManager Resources { get { return Instance._resources; } } 
    public static UIManager UI { get { return Instance._ui; } }
    public static TableManager Table { get { return Instance._data; } }
    public static SceneManagerEX SceneManager { get { return Instance._sceneManager; } }
    public static T GetCurrentScene<T>() where T : BaseScene
    {
        return Instance._sceneManager.CurrentScene as T;
    }
}
