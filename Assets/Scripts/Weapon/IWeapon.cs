using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    float AttackDelay { get; }
    void Attack();
}
