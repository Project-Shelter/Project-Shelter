using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorDown : ActorBaseState
{
    public ActorDown(Actor actor) : base(actor) { }
    public override void EnterState()
    {
        Actor.Anim.Play("Player Crouch");
        ActorController.Instance.Stop();
    }

    public override void UpdateState()
    {
        ActorController.Instance.Turn();
    }

    public override void FixedUpdateState() { }

    public override void ExitState() { }

}
