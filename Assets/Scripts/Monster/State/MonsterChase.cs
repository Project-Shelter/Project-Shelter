using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonsterBaseState
{
    private Vector3 targetPos;
    private int chasePoint;
    private float chaseRadius;
    private float timeOutsideRadius;
    private float ChaseSpeed
    {
        get
        {
            if (DayNight.Instance.isDay)
                return StateMachine.Owner.Stat.dayChaseSpeed.GetValue();
            else
                return StateMachine.Owner.Stat.nightChaseSpeed.GetValue();
        }

    }

    public MonsterChase(MonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void EnterState()
    {
        StateMachine.Owner.Anim.SetBool("IsMoving", true);
        StateMachine.Owner.Anim.speed = 1f;
        targetPos = StateMachine.Owner.Target.position;
        chasePoint = StateMachine.Owner.Stat.chasePoint;
        chaseRadius = StateMachine.Owner.Stat.chaseRadius.GetValue();
        timeOutsideRadius = 0f;
    }

    public override void ExitState()
    {
        StateMachine.Owner.Target = null;
        StateMachine.Owner.MoveBody.Stop();
        StateMachine.Owner.Anim.SetBool("IsMoving", true);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Chase();
        CanKeepChasing();
    }

    private void CanKeepChasing()
    {
        float distance = Vector3.Distance(StateMachine.Owner.Tr.position, targetPos);
        if(distance > chaseRadius)
        {
            timeOutsideRadius += Time.deltaTime;
            if(timeOutsideRadius >= 1f)
            {
                chasePoint--;
                timeOutsideRadius = 0f;
            }
        }
        else
        {
            chasePoint = StateMachine.Owner.Stat.chasePoint;
        }
    }

    private void Chase()
    {
        targetPos = StateMachine.Owner.Target.position;
        StateMachine.Owner.MoveBody.MoveToPos(targetPos, ChaseSpeed);
        StateMachine.Owner.MoveBody.Turn();
    }

    public override void FixedUpdateState()
    {

    }

    protected override void ChangeFromState()
    {
        if (chasePoint <= 0)
        {
            StateMachine.SetState("Move");
            return;
        }

        if(StateMachine.Owner.MoveBody.IsArrived())
        {
            // StateMachine.SetState("Attack");
        }
    }
}
