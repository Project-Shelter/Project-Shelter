public class ActorIdle : ActorBaseState
{
    public ActorIdle(Actor actor) : base(actor) { }
    
    public override void EnterState()
    {
        Actor.MoveBody.Stop();
    }

    public override void UpdateState() 
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() { }

    protected override void ChangeFromState()
    {
        if (Actor.CanSwitch) Actor.StateMachine.SetState(ActorState.Switch);
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
        if (Actor.MoveBody.CanMove) Actor.StateMachine.SetState(ActorState.Walk);
        if (Actor.MoveBody.CanDash) Actor.StateMachine.SetState(ActorState.Dash);
    }
}
