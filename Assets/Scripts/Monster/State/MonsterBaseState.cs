using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState : BaseState<MonsterStateMachine>
{
    protected MonsterBaseState(MonsterStateMachine stateMachine) : base(stateMachine) { }
}
