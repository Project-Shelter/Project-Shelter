using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : IWeapon
{
    private Stat damage;
    private Stat attackDelay;
    private Stat attackRange;
    
    private Stat maxAmmo;
    private Stat currentAmmo;

    public IEnumerator Attack()
    {
        Debug.Log("Range Attack");
        yield return new WaitForSeconds(attackDelay.GetValue());
    }
}
