using UnityEngine;

public class ActorDash : ActorBaseState
{
    private float dashTime;
    private float escapeTime;

    public ActorDash(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        Actor.WeaponSocket.gameObject.SetActive(false);
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsRunning, true);
        InitDashTime();
    }

    private void InitDashTime()
    {
        dashTime = 0f;
        escapeTime = Actor.Stat.dashTime.GetValue();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Actor.ActionRadius.AlertForMonstersInRadius();
    }

    public override void FixedUpdateState()
    {
        dashTime += Time.deltaTime;
        Actor.MoveBody.Dash();
    }

    public override void UpdateWithNoCtrl() 
    {
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
    }

    public override void ExitState()
    {
        Actor.WeaponSocket.gameObject.SetActive(true);
        Actor.Anim.SetAnimParamter(ActorAnimParameter.IsRunning, false);
        Actor.MoveBody.DashOnCool();
    }

    protected override void ChangeFromState()
    {
        if (dashTime >= escapeTime) Actor.StateMachine.SetState(ActorState.Idle);
    }
}
