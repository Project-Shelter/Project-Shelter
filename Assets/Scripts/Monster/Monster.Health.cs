using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Monster : MonoBehaviour, ILivingEntity, IMovable
{
    public void OnDamage(float damage, Vector2 hitPoint, Vector2 hitNormal)
    {
        health.OnDamage(damage, hitPoint, hitNormal);
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
