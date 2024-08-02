using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBaseState : BaseState<ObjectStateMachine>
{
    protected ObjectBaseState(ObjectStateMachine stateMachine) : base(stateMachine) { }
}
