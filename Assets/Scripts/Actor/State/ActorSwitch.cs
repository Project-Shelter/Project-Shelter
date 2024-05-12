public class ActorSwitch : ActorBaseState
{
    public ActorSwitch(Actor actor) : base(actor) { }
    
    public override void EnterState()
    {
        Actor.MoveBody.Stop();
        Actor.ActorSwitchEffect.Play();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!Actor.ActorSwitchEffect.IsAlive(true))
        {
            ActorController.Instance.SwitchActor();
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState() 
    {
        Actor.ActorSwitchEffect.Stop();
    }

    protected override void ChangeFromState()
    {

    }
}
