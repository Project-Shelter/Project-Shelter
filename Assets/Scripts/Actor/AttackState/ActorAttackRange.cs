using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttackRange : ActorAttackState
{
    bool isAttacking;
    public ActorAttackRange(Actor actor) : base(actor)
    {
    }

    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, true);
        isAttacking = true;
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
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsAttacking, false);
    }

    protected override void ChangeFromState()
    {
        if (!Actor.CanAttack && !isAttacking)
        {
            Actor.AttackStateMachine.SetState(AttackState.Idle);
        }
    }
}
