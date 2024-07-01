public abstract class ActorAttackState
{
    protected Actor Actor { get; private set; }
    protected ActorAttackState(Actor actor)
    {
        Actor = actor;
    }

    public abstract void EnterState();
    public virtual void UpdateState() { ChangeFromState(); }
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    protected abstract void ChangeFromState();
}
