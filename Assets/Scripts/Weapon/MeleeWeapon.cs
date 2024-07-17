using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : IWeapon
{
    private Stat damage;
    private Stat attackDelay;
    private Stat attackRange;

    public IEnumerator Attack()
    {
        Debug.Log("Melee Attack");
        yield return new WaitForSeconds(attackDelay.GetValue());
    }
}
