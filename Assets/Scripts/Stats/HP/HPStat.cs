using System;
using System.Collections;
using UnityEngine;

public class HPStat : IHPStat
{
    public Stat MaxHP { get; protected set; } = new Stat();
    public float HP { get; protected set; }
    public Stat MinHP { get; protected set; } = new Stat();    
    public bool IsDead { get; protected set; }
    public event Action OnDeath;

    public HPStat(float minHP, float maxHP)
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

    public IEnumerator BleedingCoroutine(float lastBleedingTime, float timeBetBleeding)
    {
        while (true)
        {
            if (lastBleedingTime + timeBetBleeding <= Time.time)
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