using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Input = UnityEngine.Windows.Input;

public class ActorStateChanger
{
    public static void ChangeState(in Actor actor)
    {
        ChangeFromAny(actor);
        switch (actor.StateMachine.CurrentState)
        {
            case ActorState.Idle:
                ChangeFromIdle(actor);
                break;
            case ActorState.Move:
                ChangeFromMove(actor);
                break;
            case ActorState.OnAir:
                ChangeFromOnAir(actor);
                break;
            case ActorState.OnLadder:
                ChangeFromOnLadder(actor);
                break;
            case ActorState.Down:
                ChangeFromDown(actor);
                break;
            case ActorState.Attack:
                ChangeFromAttack(actor);
                break;
        }
    }

    public static void InitState(in Actor actor)
    {
        if (actor.IsDead) { actor.StateMachine.SetState(ActorState.Die); }
        else if (actor.OnLadder) { actor.StateMachine.SetState(ActorState.OnLadder); }
        else { actor.StateMachine.SetState(ActorState.Idle); }
    }

    private static void ChangeFromAny(in Actor actor)
    {
        if (InputHandler.ButtonCtrl)
        {
            actor.StateMachine.SetState(ActorState.NoControl);
        }
    }

    private static void ChangeFromIdle(in Actor actor)
    {
        if (actor.IsDead) actor.StateMachine.SetState(ActorState.Die);

        if (!actor.OnGround)
        {
            actor.StateMachine.SetState(ActorState.OnAir);
        }
        
        if (InputHandler.ButtonD || InputHandler.ButtonA) actor.StateMachine.SetState(ActorState.Move);
        if (InputHandler.ButtonS)
        {
            if(actor.CanGoDown)  actor.StateMachine.SetState(ActorState.OnLadder);
            else actor.StateMachine.SetState(ActorState.Down);
        }
        if (InputHandler.ButtonW && actor.CanGoUp) actor.StateMachine.SetState(ActorState.OnLadder);
        if (InputHandler.ClickLeft) actor.StateMachine.SetState(ActorState.Attack);
    }

    private static void ChangeFromMove(in Actor actor)
    {
        if (actor.IsDead) actor.StateMachine.SetState(ActorState.Die);

        if (!actor.OnGround)
        {
            actor.StateMachine.SetState(ActorState.OnAir);
        }
        
        if (InputHandler.ButtonS)
        {
            if(actor.CanGoDown)  actor.StateMachine.SetState(ActorState.OnLadder);
            else actor.StateMachine.SetState(ActorState.Down);
        }
        if (InputHandler.ButtonW && actor.CanGoUp) actor.StateMachine.SetState(ActorState.OnLadder);
        if (InputHandler.ClickLeft) actor.StateMachine.SetState(ActorState.Attack);
        if(!(InputHandler.ButtonD || InputHandler.ButtonA)) actor.StateMachine.SetState(ActorState.Idle);
    }

    private static void ChangeFromOnAir(in Actor actor)
    {
        if (actor.IsDead) actor.StateMachine.SetState(ActorState.Die);

        if (actor.OnGround) actor.StateMachine.SetState(ActorState.Idle);
        if (InputHandler.ClickLeft) actor.StateMachine.SetState(ActorState.Attack);
    }

    private static void ChangeFromOnLadder(in Actor actor)
    {
        if (actor.IsDead) actor.StateMachine.SetState(ActorState.Die);

        if ((actor.CanGoUp && actor.OnGround) || !actor.OnLadder) actor.StateMachine.SetState(ActorState.Idle);
    }

    private static void ChangeFromDown(in Actor actor)
    {
        if (actor.IsDead) actor.StateMachine.SetState(ActorState.Die);

        if (!InputHandler.ButtonS) actor.StateMachine.SetState(ActorState.Idle);
        if (actor.CanGoDown) actor.StateMachine.SetState(ActorState.OnLadder);
    }

    private static void ChangeFromAttack(in Actor actor)
    {
        if (actor.IsDead) actor.StateMachine.SetState(ActorState.Die);

        if (!actor.IsAttacking) actor.StateMachine.SetState(ActorState.Idle);
    }

    private static void ChangeFromNoCtrl(in Actor actor)
    {
    }
    private static void ChangeFromDie(in Actor actor)
    {
    }
}
