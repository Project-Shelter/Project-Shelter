using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonsterBaseState
{
    private BreakableObject obstacleOnPath;
    private ILivingEntity chaseTarget;
    private int chasePatience;
    private float chaseRadius;
    private float timeOutsideRadius;
    private float ChaseSpeed
    {
        get
        {
            if (Managers.GetCurrentScene<GameScene>().DayNight.isDay)
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
        chaseTarget = StateMachine.Owner.DetectedTarget;
        chasePatience = StateMachine.Owner.Stat.chasePatience;
        chaseRadius = StateMachine.Owner.Stat.chaseRadius.GetValue();
        timeOutsideRadius = 0f;
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
        SetTarget();
        FindAttackTarget();
        CanKeepChasing();
        base.UpdateState();
    }

    private void SetTarget()
    {
        if (obstacleOnPath) { chaseTarget = obstacleOnPath; }
        else { chaseTarget = StateMachine.Owner.DetectedTarget; }
    }

    private void Chase()
    {
        Vector3 targetPos = chaseTarget.Coll.bounds.center;
        float diff = NavMeshController.Instance.CalculateDiff(StateMachine.Owner.Tr.position, targetPos);
        if (diff < 3f) { NavMeshController.Instance.ChangeAgentType(StateMachine.Owner.MoveBody.Agent, Agent.WithObjects); }
        else { NavMeshController.Instance.ChangeAgentType(StateMachine.Owner.MoveBody.Agent, Agent.WithoutObjects); }

        StateMachine.Owner.MoveBody.MoveToPos(targetPos, ChaseSpeed);
        StateMachine.Owner.MoveBody.Turn();
        obstacleOnPath = StateMachine.Owner.Attacker.FindObstacleObj(StateMachine.Owner.MoveBody.Agent.path);
    }

    private void FindAttackTarget()
    {
        Direction attackDir = StateMachine.Owner.Attacker.GetDirectionToTarget(chaseTarget);
        Collider2D[] hits = StateMachine.Owner.Attacker.GetCollsInAttackRange(attackDir);
        foreach (Collider2D hit in hits)
        {
            if(hit == chaseTarget.Coll)
            {
                if(chaseTarget is BreakableObject obj && chaseTarget == (ILivingEntity)obstacleOnPath) { StateMachine.Owner.ObstacleTarget = obj; }
                else { StateMachine.Owner.AttackTarget = chaseTarget; }
                break;
            }
        }
    }

    private void CanKeepChasing()
    {
        float distance = Vector3.Distance(StateMachine.Owner.Tr.position, StateMachine.Owner.DetectedTarget.Coll.bounds.center);
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
        if (StateMachine.Owner.DetectedTarget == null || chasePatience <= 0)
        {
            StateMachine.SetState("Move");
            return;
        }

        if(StateMachine.Owner.AttackTarget != null)
        {
            StateMachine.SetState("Attack");
            return;
        }

        if(StateMachine.Owner.ObstacleTarget != null)
        {
            StateMachine.SetState("ObjAttack");
            return;
        }
    }
}
