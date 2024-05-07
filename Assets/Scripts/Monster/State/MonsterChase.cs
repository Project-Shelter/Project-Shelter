using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonsterBaseState
{
    private Vector3 targetPos;
    private int chasePatience;
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
        targetPos = StateMachine.Owner.ChaseTarget.transform.position;
        chasePatience = StateMachine.Owner.Stat.chasePatience;
        chaseRadius = StateMachine.Owner.Stat.chaseRadius.GetValue();
        StateMachine.Owner.MoveBody.Stop();
        StateMachine.Owner.MoveBody.ChangeAgentType("Chase");
        timeOutsideRadius = 0f;
    }

    public override void ExitState()
    {
        StateMachine.Owner.Anim.SetBool("IsMoving", false);
        StateMachine.Owner.ChaseTarget = null;
        StateMachine.Owner.MoveBody.Stop();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(StateMachine.Owner.ChaseTarget == null)
        {
            return;
        }

        CanAttackTarget();
        Chase();
        CanKeepChasing();
    }

    private void CanAttackTarget()
    {
        Collider2D obstacleCollider = null;
        Collider2D[] hits = StateMachine.Owner.MonsterAttacker.CollsInAttackRange(StateMachine.Owner.MoveBody.MoveDir);
        foreach (Collider2D hit in hits)
        {
            if (obstacleCollider == null && hit.TryGetComponent<BreakableObject>(out _)) 
            {
                obstacleCollider = hit;
            }
            if (hit == StateMachine.Owner.ChaseTarget)
            {
                StateMachine.Owner.AttackTarget = StateMachine.Owner.ChaseTarget;
            }
        }

        if(StateMachine.Owner.AttackTarget == null && obstacleCollider != null)
        {
            StateMachine.Owner.AttackTarget = obstacleCollider;
        }
    }

    private void CanKeepChasing()
    {
        float distance = Vector3.Distance(StateMachine.Owner.Tr.position, targetPos);
        if(distance > chaseRadius)
        {
            timeOutsideRadius += Time.deltaTime;
            if(timeOutsideRadius >= 1f)
            {
                chasePatience--;
                timeOutsideRadius = 0f;
            }
        }
        else
        {
            chasePatience = StateMachine.Owner.Stat.chasePatience;
        }
    }

    private void Chase()
    {
        targetPos = StateMachine.Owner.ChaseTarget.transform.position;
        StateMachine.Owner.MoveBody.MoveToPos(targetPos, ChaseSpeed);
        StateMachine.Owner.MoveBody.Turn();
    }

    public override void FixedUpdateState()
    {

    }

    protected override void ChangeFromState()
    {
        if (chasePatience <= 0)
        {
            StateMachine.SetState("Move");
            return;
        }

        if(StateMachine.Owner.AttackTarget != null)
        {
            StateMachine.SetState("Attack");
            return;
        }
    }
}
