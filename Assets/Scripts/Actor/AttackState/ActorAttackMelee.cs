using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackMelee : ActorAttackState
{
    public ActorAttackMelee(Actor actor) : base(actor) { }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
    }

    protected override void ChangeFromState()
    {
        if (!Actor.CanAttack)
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
        }
    }
}
