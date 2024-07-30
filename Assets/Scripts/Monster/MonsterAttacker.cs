using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttacker
{
    private Monster owner;
    public event Action OnAttackEnded;

    public MonsterAttacker(Monster owner)
    {
        this.owner = owner;
    }

    public Collider2D[] GetCollsInAttackRange(Direction dir)
    {
        Vector2 collCenter = owner.Coll.bounds.center;
        Vector2 collSize = owner.Coll.bounds.extents;
        float attackRange = owner.Stat.attackRange.GetValue();
        Vector2 attackCenter;

        switch(dir)
        {
            case Direction.Up:
                attackCenter = new(collCenter.x, collCenter.y - collSize.y / 2);
                break;
            case Direction.Down:
                attackCenter = new(collCenter.x, collCenter.y - collSize.y / 2);
                break;
            case Direction.Left:
                attackCenter = new(collCenter.x + -collSize.x, collCenter.y - collSize.y / 2);
                break;
            default:
                attackCenter = new(collCenter.x + collSize.x, collCenter.y - collSize.y / 2);
                break;
        }
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCenter, attackRange, 1 << owner.gameObject.layer);
        return hits;
    }

    public BreakableObject FindObstacleObj(NavMeshPath path)
    {
        if (path.status != NavMeshPathStatus.PathComplete) return null;

        if (path.corners.Length > 1)
        {
            Vector2 origin = path.corners[0];
            Vector2 dir = path.corners[1] - path.corners[0];

            float distance = owner.Stat.attackRange.GetValue() < dir.magnitude ? owner.Stat.attackRange.GetValue() : dir.magnitude;
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, distance, 1 << owner.gameObject.layer);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit && hit.collider.TryGetComponent(out BreakableObject obstacleObj))
                {  
                    return obstacleObj;
                }
            }
        }
        return null;
    }

    public Direction GetDirectionToTarget(ILivingEntity target)
    {
        Collider2D coll = target.Coll;
        Vector2 closestPoint = coll.ClosestPoint(owner.Coll.bounds.center);
        Vector2 normalVector = (closestPoint - (Vector2)owner.Coll.bounds.center).normalized;
        float attackAngle = Vector2.SignedAngle(Vector2.right, normalVector);
        if (attackAngle < 0) { attackAngle += 360; }

        if(attackAngle >= 45 && attackAngle < 135)
        {
            return Direction.Up;
        }
        else if(attackAngle >= 135 && attackAngle < 225)
        {
            return Direction.Left;
        }
        else if(attackAngle >= 225 && attackAngle < 315)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.Right;
        }
    }

    public void Attack(ILivingEntity target)
    {
        Direction attackDir = GetDirectionToTarget(target);
        Collider2D[] hits = GetCollsInAttackRange(attackDir);

        Vector2 hitPoint = target.Coll.ClosestPoint(owner.Coll.bounds.center);
        Vector2 hitNormal = (hitPoint - (Vector2)owner.Coll.bounds.center).normalized;

        foreach (Collider2D hit in hits)
        {
            if (hit == target.Coll)
            {
                Debug.Log("Attack to " + target);
                target.OnDamage(owner.Stat.attackDamage.GetValue(), hitPoint, hitNormal, owner);
            }
        }
        OnAttackEnded?.Invoke();
    }
}
