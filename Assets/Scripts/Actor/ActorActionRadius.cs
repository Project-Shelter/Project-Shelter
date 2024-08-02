using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorActionRadius
{
    Actor actor;
    public ActorActionRadius(Actor actor)
    {
        this.actor = actor;
    }

    public void AlertForMonstersInRadius()
    {
        Collider2D[] colls = FindCollsInRadius();
        foreach(Collider2D coll in colls)
        {
            Monster monster = coll.GetComponent<Monster>();
            if (monster)
            {
                monster.DetectTarget(actor);
            }
        }
    }

    private Collider2D[] FindCollsInRadius()
    {
        Vector2 position = actor.Tr.position;
        float radius = actor.Stat.monsterViewRadius.GetValue();
        int layerMask = actor.gameObject.layer; // 추후 기획 수정에 따라 필요 가능성
        Collider2D[] colls = Physics2D.OverlapCircleAll(position, radius);
        return colls;
    }
}
