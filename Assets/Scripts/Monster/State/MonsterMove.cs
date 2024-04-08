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

    public MonsterMove(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        patrolIdx = 0;
        patrolLength = StateMachine.Owner.Stat.patrolMovePos.Length;
        patrolMoveReverse = false;
        StateMachine.Owner.MoveBody.Stop();
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

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
    }

    private void PatrolMove()
    {
        if (!StateMachine.Owner.MoveBody.IsArrived()) return;
        if (patrolLength <= 1) return;

        StateMachine.Owner.MoveBody.MoveToPos(StateMachine.Owner.Stat.patrolMovePos[patrolIdx]);

        if (patrolMoveReverse) patrolIdx--;
        else patrolIdx++;
        if (patrolIdx == patrolLength - 1 || patrolIdx == 0) patrolMoveReverse = !patrolMoveReverse;
    }

    private void RandomMove()
    {
        if (!StateMachine.Owner.MoveBody.IsArrived()) return;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)StateMachine.Owner.Tr.position + randomDirection, randomDirection, 5.0f);
        Debug.DrawRay(StateMachine.Owner.Tr.position, randomDirection * 5.0f, Color.red, 1.0f);
        Debug.Log(hit.distance);
        if (!hit || hit.distance > 1.0f)
        {
            Debug.Log(hit.point);
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
            {
                StateMachine.Owner.MoveBody.MoveToPos(navHit.position);
            }
        }
    }

    public override void FixedUpdateState()
    {

    }

    protected override void ChangeFromState()
    {

    }
}