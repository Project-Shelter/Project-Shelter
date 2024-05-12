using System.Collections.Generic;
using UnityEngine;
using static MonsterStateManager;

public class MonsterAttack : MonsterBaseState
{
    private List<Collider2D> attackedColls = new();
    public MonsterAttack(in MonsterStateManager manager) : base(manager) { }

    public override void OnStateEnter()
    {
        Manager.Animator.SetFloat("MovingBlend", 0f);
        Manager.Animator.Play("Attack");
    }

    public override void OnStateUpdate()
    {
        Manager.Move(Vector2.zero);

        float nowAnimTime = Manager.Animator.GetCurrentAnimatorStateInfo((int)AnimationLayer.Attack).normalizedTime;
        if (nowAnimTime >= 0.375 && nowAnimTime <= 0.525)
        {
            Attack();
        }
        else if (nowAnimTime >= 1.0)
        {
            Manager.canAttack = false;
        }
    }

    public override void OnStateFixedUpdate()
    {

    }

    public override void OnStateExit()
    {
        Manager.Animator.Play("Empty", (int)AnimationLayer.Attack);
        attackedColls.Clear();
        Manager.canAttack = false;
    }

    private void Attack()
    {
        Vector2 collCenter = Manager.Coll.bounds.center;
        Vector2 attackCenter = new(collCenter.x + Manager.Direction * Manager.Stat.attackRange.GetValue() / 2, collCenter.y);
        Vector2 attackSize = new(Manager.Stat.attackRange.GetValue(), Manager.Coll.bounds.size.y);

        Collider2D[] hit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0, 1 << (int)Define.Layer.Character);
        foreach (Collider2D collider in hit)
        {
            if (collider.CompareTag("Player"))
            {
                ILivingEntity target = collider.GetComponentInParent<ILivingEntity>();
                if (!attackedColls.Contains(collider) && target != null)
                {
                    Vector2 hitPoint = collider.ClosestPoint((Vector2)Manager.transform.position + Manager.Stat.AttackPoint);
                    Vector2 hitNormal = hitPoint - Manager.Stat.AttackPoint;
                    target.OnDamage(Manager.Stat.attackDamage.GetValue(), hitPoint, hitNormal);
                    attackedColls.Add(collider);
                }
            }
        }
    }
}
