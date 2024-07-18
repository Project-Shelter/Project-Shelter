using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackedPart
{
    Head,
    Trunk,
    Leg,
    Arm,
    Normal
}

public class ActorHealth : LivingEntity
{
    private Actor actor;

    private ParticleSystem onDamageEffect;
    public AttackedPart nowAttackedPart;
    private Dictionary<AttackedPart, IBuff> debuffsByPart = new();

    private SpriteRenderer sprite;

    public ActorHealth(Actor actor, float minHP, float maxHP) : base(minHP, maxHP)
    {
        this.actor = actor;
        sprite = Util.GetOrAddComponent<SpriteRenderer>(actor.gameObject);
        onDamageEffect = Util.FindChild<ParticleSystem>(actor.gameObject, "OnDamageEffect");

        debuffsByPart[AttackedPart.Head] = null;
        debuffsByPart[AttackedPart.Trunk] = new BleedingDebuff(actor, actor.Stat.bleedingDebuffVal.GetValue(), "BleedingDebuff");
        debuffsByPart[AttackedPart.Leg] = new SlowDebuff(actor.Stat.moveSpeed, actor.Stat.slowDebuffVal.GetValue(), "SlowDebuff");
        debuffsByPart[AttackedPart.Arm] = new DamageDecreaseDebuff(actor.Stat.attackDamage, actor.Stat.damageDebuffVal.GetValue(), "DamageDecreaseDebuff");
        debuffsByPart[AttackedPart.Normal] = null;
    }

    public override void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        nowAttackedPart = SelectAttackedPart();
        IBuff debuffByPart = debuffsByPart[nowAttackedPart];
        if (nowAttackedPart == AttackedPart.Head) { damage *= 1.5f; }
        base.OnDamage(damage, hitPoint, hitNormal);

        onDamageEffect.transform.position = hitPoint;
        onDamageEffect.Play();
        if(debuffByPart != null) { actor.Buff.AddBuff(debuffByPart); }
        actor.StartCoroutine(TurnRed(0.2f)); // 임시로 넣어놓은 값
    }

    private AttackedPart SelectAttackedPart()
    {
        AttackedPart attackedPart;
        float attackedPartIndex = Random.value;
        if (attackedPartIndex <= actor.Stat.AttackedChances[(int)AttackedPart.Head])
        { attackedPart = AttackedPart.Head; }
        else if (attackedPartIndex <= actor.Stat.AttackedChances[(int)AttackedPart.Trunk])
        { attackedPart = AttackedPart.Trunk; }
        else if (attackedPartIndex <= actor.Stat.AttackedChances[(int)AttackedPart.Leg])
        { attackedPart = AttackedPart.Leg; }
        else if (attackedPartIndex <= actor.Stat.AttackedChances[(int)AttackedPart.Arm])
        { attackedPart = AttackedPart.Arm; }
        else
        { attackedPart = AttackedPart.Normal; }

        return attackedPart;
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

    public override void Die()
    {
        base.Die();
    }

    private void CheckHPOver()
    {
        if (HP >= MaxHP.GetValue() * actor.Stat.debuffHPRatio)
        {
            foreach (IBuff debuff in debuffsByPart.Values)
            {
                if (debuff != null) { actor.Buff.RemoveBuff(debuff); }
            }
        }
    }

    public override void RestoreHP(float restoreHP)
    {
        base.RestoreHP(restoreHP);
        CheckHPOver();
    }
}
