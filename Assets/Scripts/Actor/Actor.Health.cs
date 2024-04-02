using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Actor : MonoBehaviour, ILivingEntity
{
    public Action<AttackedPart> AttackedAction = null;

    private void InitHelath()
    {
        if (isHumanActor)
        {
            // 인간 캐릭터 사망시 행동
            health.OnDeath += () => Debug.Log("Gameover");
        }
        else
        {
            // 기계 캐릭터 사망시 행동
        }
    }

    public void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        health.OnDamage(damage, hitPoint, hitNormal);
        if (health.nowAttackedPart != AttackedPart.Normal)
        {
            AttackedAction?.Invoke(health.nowAttackedPart);
            if (health.nowAttackedPart != AttackedPart.Normal)
            {
                AttackedAction?.Invoke(health.nowAttackedPart);
            }
        }
    }

    public void RestoreHP(float restoreHP)
    {
        health.RestoreHP(restoreHP);
    }

    public void Die()
    {
        health.Die();
    }
    public void Bleeding(float bleedingTick)
    {
        health.Bleeding(bleedingTick);
    }

    public void StopBleeding()
    {
        health.StopBleeding();
    }
}