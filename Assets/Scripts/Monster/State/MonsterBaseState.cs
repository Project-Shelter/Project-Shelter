using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState : BaseState<MonsterStateMachine>
{
    protected MonsterBaseState(MonsterStateMachine stateMachine) : base(stateMachine) { }
    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateFixedUpdate();
    public abstract void OnStateExit();
}
