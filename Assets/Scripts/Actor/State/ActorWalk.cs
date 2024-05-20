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
        Actor.MoveBody.Turn();
        Actor.ActionRadius.AlertForMonstersInRadius();
    }

    public override void FixedUpdateState()
    {
        Actor.MoveBody.Move();
    }

    public override void ExitState() 
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsMoving, false);
    }

    protected override void ChangeFromState()
    {
        if (Actor.CanSwitch) Actor.StateMachine.SetState(ActorState.Switch);
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
        if (Actor.CanConceal) Actor.StateMachine.SetState(ActorState.Conceal);
        if (Actor.MoveBody.CanDash) Actor.StateMachine.SetState(ActorState.Dash);
        if (!Actor.MoveBody.CanMove) Actor.StateMachine.SetState(ActorState.Idle);
    }
}
