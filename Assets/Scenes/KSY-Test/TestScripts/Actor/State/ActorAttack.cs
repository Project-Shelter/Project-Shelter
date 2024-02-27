using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAttack : ActorBaseState
{
    private HashSet<IDamageable> attackedObjs = new();

    public ActorAttack(Actor actor) : base(actor) { }

    public override void EnterState()
    {
        //Actor.Rigid.velocity = new(0f, Actor.Rigid.velocity.y);
        Actor.Body.Velocity = new(0f, Actor.Body.Velocity.y);
        Actor.Anim.speed = Actor.Stat.attackSpeed.GetValue();
        Actor.Anim.Play("Player Attack");
    }

    public override void UpdateState()
    {

        float nowAnimTime = Actor.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (nowAnimTime >= 0.125 && nowAnimTime <= 0.375)
        {
            Attack();
        }
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState() 
    {
        attackedObjs.Clear();
        Actor.Anim.speed = 1.0f;
    }

    private void Attack()
    {
        float direction = Mathf.Sign(Actor.transform.rotation.y);
        Vector2 collCenter = Actor.Coll.bounds.center;
        Vector2 attackCenter = new(collCenter.x + direction * Actor.Stat.attackRange.GetValue() / 2, collCenter.y);
        Vector2 attackSize = new(Actor.Stat.attackRange.GetValue(), Actor.Coll.bounds.size.y);
        Collider2D[] hit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0);
        foreach (Collider2D collider in hit)
        {
            if (collider.CompareTag("Enemy"))
            {
                IDamageable target = collider.GetComponentInParent<IDamageable>();
                if (target != null && !attackedObjs.Contains(target))
                {
                    attackedObjs.Add(target);
                    Vector2 hitPoint = collider.ClosestPoint((Vector2)Actor.transform.position + Actor.Stat.AttackPoint);
                    Vector2 hitNormal = hitPoint - Actor.Stat.AttackPoint;
                    target.OnDamage(Actor.Stat.attackDamage.GetValue(), hitPoint, hitNormal);
                }
            }
        }
    }
}
