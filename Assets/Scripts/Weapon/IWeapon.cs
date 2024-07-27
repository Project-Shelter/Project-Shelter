using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Init(Actor owner);
    void SetActive(bool value);
    bool IsActived { get; }
    Action OnAttack { get; set; }
    float AttackDelay { get; }
    void Attack();
}
