using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : IMeleeWeapon
{
    public float AttackDelay => throw new System.NotImplementedException();

    private Stat damage;
    private Stat attackDelay;
    private Stat attackRange;

    public IEnumerator Attack()
    {
        Debug.Log("Melee Attack");
        yield return new WaitForSeconds(attackDelay.GetValue());
    }

    void IWeapon.Attack()
    {
        throw new System.NotImplementedException();
    }
}
