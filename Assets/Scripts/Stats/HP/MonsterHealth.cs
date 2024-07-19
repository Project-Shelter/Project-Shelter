using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : LivingEntity
{
    private Monster owner;

    private SpriteRenderer sprite;
    private ParticleSystem onDamageEffect;

    public MonsterHealth(Monster owner, float minHP, float maxHP) : base(minHP, maxHP)
    {
        this.owner = owner;
        sprite = owner.Sprite;
        onDamageEffect = Util.FindChild<ParticleSystem>(owner.gameObject, "OnDamageEffect");
    }

    public override void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        onDamageEffect.transform.position = hitPoint;
        onDamageEffect.Play();
        owner.StartCoroutine(TurnRed(0.2f)); // 임시로 넣어놓은 값

        owner.MoveBody.Knockback(hitNormal);
    }

    public override void RestoreHP(float restoreHP)
    {
        base.RestoreHP(restoreHP);
    }

    public override void Die()
    {
        base.Die();
    }

    private IEnumerator TurnRed(float time)
    {
        float startTime = Time.time;
        while (startTime + time >= Time.time)
        {
            sprite.color = Color.red;
            yield return null;
        }
        sprite.color = Color.white;
    }
}
