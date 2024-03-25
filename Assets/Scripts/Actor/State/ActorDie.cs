public class ActorDie : ActorBaseState
{
    public ActorDie(Actor actor) : base(actor) { }
    public override void EnterState()
    {
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsDead, true);
    }

    public override void UpdateState() 
    {
        base.UpdateState();
        //if(Actor.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    Managers.Instance.GameOverAction?.Invoke();
        //}
    }

    public override void FixedUpdateState() { }

    public override void ExitState() 
    {

        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsDead, Actor.IsDead);
    }

    protected override void ChangeFromState()
    {
        if (Actor.CanSwitch) Actor.StateMachine.SetState(ActorState.Switch);
    }
}
