using UnityEngine;

public class ActorWalk : ActorBaseState
{
    public ActorWalk(Actor actor) : base(actor) {}

    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsMoving, true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!Actor.IsAttacking)
        {
            Actor.MoveBody.Turn();
        }
        
    }

    public override void FixedUpdateState()
    {
        Actor.MoveBody.Move();
    }

    public override void UpdateWithNoCtrl()
    {
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
        Actor.ActionRadius.AlertForMonstersInRadius();
    }

    public override void ExitState() 
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsMoving, false);
    }

    protected override void ChangeFromState()
    {
        if (Actor.CanInteract) Actor.StateMachine.SetState(ActorState.Interact);
        if (Actor.MoveBody.CanDash) Actor.StateMachine.SetState(ActorState.Dash);
        if (!Actor.MoveBody.CanMove) Actor.StateMachine.SetState(ActorState.Idle);
    }
}
