using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorInteract : ActorBaseState
{
    public ActorInteract(Actor actor) : base(actor) { }

    private PlayerCamera camera;

    public override void EnterState()
    {
        Actor.MoveBody.Stop();
        Actor.Interactable.Interact(Actor);
        camera = ServiceLocator.GetService<PlayerCamera>();
        camera.SetZoom(true);
        Actor.Controller.BeforeSwitchActorAction +=
            () => {
                if (Actor == Actor.Controller.CurrentActor) { camera.SetZoom(false); }
            };
        Actor.Controller.SwitchActorAction +=
            () => {
                if (Actor == Actor.Controller.CurrentActor) { camera.SetZoom(true); }
            };
    }

    public override void UpdateState() 
    { 
        base.UpdateState();
        Actor.Aim();
    }

    public override void FixedUpdateState() { }

    public override void UpdateWithNoCtrl()
    {
        Actor.ActionRadius.AlertForMonstersInRadius();
        if (Actor.Interactable == null) Actor.StateMachine.SetState(ActorState.Idle);
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
    }

    public override void ExitState()
    {
        if (Actor.Interactable != null) { Actor.Interactable.StopInteract(); }
        camera.SetZoom(false);
        Actor.Controller.BeforeSwitchActorAction -=
            () => {
                if (Actor == Actor.Controller.CurrentActor) { camera.SetZoom(false); }
            };
        Actor.Controller.SwitchActorAction -=
            () => {
                if (Actor == Actor.Controller.CurrentActor) { camera.SetZoom(true); }
            };
    }

    protected override void ChangeFromState()
    {
        if (InputHandler.ButtonE) Actor.StateMachine.SetState(ActorState.Idle);
    }
}
