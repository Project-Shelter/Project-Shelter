using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorDie : ActorBaseState
{
    public ActorDie(Actor actor) : base(actor) { }
    public override void EnterState()
    {
        Actor.Anim.speed = 1;
        Actor.Anim.Play("Player Die");
    }

    public override void UpdateState() 
    {
        if(Actor.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Managers.Instance.GameOverAction?.Invoke();
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState() { }

}
