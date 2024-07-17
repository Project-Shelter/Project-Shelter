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
        if (!Actor.IsAttacking)
        {
            Actor.MoveBody.Turn();
        }
    }

    public override void FixedUpdateState() { }

    public override void UpdateWithNoCtrl() 
    {
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
        Actor.ActionRadius.AlertForMonstersInRadius(); 
    }

    public override void ExitState() { }

    protected override void ChangeFromState()
    {
        if (Actor.CanInteract) Actor.StateMachine.SetState(ActorState.Interact);
        if (Actor.MoveBody.CanMove) Actor.StateMachine.SetState(ActorState.Walk);
        if (Actor.MoveBody.CanDash) Actor.StateMachine.SetState(ActorState.Dash);
    }
}
