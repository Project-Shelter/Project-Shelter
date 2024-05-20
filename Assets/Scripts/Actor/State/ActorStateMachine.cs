using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public enum ActorState
{
    Idle,
    Walk,
    Dash,
    Conceal,
    Die,
    Switch,
}

public class ActorStateMachine
{
    private Actor actor;
    private Dictionary<ActorState, ActorBaseState> stateDict = new Dictionary<ActorState, ActorBaseState>();
    public ActorState CurrentState { get; private set; }
    public ActorStateMachine(Actor actor)
    {
        this.actor = actor;
        InitState();
        SetState(ActorState.Idle);
    }

    public void StateUpdate()
    {
        stateDict[CurrentState].UpdateState();
    }
    public void StateFixedUpdate() { stateDict[CurrentState].FixedUpdateState(); }
    
    public void SetState(ActorState state)
    {
        if (CurrentState == state)
            return;
        
        stateDict[CurrentState].ExitState();
        CurrentState = state;
        stateDict[state].EnterState();
    }

    private void InitState()
    {
        stateDict.Add(ActorState.Idle, new ActorIdle(actor));
        stateDict.Add(ActorState.Walk, new ActorWalk(actor));
        stateDict.Add(ActorState.Dash, new ActorDash(actor));
        stateDict.Add(ActorState.Conceal, new ActorConceal(actor));
        stateDict.Add(ActorState.Die, new ActorDie(actor));
        stateDict.Add(ActorState.Switch, new ActorSwitch(actor));
    }
    
}
