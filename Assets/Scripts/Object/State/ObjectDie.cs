using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDie : ObjectBaseState
{
    public ObjectDie(ObjectStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {

    }

    protected override void ChangeFromState()
    {
        if (!StateMachine.Owner.IsDead)
        {
            StateMachine.SetState("Idle");
        }
    }
}
