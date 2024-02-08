using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoSingleton<Managers>
{
    private ResourceManager _resources = new ResourceManager();
    private UIManager _ui = new UIManager();

    public Action GameOverAction = null;

    public static ResourceManager Resources { get { return Instance._resources; } } 
    public static UIManager UI { get { return Instance._ui; } }
}
