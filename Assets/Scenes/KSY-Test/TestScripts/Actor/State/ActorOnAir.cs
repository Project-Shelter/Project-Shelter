using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public class ActorOnAir : ActorBaseState
{
    public ActorOnAir(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        Actor.Anim.Play("Player Jump");
    }

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        if (InputHandler.ButtonD || InputHandler.ButtonA)
        {
            ActorController.Instance.Move();
        }
    }
    public override void ExitState()
    {
        Actor.Rigid.velocity = Vector2.zero;
    }
}
