using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMoveBody
{
    private Monster owner;
    private NavMeshAgent agent;

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

    public Vector2 Velocity { get { return agent.velocity; } }
    private int HorizontalAxis;
    private int VerticalAxis;
    private Direction moveDir;

    #endregion

    public MonsterMoveBody(Monster owner)
    {
        this.owner = owner;
        agent = Util.GetOrAddComponent<NavMeshAgent>(owner.gameObject);
        agent.updateRotation = false;
        agent.autoTraverseOffMeshLink = false;

        InitSpeed();
    }

    private void InitSpeed()
    {
        if(DayNight.Instance.isDay)
            agent.speed = owner.Stat.dayMoveSpeed.GetValue();
        else
            agent.speed = owner.Stat.nightMoveSpeed.GetValue();

        DayNight.Instance.WhenDayBegins += () => agent.speed = owner.Stat.dayMoveSpeed.GetValue();
        DayNight.Instance.WhenNightBegins += () => agent.speed = owner.Stat.nightMoveSpeed.GetValue();
    }

    public void MoveToPos(Vector3 pos, float speed)
    {
        agent.speed = speed;
        agent.SetDestination(pos);
        if (agent.isOnOffMeshLink)
        {
            Debug.Log("OnOffMeshLink");
            agent.Warp(agent.currentOffMeshLinkData.endPos);
        }
    }

    public bool IsArrived()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Turn()
    {
        HorizontalAxis = agent.velocity.x > 0 ? 1 : agent.velocity.x < 0 ? -1 : 0;
        VerticalAxis = agent.velocity.y > 0 ? 1 : agent.velocity.y < 0 ? -1 : 0;
        if (HorizontalAxis == 0 && VerticalAxis == 0) return;
        else if (HorizontalAxis == 1) MoveDir = Direction.Right;
        else if (HorizontalAxis == -1) MoveDir = Direction.Left;
        else if (VerticalAxis == 1) MoveDir = Direction.Up;
        else if (VerticalAxis == -1) MoveDir = Direction.Down;
    }

    public void Stop()
    {
        agent.velocity = Vector3.zero;
    }

    public void ChangeAgentType(string name)
    {
        int ? agentTypeId = null;
        for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
            if (name == NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
            {
                agentTypeId = settings.agentTypeID;
                break;
            }
        }
        if(agentTypeId != null)
        {
            agent.agentTypeID = (int)agentTypeId;
        }
    }
}
