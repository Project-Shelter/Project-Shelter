using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{
    Idle,
    Melee,
    Range,
    Reload,
    Disarm,
}

public class ActorAttackStateMachine
{
    private Actor actor;
    private Dictionary<AttackState, ActorAttackState> stateDict = new Dictionary<AttackState, ActorAttackState>();
    public List<AttackState> CanSwitchStates { get; private set; }
    public AttackState CurrentState { get; private set; }
    public ActorAttackStateMachine(Actor actor)
    {
        this.actor = actor;
        InitState();
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
        stateDict.Add(AttackState.Reload, new ActorAttackReload(actor));
        stateDict.Add(AttackState.Disarm, new ActorAttackDisarm(actor));

        CanSwitchStates = new List<AttackState> { AttackState.Idle, AttackState.Disarm };

        CurrentState = AttackState.Idle;
        stateDict[CurrentState].EnterState();
    }

}
