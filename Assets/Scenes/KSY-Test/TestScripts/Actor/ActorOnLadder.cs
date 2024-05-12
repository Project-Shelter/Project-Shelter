using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ActorOnLadder : ActorBaseState
{
    public ActorOnLadder(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        Actor.Anim.Play("Player OnLadder");
        Actor.Tr.position = new(Actor.CurrentLadder.transform.position.x, Actor.transform.position.y);
        ActorController.Instance.Stop();
        ActorController.Instance.NoGravity();
    }

    //ONGround 체크로 인해 버그 발생 (Idle <-> OnLadder)
    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        if (InputHandler.ButtonW) { ActorController.Instance.GoUp(); }
        if (InputHandler.ButtonS) { ActorController.Instance.GoDown(); }
        if (!InputHandler.ButtonW && !InputHandler.ButtonS) { Actor.Anim.speed = 0; }
        else { Actor.Anim.speed = 1; }
    }

    public override void ExitState()
    {
        Actor.Anim.speed = 1;
        Actor.Rigid.velocity = Vector2.zero;
        ActorController.Instance.SetGravity();
    }
}
