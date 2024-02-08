using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MonsterObserver
{
    public Actor Observe(Vector2 point, Vector2 offset, Vector2 size)
    {
        Actor target = null;
        float minDist = float.MaxValue;

        Collider2D[] hit = Physics2D.OverlapBoxAll(point + offset, size, 0, 1 << (int)Define.Layer.Character);
        foreach (Collider2D collider in hit)
        {
            if (collider.CompareTag("Player"))
            {
                Actor now = collider.GetComponent<Actor>();
                if (IsHitWall(point, Mathf.Sign(offset.x), Mathf.Abs(offset.x)) || now.IsDead) { continue; }

                float dist = Vector2.Distance(collider.transform.position, point);
                if (minDist > dist)
                {
                    minDist = dist;
                    target = now;
                }
            }
        }

        return target;
    }
    private bool IsHitWall(Vector2 origin, float direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * direction, distance, 1 << (int)Define.Layer.Wall);
        if (hit) { return true; }
        else { return false; }
    }
}
