using UnityEngine;

public class ActorNoControl : ActorBaseState
{
    public ActorNoControl(Actor actor) : base(actor) { }
    
    public override void EnterState()
    {
        Actor.Rigid.velocity = Vector2.zero;
        Actor.Stat.ActorChangeEffect.Play();

        if (Actor.OnLadder)
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