using System.Collections.Generic;
using UnityEngine;

public enum ActorState
{
    Idle,
    Walk,
    Dash,
    Interact,
    UseItem,
    Die,
}

public class ActorStateMachine
{
    private Actor actor;
    private Dictionary<ActorState, ActorBaseState> stateDict = new Dictionary<ActorState, ActorBaseState>();
    public List<ActorState> CanSwitchStates { get; private set; }
    public List<ActorState> CanAttackStates { get; private set; }
    public ActorState CurrentState { get; private set; }
    public ActorStateMachine(Actor actor)
    {
        this.actor = actor;
        InitState();
    }

    public void StateUpdate() { stateDict[CurrentState].UpdateState(); }
    public void StateFixedUpdate() { stateDict[CurrentState].FixedUpdateState(); }
    public void StateUpdateWithNoCtrl() { stateDict[CurrentState].UpdateWithNoCtrl(); }
    
    public void SetState(ActorState state)
    {
        if (CurrentState == state) return;

        stateDict[CurrentState].ExitState();
        CurrentState = state;
        stateDict[state].EnterState();
    }

    private void InitState()
    {
        stateDict.Add(ActorState.Idle, new ActorIdle(actor));
        stateDict.Add(ActorState.Walk, new ActorWalk(actor));
        stateDict.Add(ActorState.Dash, new ActorDash(actor));
        stateDict.Add(ActorState.Interact, new ActorInteract(actor));
        stateDict.Add(ActorState.UseItem, new ActorUseItem(actor));
        stateDict.Add(ActorState.Die, new ActorDie(actor));

        CanSwitchStates = new List<ActorState> { ActorState.Idle, ActorState.Walk, ActorState.Interact, ActorState.Die };
        CanAttackStates = new List<ActorState> { ActorState.Idle, ActorState.Walk };

        CurrentState = ActorState.Idle;
        stateDict[CurrentState].EnterState();
    }
    
}
