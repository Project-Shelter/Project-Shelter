using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorIdle : ActorBaseState
{
    public ActorIdle(Actor actor) : base(actor) { }
    
    public override void EnterState()
    {
        ActorController.Instance.Stop();
        Actor.Anim.Play("Player Idle");
    }

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        if(InputHandler.ButtonW) ActorController.Instance.Jump();
    }

    public override void ExitState() { }
}
