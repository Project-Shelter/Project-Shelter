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

    public Collider2D[] CollsInAttackRange(Direction dir)
    {
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

    public BreakableObject MeetWithObject(Direction dir)
    {
        Collider2D[] hits = CollsInAttackRange(dir);
        foreach(Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out BreakableObject breakableObject)) return breakableObject;
        }
        return null;
    }
}
