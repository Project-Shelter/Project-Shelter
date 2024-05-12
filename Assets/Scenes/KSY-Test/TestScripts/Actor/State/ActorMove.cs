using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class ActorMove : ActorBaseState
{
    public ActorMove(Actor actor) : base(actor) { }
    public override void EnterState()
    {
        Actor.Anim.Play("Player Walk");
    }

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        ActorController.Instance.Move();
        if(InputHandler.ButtonW) ActorController.Instance.Jump();
    }

    public override void ExitState() { }
}
