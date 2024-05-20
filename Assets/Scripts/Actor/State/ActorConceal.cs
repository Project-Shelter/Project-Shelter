using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorConceal : ActorBaseState
{
    public ActorConceal(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsConcealing, true);
        Actor.Tr.position = Actor.concealment.Tr.position;
        Actor.MoveBody.Stop();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Actor.ActionRadius.AlertConcealmentInRadius();
    }

    public override void FixedUpdateState()
    {
    }

    public override void ExitState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsConcealing, false);
    }

    protected override void ChangeFromState()
    {
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
        if (Actor.CanSwitch) Actor.StateMachine.SetState(ActorState.Switch);
        if (InputHandler.ButtonE || !Actor.concealment) Actor.StateMachine.SetState(ActorState.Idle);
    }
}
