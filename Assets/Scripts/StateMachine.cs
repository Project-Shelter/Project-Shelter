using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T> where T : StateMachine<T>
{
    protected Dictionary<string, BaseState<T>> stateDict = new();
    public string CurrentState { get; private set; }

    public void Init(string initState)
    {
        SetState(initState);
    }

    public void StateUpdate() { stateDict[CurrentState].UpdateState(); }
    public void StateFixedUpdate() { stateDict[CurrentState].FixedUpdateState(); }

    public void SetState(string state)
    {
        if(!stateDict.ContainsKey(state))
        {
            Debug.LogError("Invalid State");
            return;
        }

        if (CurrentState == state) return;
        stateDict[CurrentState].ExitState();
        CurrentState = state;
        stateDict[state].EnterState();
    }

    public void AddState(string stateName, BaseState<T> state)
    {
        stateDict.Add(stateName, state);
    }
}
