using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateMachine : StateMachine<ObjectStateMachine>
{
    public BreakableObject Owner { get; private set; }

    public ObjectStateMachine(BreakableObject owner)
    {
        Owner = owner;
        InitState();
    }

    protected virtual void InitState()
    {
        AddState("Idle", new ObjectIdle(this));
        AddState("Attack", new ObjectAttack(this));
        AddState("Die", new ObjectDie(this));
    }
}
