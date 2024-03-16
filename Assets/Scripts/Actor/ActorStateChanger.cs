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
            case ActorState.Walk:
                ChangeFromMove(actor);
                break;
            case ActorState.Dash:
                ChangeFromDash(actor);
                break;
            case ActorState.Die:
                ChangeFromDie(actor);
                break;
            case ActorState.Switch:
                ChangeFromNoCtrl(actor);
                break;
        }
    }

    private static void ChangeFromAny(in Actor actor)
    {
    }

    private static void ChangeFromIdle(in Actor actor)
    {
    }

    private static void ChangeFromDash(in Actor actor)
    {
    }

    private static void ChangeFromMove(in Actor actor)
    {
    }

    private static void ChangeFromNoCtrl(in Actor actor)
    {
    }
    private static void ChangeFromDie(in Actor actor)
    {
    }
}
