using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Actor의 행동정의 최상위 클래스
public abstract class ActorBaseState
{
    protected Actor Actor { get; private set; }
    protected ActorBaseState(Actor actor)
    {
        Actor = actor;
    }
    
    //Start, Update, FixedUpdate, Disable
    public abstract void EnterState();
    public virtual void UpdateState() { ChangeFromState(); }
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    protected abstract void ChangeFromState();

}
