public abstract class ActorBaseState
{
    protected Actor Actor { get; private set; }
    protected ActorBaseState(Actor actor)
    {
        Actor = actor;
    }
    
    public abstract void EnterState();
    public virtual void UpdateState() { if(!Actor.IsSwitching) ChangeFromState(); }
    public abstract void FixedUpdateState();
    public abstract void UpdateWithNoCtrl();
    public abstract void ExitState();
    protected abstract void ChangeFromState();
}
