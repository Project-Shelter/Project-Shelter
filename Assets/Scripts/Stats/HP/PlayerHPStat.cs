using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPStat : HPStat
{
    private ActorStat stat;
    public Dictionary<AttackedPart, IBuff> DebuffsByPart { get; private set; } = new Dictionary<AttackedPart, IBuff>();
    public AttackedPart nowAttackedPart;

    private bool isTurningRed = false;
    private SpriteRenderer renderer;

    public PlayerHPStat(ActorStat stat, float minHP, float maxHP) : base(minHP, maxHP)
    {
        this.stat = stat;
        renderer = stat.GetComponent<SpriteRenderer>();

        DebuffsByPart[AttackedPart.Head] = null;
        DebuffsByPart[AttackedPart.Trunk] = new BleedingDebuff(stat, stat.bleedingDebuffValue.GetValue(), "BleedingDebuff");
        DebuffsByPart[AttackedPart.Leg] = new SlowDebuff(stat, stat.moveSpeedDebuffValue.GetValue(), "SlowDebuff");
        DebuffsByPart[AttackedPart.Arm] = new DamageDecreaseDebuff(stat, stat.damageDebuffValue.GetValue(), "DamageDecreaseDebuff");
        DebuffsByPart[AttackedPart.Normal] = null;
    }

    public enum AttackedPart
    {
        Head,
        Trunk,
        Leg,
        Arm,
        Normal
    }

    public override void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        nowAttackedPart = SelectAttackedPart();
        IBuff debuffByPart = DebuffsByPart[nowAttackedPart];
        if (nowAttackedPart == AttackedPart.Head)
            { damage *= 1.5f; }
        base.OnDamage(damage, hitPoint, hitNormal);
        stat.OnDamageEffect.transform.position = hitPoint;
        stat.OnDamageEffect.Play();

        // For Demo
        CoroutineHandler.StartStaticCoroutine(TurnRed(renderer, 0.2f));
        ///////////////

        if(debuffByPart != null) { stat.AddBuff(debuffByPart); }
        CheckInactivingDebuffs();
    }

    private IEnumerator TurnRed(SpriteRenderer renderer, float time)
    {
        float startTime = Time.time;
        while(startTime + time >= Time.time)
        {
            renderer.color = Color.red;
            yield return null;
        }
        renderer.color = Color.white;
        isTurningRed = false;
    }

    private AttackedPart SelectAttackedPart()
    {
        AttackedPart attackedPart;
        float attackedPartIndex = Random.value;
        if (attackedPartIndex <= stat.AttackedChances[(int)AttackedPart.Head])
        { attackedPart = AttackedPart.Head; }
        else if (attackedPartIndex <= stat.AttackedChances[(int)AttackedPart.Trunk])
        { attackedPart = AttackedPart.Trunk; }
        else if (attackedPartIndex <= stat.AttackedChances[(int)AttackedPart.Leg])
        { attackedPart = AttackedPart.Leg; }
        else if (attackedPartIndex <= stat.AttackedChances[(int)AttackedPart.Arm])
        { attackedPart = AttackedPart.Arm; }
        else
        { attackedPart = AttackedPart.Normal; }

        return attackedPart;
    }

    public override void Die()
    {
        base.Die();
    }

    private void CheckInactivingDebuffs()
    {
        if (HP >= MaxHP.GetValue() * stat.DEBUFF_HP_RATIO)
        {
            foreach (IBuff debuff in DebuffsByPart.Values)
            {
                if (debuff != null) { stat.RemoveBuff(debuff); }
            }
        }
    }

    public override void RestoreHP(float restoreHP)
    {
        base.RestoreHP(restoreHP);
        CheckInactivingDebuffs();
    }
}
