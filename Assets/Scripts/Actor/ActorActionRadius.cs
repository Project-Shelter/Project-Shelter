using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActorActionRadius
{
    Actor actor;
    private float actionRadius;
    public ActorActionRadius(Actor actor)
    {
        this.actor = actor;
        actionRadius = actor.Stat.monsterViewRadius.GetValue();
    }

    public void AlertForMonstersInRadius()
    {
        Vector2 position = actor.Tr.position;
        float radius = actionRadius;
        int layerMask = actor.gameObject.layer;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);

        foreach(var collider in colliders)
        {
            Monster monster = collider.GetComponent<Monster>();
            if (monster)
            {
                monster.DetectTarget(actor.Coll);
            }
        }
    }

}
