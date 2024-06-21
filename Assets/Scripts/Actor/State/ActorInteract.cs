using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorInteract : ActorBaseState
{
    public ActorInteract(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        Actor.MoveBody.Stop();
        Actor.interactable.Interact(Actor);
        Managers.Scene.GetCurrentScene<GameScene>().PlayerCamera.SetZoom(true);
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.BeforeSwitchActorAction +=
            () => {
                if(Actor == Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor)
                    Managers.Scene.GetCurrentScene<GameScene>().PlayerCamera.SetZoom(false);
            };
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.SwitchActorAction +=
            () => {
                if (Actor == Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor)
                    Managers.Scene.GetCurrentScene<GameScene>().PlayerCamera.SetZoom(true);
            };
    }

    public override void UpdateState() { base.UpdateState(); }

    public override void FixedUpdateState() { }

    public override void UpdateWithNoCtrl()
    {
        Actor.ActionRadius.AlertForMonstersInRadius();
        if (Actor.interactable == null) Actor.StateMachine.SetState(ActorState.Idle);
        if (Actor.IsDead) Actor.StateMachine.SetState(ActorState.Die);
    }

    public override void ExitState()
    {
        if (Actor.interactable != null) { Actor.interactable.StopInteract(); }
        Managers.Scene.GetCurrentScene<GameScene>().PlayerCamera.SetZoom(false);
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.BeforeSwitchActorAction -=
            () => {
                if (Actor == Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor)
                    Managers.Scene.GetCurrentScene<GameScene>().PlayerCamera.SetZoom(false);
            };
        Managers.Scene.GetCurrentScene<GameScene>().ActorController.SwitchActorAction -=
            () => {
                if (Actor == Managers.Scene.GetCurrentScene<GameScene>().ActorController.CurrentActor)
                    Managers.Scene.GetCurrentScene<GameScene>().PlayerCamera.SetZoom(true);
            };
    }

    protected override void ChangeFromState()
    {
        if (InputHandler.ButtonE) Actor.StateMachine.SetState(ActorState.Idle);
    }
}
