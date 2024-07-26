using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorInteract : ActorBaseState
{
    private PlayerCamera camera;
    private Action ZoomTrueAction;
    private Action ZoomFalseAction;

    private bool canInteract;
    public ActorInteract(Actor actor) : base(actor) 
    {
        camera = ServiceLocator.GetService<PlayerCamera>();
        ZoomTrueAction = () => { if (Actor == Actor.Controller.CurrentActor) { camera.SetZoom(true); } };
        ZoomFalseAction = () => { if (Actor == Actor.Controller.CurrentActor) { camera.SetZoom(false); } };
    }

    public override void EnterState()
    {
        Actor.MoveBody.Stop();
        Actor.Interactable.StartInteract(Actor);
        canInteract = true;
        camera.SetZoom(true);
        Actor.Controller.BeforeSwitchActorAction += ZoomFalseAction;
        Actor.Controller.SwitchActorAction += ZoomTrueAction;
    }

    public override void UpdateState() 
    { 
        base.UpdateState();
        Actor.Interactable.Interacting();
        canInteract = Actor.Interactable.CanKeepInteracting();
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
        Actor.Controller.BeforeSwitchActorAction -= ZoomFalseAction;
        Actor.Controller.SwitchActorAction -= ZoomTrueAction;
    }

    protected override void ChangeFromState()
    {
        if (Actor.IsDead)
        {
            Actor.StateMachine.SetState(ActorState.Die);
            return;
        }
        if (Actor.Interactable == null || !canInteract) 
        {
            Actor.StateMachine.SetState(ActorState.Idle);
            return;
        }
    }
}
