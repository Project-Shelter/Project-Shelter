using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> where T : StateMachine<T>
{
    protected T StateMachine { get; private set; }

    protected BaseState(T stateMachine)
    {
        StateMachine = stateMachine;
    }

    //Start, Update, FixedUpdate, Disable
    public abstract void EnterState();
    public virtual void UpdateState() { ChangeFromState(); }
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    protected abstract void ChangeFromState();
}
