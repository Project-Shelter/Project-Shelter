using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum MonsterMoveType
{
    Hold,
    Patrol,
    Random,
}

public class MonsterMove : MonsterBaseState
{
    private int patrolIdx;
    private int patrolLength;
    private bool patrolMoveReverse;
    private Vector3 lastPoint;
    private float MoveSpeed
    {
        get
        {
            if (DayNight.Instance.isDay)
                return StateMachine.Owner.Stat.dayMoveSpeed.GetValue();
            else
                return StateMachine.Owner.Stat.nightMoveSpeed.GetValue();
        }

    }

    public MonsterMove(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        patrolIdx = 0;
        patrolLength = StateMachine.Owner.Stat.patrolMovePos.Length;
        patrolMoveReverse = false;
        lastPoint = stateMachine.Owner.Tr.position;
    }

    public override void EnterState()
    {
        StateMachine.Owner.Anim.SetBool("IsMoving", true);
        StateMachine.Owner.Anim.speed = 0.5f;
        StateMachine.Owner.MoveBody.ChangeAgentType("Idle");
    }

    public override void ExitState()
    {
        StateMachine.Owner.MoveBody.Stop();
        StateMachine.Owner.Anim.SetBool("IsMoving", false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Move();
    }

    private void Move()
    {
        switch(StateMachine.Owner.Stat.moveType)
        {
            case MonsterMoveType.Hold:
                break;
            case MonsterMoveType.Patrol:
                PatrolMove();
                break;
            case MonsterMoveType.Random:
                RandomMove();
                break;
        }

        StateMachine.Owner.MoveBody.Turn();
    }

    private void PatrolMove()
    {
        if (!StateMachine.Owner.MoveBody.IsArrived()) return;
        if (patrolLength <= 1) return;

        StateMachine.Owner.MoveBody.MoveToPos(StateMachine.Owner.Stat.patrolMovePos[patrolIdx], MoveSpeed);

        if (patrolMoveReverse) patrolIdx--;
        else patrolIdx++;
        if (patrolIdx == patrolLength - 1 || patrolIdx == 0) patrolMoveReverse = !patrolMoveReverse;
    }

    private void RandomMove()
    {
        if (!StateMachine.Owner.MoveBody.IsArrived()) return;

        if (RandomPoint(StateMachine.Owner.Tr.position, 5f, out Vector3 result))
        {
            lastPoint = result;
            StateMachine.Owner.MoveBody.MoveToPos(result, MoveSpeed);
        }
        else
        {
            StateMachine.Owner.MoveBody.MoveToPos(lastPoint, MoveSpeed);
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle;
            Vector3 randomPoint = center + randomDirection * range;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public override void FixedUpdateState()
    {

    }

    protected override void ChangeFromState()
    {
        if(StateMachine.Owner.Target != null)
        {
            StateMachine.SetState("Chase");
        }
    }
}