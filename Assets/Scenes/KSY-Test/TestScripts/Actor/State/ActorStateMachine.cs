using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStateMachine
{
    private Dictionary<ActorState, ActorBaseState> stateDict = new Dictionary<ActorState, ActorBaseState>();
    public ActorState CurrentState { get; private set; }
    public ActorStateMachine(Actor actor)
    {
        InitState(actor);
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

    private void InitState(Actor actor)
    {
        stateDict.Add(ActorState.Idle, new ActorIdle(actor));
        stateDict.Add(ActorState.Move, new ActorMove(actor));
        stateDict.Add(ActorState.OnAir, new ActorOnAir(actor));
        stateDict.Add(ActorState.OnLadder, new ActorOnLadder(actor));
        stateDict.Add(ActorState.Down, new ActorDown(actor));
        stateDict.Add(ActorState.Attack, new ActorAttack(actor));
        stateDict.Add(ActorState.NoControl, new ActorNoControl(actor));
        stateDict.Add(ActorState.Die, new ActorDie(actor));
    }
    
}
