using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackDisarm : ActorAttackState
{
    public ActorAttackDisarm(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
    }

    protected override void ChangeFromState()
    {
        if (Actor.Weapon != null)
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
            return;
        }
    }
}