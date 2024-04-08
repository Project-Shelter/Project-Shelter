using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMoveBody : MonoBehaviour
{
    private Monster owner;
    private Rigidbody2D rigid;
    private NavMeshAgent agent;

    #region Movement Variables

    private int HorizontalAxis;
    private int VerticalAxis;
    
    #endregion
    
    public MonsterMoveBody(Monster owner)
    {
        this.owner = owner;
        rigid = Util.GetOrAddComponent<Rigidbody2D>(owner.gameObject);
        agent = Util.GetOrAddComponent<NavMeshAgent>(owner.gameObject);
        agent.updateRotation = false;

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

    public void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
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

    public void Stop()
    {
        agent.velocity = Vector3.zero;
    }
}
