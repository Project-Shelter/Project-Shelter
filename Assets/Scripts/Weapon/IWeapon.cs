using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Init(Actor owner);
    Action OnAttack { get; set; }
    float AttackDelay { get; }
    void Attack();
}
