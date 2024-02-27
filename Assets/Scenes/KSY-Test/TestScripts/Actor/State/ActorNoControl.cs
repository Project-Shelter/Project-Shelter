using UnityEngine;

public class ActorNoControl : ActorBaseState
{
    public ActorNoControl(Actor actor) : base(actor) { }
    
    public override void EnterState()
    {
        ActorController.Instance.Stop();
        Actor.Stat.ActorChangeEffect.Play();

        if (Actor.StateMachine.StateBeforeSwitch == ActorState.OnLadder)
        {
            ActorController.Instance.NoGravity();
            Actor.Anim.speed = 0;
        }
        else if (!Actor.IsDead)
        {
            Actor.Anim.Play("Player Idle");
        }
    }

    public override void UpdateState()
    {
        if (!Actor.Stat.ActorChangeEffect.IsAlive(true))
        {
             ActorController.Instance.SwitchActor();
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState() { }
}
