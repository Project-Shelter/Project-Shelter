using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{
    Idle,
    Melee,
    Range,
}

public class ActorAttackStateMachine
{
    private Actor actor;
    private Dictionary<AttackState, ActorAttackState> stateDict = new Dictionary<AttackState, ActorAttackState>();
    public AttackState CurrentState { get; private set; }
    public ActorAttackStateMachine(Actor actor)
    {
        this.actor = actor;
        InitState();
        SetState(AttackState.Idle);
    }

    public void StateUpdate() { stateDict[CurrentState].UpdateState(); }
    public void StateFixedUpdate() { stateDict[CurrentState].FixedUpdateState(); }

    public void SetState(AttackState state)
    {
        if (CurrentState == state) return;

        stateDict[CurrentState].ExitState();
        CurrentState = state;
        stateDict[state].EnterState();
    }

    private void InitState()
    {
        stateDict.Add(AttackState.Idle, new ActorAttackIdle(actor));
        stateDict.Add(AttackState.Melee, new ActorAttackMelee(actor));
        stateDict.Add(AttackState.Range, new ActorAttackRange(actor));
    }

}
