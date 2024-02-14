using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    private void OnEnable()
    {
        ActorState actorState = ActorController.Instance.CurrentActor.StateMachine.CurrentState;
        if ((actorState != ActorState.OnLadder) && (actorState != ActorState.Die))
            ActorController.Instance.CurrentActor.StateMachine.SetState(ActorState.Idle);
        ActorController.Instance.canControl = false;
    }
    private void OnDisable()
    {
        ActorController.Instance.canControl = true;
    }
}
