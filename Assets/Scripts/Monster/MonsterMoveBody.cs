using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class MonsterMoveBody
{
    private Monster owner;
    public NavMeshAgent Agent { get; private set; }

    public Direction MoveDir
    {
        get { return moveDir; }
        private set
        {
            if (value != moveDir)
            {
                moveDir = value;
                owner.Anim.SetFloat("MoveDirection", (float)value);
            }
        }
    }

    #region Movement Variables

    public Vector2 Velocity { get { return Agent.velocity; } }
    private int HorizontalAxis;
    private int VerticalAxis;
    private Direction moveDir;

    #endregion

    public MonsterMoveBody(Monster owner)
    {
        this.owner = owner;
        Agent = Util.GetOrAddComponent<NavMeshAgent>(owner.gameObject);
        Agent.updateRotation = false;
        Agent.autoTraverseOffMeshLink = false;

        InitSpeed();
    }

    private void InitSpeed()
    {
        if(ServiceLocator.GetService<DayNight>().isDay)
            Agent.speed = owner.Stat.dayMoveSpeed.GetValue();
        else
            Agent.speed = owner.Stat.nightMoveSpeed.GetValue();

        ServiceLocator.GetService<DayNight>().WhenDayBegins += () => Agent.speed = owner.Stat.dayMoveSpeed.GetValue();
        ServiceLocator.GetService<DayNight>().WhenNightBegins += () => Agent.speed = owner.Stat.nightMoveSpeed.GetValue();
    }

    public void MoveToPos(Vector3 pos, float speed)
    {
        Agent.isStopped = false;
        Agent.speed = speed;
        Agent.SetDestination(pos);
        if (Agent.isOnOffMeshLink)
        {
            Agent.Warp(Agent.currentOffMeshLinkData.endPos);
        }
    }

    public bool IsArrived()
    {
        if (!Agent.pathPending && (Agent.remainingDistance <= Agent.stoppingDistance))
        {
            return true;
        }
        return false;
    }

    public void Turn()
    {
        bool isHorizontal = Math.Abs(Agent.velocity.x) > Math.Abs(Agent.velocity.y);
        HorizontalAxis = Agent.velocity.x > 0 ? 1 : Agent.velocity.x < 0 ? -1 : 0;
        VerticalAxis = Agent.velocity.y > 0 ? 1 : Agent.velocity.y < 0 ? -1 : 0;
        if (HorizontalAxis == 0 && VerticalAxis == 0) return;
        else if (HorizontalAxis == 1 && isHorizontal) MoveDir = Direction.Right;
        else if (HorizontalAxis == -1 && isHorizontal) MoveDir = Direction.Left;
        else if (VerticalAxis == 1) MoveDir = Direction.Up;
        else if (VerticalAxis == -1) MoveDir = Direction.Down;
    }

    public void Stop()
    {
        Agent.isStopped = true;
    }
}
