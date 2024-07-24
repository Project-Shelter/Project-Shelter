using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeleeWeapon : IWeapon
{
    float AfterAttackDelay { get; }
    void AfterAttack();
    void EndAttack();
}
