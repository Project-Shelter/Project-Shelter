using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController
{
    private int nextActor = 0;
    public Actor CurrentActor { get; private set; } = null;
    private List<Actor> actorsList = new List<Actor>();

    public Action BeforeSwitchActorAction = null;
    public Action SwitchActorAction = null;

    public void Init()
    {
        InitActors();
        InitSwitchActorAction();
    }
    public void Update()
    {
        CurrentActor.ActorUpdate();
        SetCursor();
    }
    
    private void SetCursor()
    {
        if (InputHandler.ClickRight) Cursor.SetCursor(InputHandler.AimCursor, InputHandler.CursorHotspot, CursorMode.Auto);
        if (InputHandler.ClickRightUp) Cursor.SetCursor(InputHandler.DefaultCursor, InputHandler.CursorHotspot, CursorMode.Auto);
    }

    public void FixedUpdate()
    {
        CurrentActor.ActorFixedUpdate();
    }
    
    #region ActorSwitching

    private void InitActors()
    {
        Actor[] temp = UnityEngine.Object.FindObjectsOfType<Actor>();
        foreach (var act in temp)
        {
            actorsList.Add(act);
        }

        CurrentActor = actorsList[0];
        nextActor = 1 % actorsList.Count;
    }
    private void InitSwitchActorAction()
    {
        // CurrentActor를 실시간으로 받아오기 위해 람다식으로 묶어놓음
        BeforeSwitchActorAction += () => { CurrentActor.ExitControl(); };
        SwitchActorAction += () => { CurrentActor.EnterControl(); };
    }

    public void SwitchActor()
    {
        BeforeSwitchActorAction?.Invoke();
        CurrentActor = actorsList[nextActor];
        nextActor = (nextActor + 1) % actorsList.Count;
        SwitchActorAction?.Invoke();
    }
    
    #endregion
}
