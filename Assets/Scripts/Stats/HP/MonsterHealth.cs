using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : LivingEntity
{
    private Monster owner;

    public MonsterHealth(Monster owner, float minHP, float maxHP) : base(minHP, maxHP)
    {
        this.owner = owner;
    }

    public override void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        // 넉백
        owner.MoveBody.Knockback(hitNormal);
    }

    public override void RestoreHP(float restoreHP)
    {
        base.RestoreHP(restoreHP);
    }
}
