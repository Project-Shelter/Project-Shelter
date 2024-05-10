using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttacker
{
    private Monster owner;

    public MonsterAttacker(Monster owner)
    {
        this.owner = owner;
    }

    public Collider2D[] GetCollsInAttackRange(Direction dir)
    {
        Debug.Log(dir);
        Vector2 collCenter = owner.Coll.bounds.center;
        Vector2 collSize = owner.Coll.bounds.size;
        float attackRange = owner.Stat.attackRange.GetValue();
        Vector2 attackCenter;
        Vector2 attackSize;

        switch(dir)
        {
            case Direction.Up:
                attackSize = new(collSize.x / 2, attackRange);
                attackCenter = new(collCenter.x, collCenter.y + 1 * attackRange);
                break;
            case Direction.Down:
                attackSize = new(collSize.x / 2, attackRange);
                attackCenter = new(collCenter.x, collCenter.y + -1 * attackRange);
                break;
            case Direction.Left:
                attackSize = new(attackRange, collSize.y / 2);
                attackCenter = new(collCenter.x + -1 * attackRange / 2, collCenter.y);
                break;
            default:
                attackSize = new(attackRange, collSize.y / 2);
                attackCenter = new(collCenter.x + 1 * attackRange / 2, collCenter.y);
                break;
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0, 1 << owner.gameObject.layer);
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

    public bool Attack(ILivingEntity target)
    {
        Collider2D coll = target.Coll;
        Vector2 hitPoint = coll.ClosestPoint(owner.Coll.bounds.center);
        Vector2 hitNormal = (hitPoint - (Vector2)owner.Pos).normalized;
        Debug.Log(hitNormal);
        float attackAngle = Vector2.SignedAngle(Vector2.right, hitNormal);
        if (attackAngle < 0) { attackAngle += 360; }

        Collider2D[] hits;
        if (attackAngle >= 315 || attackAngle < 45) hits = GetCollsInAttackRange(Direction.Right);
        else if (attackAngle >= 45 && attackAngle < 135) hits = GetCollsInAttackRange(Direction.Up);
        else if (attackAngle >= 135 && attackAngle < 225) hits = GetCollsInAttackRange(Direction.Left);
        else hits = GetCollsInAttackRange(Direction.Down);

        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit);
            if (hit == target.Coll)
            {
                target.OnDamage(owner.Stat.attackDamage.GetValue(), hitPoint, hitNormal);
                return true;
            }
        }
        return false;
    }
}
