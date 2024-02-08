using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState
{
    protected MonsterStateManager Manager { get; private set; }
    protected MonsterBaseState(in MonsterStateManager manager)
    {
        Manager = manager;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateFixedUpdate();
    public abstract void OnStateExit();
}
