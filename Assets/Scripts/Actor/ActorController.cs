using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoSingleton<ActorController>
{
    private int nextActor = 0;
    public Actor CurrentActor { get; private set; } = null;
    private List<Actor> actorsList = new List<Actor>();

    public Action BeforeSwitchActorAction = null;
    public Action SwitchActorAction = null;

    private void Awake()
    {
        InitActors();
        InitSwitchActorAction();
    }
    private void Update()
    {
        CurrentActor.ActorUpdate();
    }

    private void FixedUpdate()
    {
        CurrentActor.ActorFixedUpdate();
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
