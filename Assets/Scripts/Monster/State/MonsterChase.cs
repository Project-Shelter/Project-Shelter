using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonsterBaseState
{
    private BreakableObject obstacleOnPath;
    private Vector3 targetPos;
    private int chasePatience;
    private float chaseRadius;
    private float timeOutsideRadius;
    private bool canDetectObj;
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

    public MonsterChase(MonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        StateMachine.Owner.MoveBody.Stop();
        StateMachine.Owner.Anim.SetBool("IsMoving", true);
        StateMachine.Owner.Anim.speed = 1f;
        InitVariables();
    }

    private void InitVariables()
    {
        targetPos = StateMachine.Owner.ChaseTarget.Coll.transform.position;
        chasePatience = StateMachine.Owner.Stat.chasePatience;
        chaseRadius = StateMachine.Owner.Stat.chaseRadius.GetValue();
        timeOutsideRadius = 0f;
        canDetectObj = false;
        obstacleOnPath = null;
    }

    public override void ExitState()
    {
        StateMachine.Owner.MoveBody.Stop();
        StateMachine.Owner.Anim.SetBool("IsMoving", false);
    }

    public override void UpdateState()
    {
        Chase();
        FindObstacleTarget();
        FindAttackTarget();
        CanKeepChasing();
        base.UpdateState();
    }
    private void Chase()
    {
        if(obstacleOnPath == null)
            targetPos = StateMachine.Owner.ChaseTarget.Coll.bounds.center;
        else
            targetPos = obstacleOnPath.Coll.bounds.center;
        float diff = NavMeshController.Instance.CalculateDiff(StateMachine.Owner.Tr.position, targetPos);
        if (diff < 1f)
        {
            canDetectObj = false;
            NavMeshController.Instance.ChangeAgentType(StateMachine.Owner.MoveBody.Agent, Agent.WithObjects);
        }
        else
        {
            canDetectObj = true;
            NavMeshController.Instance.ChangeAgentType(StateMachine.Owner.MoveBody.Agent, Agent.WithoutObjects);
        }
        StateMachine.Owner.MoveBody.MoveToPos(targetPos, ChaseSpeed);
        StateMachine.Owner.MoveBody.Turn();
    }

    private void FindObstacleTarget()
    {
        if (!canDetectObj) return;
        BreakableObject obstacleObj = StateMachine.Owner.Attacker.FindObstacleObj(StateMachine.Owner.MoveBody.Agent.path);
        if (obstacleObj != null)
        {
            obstacleOnPath = obstacleObj;
        }
    }

    private void FindAttackTarget()
    {
        Direction attackDir;
        if(obstacleOnPath != null)
        {
            attackDir = StateMachine.Owner.Attacker.GetDirectionToTarget(obstacleOnPath);
        }
        else
        {
            attackDir = StateMachine.Owner.Attacker.GetDirectionToTarget(StateMachine.Owner.ChaseTarget);
        }
        Collider2D[] hits = StateMachine.Owner.Attacker.GetCollsInAttackRange(attackDir);
        foreach (Collider2D hit in hits)
        {
            if (hit == StateMachine.Owner.ChaseTarget.Coll)
            {
                StateMachine.Owner.AttackTarget = StateMachine.Owner.ChaseTarget;
                break;
            }
            if(obstacleOnPath != null)
            {
                if (hit == obstacleOnPath.Coll)
                {
                    StateMachine.Owner.ObstacleTarget = obstacleOnPath;
                    break;
                }
            }
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
                timeOutsideRadius = 0f;
                chasePatience--;
            }
        }
        else
        {
            timeOutsideRadius = 0f;
            chasePatience = StateMachine.Owner.Stat.chasePatience;
        }
    }

    public override void FixedUpdateState() {}

    protected override void ChangeFromState()
    {
        if (StateMachine.Owner.ChaseTarget == null || chasePatience <= 0)
        {
            StateMachine.SetState("Move");
            return;
        }

        if(StateMachine.Owner.AttackTarget != null)
        {
            StateMachine.SetState("Attack");
            return;
        }

        if(StateMachine.Owner.ObstacleTarget != null && StateMachine.Owner.AttackTarget == null)
        {
            StateMachine.SetState("ObjAttack");
            return;
        }
    }
}
