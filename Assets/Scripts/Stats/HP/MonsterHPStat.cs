using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPStat : LivingEntity
{
    // 추후 MonsterStat MonoBehaviour 제거 후 Stat에서 받음
    private MonsterStateManager manager;
    private MonsterStat stat;

    public MonsterHPStat(MonsterStat stat, float minHP, float maxHP) : base(minHP, maxHP)
    {
        this.stat = stat;
        manager = stat.GetComponent<MonsterStateManager>();
    }

    public override void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        manager.isHit = true;
        manager.isInjured = true;
        base.OnDamage(damage, hitPoint, hitNormal);
        stat.OnDamageParticle.transform.position = hitPoint;
        stat.OnDamageParticle.Play();
    }

    public override void RestoreHP(float restoreHP)
    {
        base.RestoreHP(restoreHP);
    }
}
