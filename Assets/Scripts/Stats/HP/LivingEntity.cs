using System;
using System.Collections;
using UnityEngine;

public class LivingEntity : ILivingEntity
{
    public Stat MaxHP { get; protected set; } = new Stat();
    public Stat MinHP { get; protected set; } = new Stat();
    public float HP { get; protected set; }
    public bool IsDead { get; protected set; }
    public event Action OnDeath;

    private Coroutine bleedingCoroutine;

    public LivingEntity(float minHP, float maxHP)
    {
        MinHP.baseValue = minHP;
        MaxHP.baseValue = maxHP;

        HP = MaxHP.GetValue();
        IsDead = false;
    }

    public virtual void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (IsDead) return;
        HP -= damage;

        if (HP <= MinHP.GetValue())
        {
            HP = MinHP.GetValue();
            Die();
        }
    }

    public virtual void RestoreHP(float restoreHP)
    {
        if (IsDead)
        {
            return;
        }

        if(HP + restoreHP > MaxHP.GetValue())
        {
            HP = MaxHP.GetValue();
        }
        else
        {
            HP += restoreHP;
        }
    }

    public virtual void Die()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }

        IsDead = true;
    }

    public virtual void Bleeding(float bleedingTick)
    {
        bleedingCoroutine = CoroutineHandler.StartStaticCoroutine(BleedingCoroutine(bleedingTick));
    }

    public virtual void StopBleeding()
    {
        if(bleedingCoroutine != null)
        {
            CoroutineHandler.StopStaticCoroutine(bleedingCoroutine);
        }
    }

    private IEnumerator BleedingCoroutine(float bleedingTick)
    {
        float lastBleedingTime = Time.time;
        while (true)
        {
            if (lastBleedingTime + bleedingTick <= Time.time)
            {
                lastBleedingTime = Time.time;
                HP--;

                if (HP <= MinHP.GetValue())
                {
                    HP = MinHP.GetValue();
                    Die();
                }
            }

            yield return null;
        }
    }
}