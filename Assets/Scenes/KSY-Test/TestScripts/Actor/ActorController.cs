using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoSingleton<ActorController>
{
    private int nextActor = 0;
    public Actor CurrentActor { get; private set; } = null;
    private List<Actor> actorsList = new List<Actor>();

    public Action PrevSwitchActorAction = null;
    public Action SwitchActorAction = null;

    public bool canControl = true;

    private void Awake()
    {
        InitActors();
    }
    private void Update()
    {
        if(canControl) CurrentActor.ActorUpdate();
    }

    private void FixedUpdate()
    {
        if (canControl) CurrentActor.ActorFixedUpdate();
    }
    
    #region ActorSwitching
    private void InitActors()
    {
        Actor[] temp = FindObjectsOfType<Actor>();
        foreach (var act in temp)
        {
            actorsList.Add(act);
        }

        CurrentActor = actorsList[0];
        nextActor = 1 % actorsList.Count;
    }
    public void SwitchActor()
    {
        PrevSwitchActorAction?.Invoke();
        CurrentActor.ExitControl();
        CurrentActor = actorsList[nextActor];
        nextActor = (nextActor + 1) % actorsList.Count;
        CurrentActor.EnterControl();
        SwitchActorAction?.Invoke();
    }
    
    #endregion

    #region ActorMovement
    private void Move(float speed)
    {
        if (CurrentActor.GoRight)
        {            
            CurrentActor.Tr.rotation = CurrentActor.Left;
            CurrentActor.Rigid.velocity = new Vector2(speed * 1.0f, CurrentActor.Rigid.velocity.y);
        }
        else
        {
            CurrentActor.Tr.rotation = CurrentActor.Right;
            CurrentActor.Rigid.velocity = new Vector2(speed * -1.0f, CurrentActor.Rigid.velocity.y);
        }
    }
    public void Move()
    {
        Move(CurrentActor.Stat.moveSpeed.GetValue());
    }

    public void Turn()
    {
        if (CurrentActor.GoRight) { CurrentActor.Tr.rotation = CurrentActor.Left; }
        else { CurrentActor.Tr.rotation = CurrentActor.Right; }
    }
    public void Jump()
    {
        if (CurrentActor.StateMachine.CurrentState != ActorState.OnAir)
        {
            CurrentActor.Rigid.velocity = new Vector2(0f, 1f);
            CurrentActor.Rigid.AddForce(Vector2.up * CurrentActor.Stat.jumpForce.GetValue(), ForceMode2D.Impulse);
        }
    }
    public void Stop()
    {
        CurrentActor.Rigid.velocity = Vector2.zero;
    }

    public void GoUp()
    {
        CurrentActor.Tr.position += CurrentActor.Stat.moveOnLadderSpeed.GetValue() * Time.deltaTime * Vector3.up;
    }
    public void GoDown()
    {
        CurrentActor.Tr.position += CurrentActor.Stat.moveOnLadderSpeed.GetValue() * Time.deltaTime * Vector3.down;
    }

    #endregion

    #region SetActorMovementState

    public void NoGravity()
    {
        CurrentActor.Rigid.gravityScale = 0;
    }
    private void SetGravity(float gravity)
    {
        CurrentActor.Rigid.gravityScale = CurrentActor.Stat.defaultGravity.GetValue();
    }
    public void SetGravity()
    {
        SetGravity(CurrentActor.Stat.defaultGravity.GetValue());
    }

    #endregion

}
